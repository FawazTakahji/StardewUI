﻿using Microsoft.Xna.Framework;
using StardewUI;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;
using StardewValley;
using System.ComponentModel;
using Xunit.Abstractions;

namespace StarML.Tests;

public class BindingTests
{
    class Model
    {
        public Color Color { get; set; }

        public string Name { get; set; } = "";
    }

    class ModelWithNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Color Color
        {
            get => color;
            set
            {
                if (value == color)
                {
                    return;
                }
                color = value;
                PropertyChanged?.Invoke(this, new(nameof(Color)));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                PropertyChanged?.Invoke(this, new(nameof(Name)));
            }
        }

        private Color color;
        private string name = "";
    }

    class FakeAssetCache : IAssetCache
    {
        private readonly Dictionary<string, object> assets = [];

        public IAssetCacheEntry<T> Get<T>(string name) where T : notnull
        {
            return assets.TryGetValue(name, out var asset)
                ? new FakeAssetCacheEntry<T>((T)asset)
                : throw new KeyNotFoundException($"Asset '{name}' not registered.");
        }

        public void Put<T>(string name, T asset)
            where T : notnull
        {
            assets[name] = asset;
        }
    }

    class FakeAssetCacheEntry<T>(T asset) : IAssetCacheEntry<T>
    {
        public T Asset { get; } = asset;

        public bool IsExpired => false;
    }

    private readonly FakeAssetCache assetCache;
    private readonly ITestOutputHelper output;
    private readonly IViewFactory viewFactory;
    private readonly IViewBinder viewBinder;

    public BindingTests(ITestOutputHelper output)
    {
        this.output = output;
        viewFactory = new ViewFactory();
        assetCache = new FakeAssetCache();
        var valueSourceFactory = new ValueSourceFactory(assetCache);
        var valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        viewBinder = new ReflectionViewBinder(attributeBindingFactory);
    }

    [Fact]
    public void Update_WithInputBindings_WritesContextToView()
    {
        var element = new SElement("label", [
            new SAttribute("max-lines", AttributeValueType.Literal, "1"),
            new SAttribute("color", AttributeValueType.InputBinding, "Color"),
            new SAttribute("text", AttributeValueType.InputBinding, "Name"),
        ]);
        var view = viewFactory.CreateView(element.Tag);
        var model = new ModelWithNotify() { Name = "Test text", Color = Color.Blue };
        using var viewBinding = viewBinder.Bind(view, element, model);

        var label = (Label)view;
        Assert.Equal(1, label.MaxLines);
        Assert.Equal("Test text", label.Text);
        Assert.Equal(Color.Blue, label.Color);

        model.Name = "New text";
        viewBinding.Update();
        Assert.Equal("New text", label.Text);
    }

    class OutputBindingTestModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Checked
        {
            get => isChecked;
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    PropertyChanged?.Invoke(this, new(nameof(Checked)));
                }
            }
        }
        public Vector2 Size
        {
            get => size;
            set
            {
                if (value != size)
                {
                    size = value;
                    PropertyChanged?.Invoke(this, new(nameof(Size)));
                }
            }
        }

        private bool isChecked;
        private Vector2 size;
    }

    [Fact]
    public void Update_WithOutputBindings_WritesViewToContext()
    {
        var element = new SElement("checkbox", [
            new SAttribute("layout", AttributeValueType.Literal, "200px 20px"),
            new SAttribute("is-checked", AttributeValueType.OutputBinding, "Checked"),
            new SAttribute("outer-size", AttributeValueType.OutputBinding, "Size"),
        ]);
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = false, Size = Vector2.Zero };
        using var viewBinding = viewBinder.Bind(view, element, model);

        // Initial bind should generally not cause immediate output sync, because we assume the view isn't completely
        // stable or fully initialized yet.

        Assert.False(model.Checked);
        Assert.Equal(Vector2.Zero, model.Size);

        var checkbox = (CheckBox)view;
        checkbox.Measure(new Vector2(1000, 1000));
        viewBinding.Update();
        Assert.False(model.Checked);
        Assert.Equal(new Vector2(200, 20), model.Size);

        checkbox.IsChecked = true;
        viewBinding.Update();
        Assert.True(model.Checked);
        Assert.Equal(new Vector2(200, 20), model.Size);
    }

    [Fact]
    public void Update_WithInOutBindings_WritesBothDirections()
    {
        var element = new SElement("checkbox", [
            new SAttribute("is-checked", AttributeValueType.TwoWayBinding, "Checked"),
        ]);
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = true };
        using var viewBinding = viewBinder.Bind(view, element, model);

        var checkbox = (CheckBox)view;
        Assert.True(model.Checked);
        Assert.True(checkbox.IsChecked);

        // No changes, nothing should happen here.
        viewBinding.Update();
        Assert.True(model.Checked);
        Assert.True(checkbox.IsChecked);

        // Simulate click to uncheck
        checkbox.IsChecked = false;
        viewBinding.Update();
        Assert.False(model.Checked);

        // Now the context is updated from some other source
        model.Checked = true;
        viewBinding.Update();
        Assert.True(checkbox.IsChecked);
    }

    [Fact]
    public void TestNodes()
    {
        assetCache.Put("TestSprite", UiSprites.ButtonDark);

        var root = new SElement("lane", [
            new SAttribute("orientation", AttributeValueType.Literal, "vertical"),
            new SAttribute("horizontal-content-alignment", AttributeValueType.Literal, "middle"),
            new SAttribute("vertical-content-alignment", AttributeValueType.Literal, "end"),
        ]);
        var child1 = new SElement("image", [
            // TODO: How can we handle complex attributes like the Layout in particular?
            //new SAttribute("width", AttributeValueType.Literal, "400"),
            new SAttribute("scale", AttributeValueType.Literal, "3.0"),
            new SAttribute("sprite", AttributeValueType.AssetBinding, "TestSprite"),
        ]);
        var child2 = new SElement("label", [
            new SAttribute("font", AttributeValueType.Literal, "dialogue"),
            new SAttribute("text", AttributeValueType.InputBinding, "HeaderText"),
        ]);
        var tree = new ViewNode(
            viewFactory, viewBinder, root, [
                new ViewNode(viewFactory, viewBinder, child1, []),
                new ViewNode(viewFactory, viewBinder, child2, []),
            ]);
        tree.Update();

        var rootView = tree.View as Lane;
        Assert.NotNull(rootView);
        Assert.Equal(Orientation.Vertical, rootView.Orientation);
        Assert.Equal(Alignment.Middle, rootView.HorizontalContentAlignment);
        Assert.Equal(Alignment.End, rootView.VerticalContentAlignment);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var image = Assert.IsType<Image>(child);
                Assert.Equal(3.0f, image.Scale);
                Assert.Equal(UiSprites.ButtonDark, image.Sprite);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Game1.dialogueFont, label.Font);
                // TODO: Should really be empty string, not null, because type is non-nullable. But
                // this is hard to figure out with reflection-based bindings.
                // Assert.Equal("", label.Text);
                Assert.Null(label.Text);
            });

        var model = new { HeaderText = "Some text" };
        tree.Context = model;
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }

    [Fact]
    public void TestEndToEnd()
    {
        var viewNodeFactory = new ViewNodeFactory(viewFactory, viewBinder);
        assetCache.Put("Mods/TestMod/TestSprite", UiSprites.SmallTrashCan);

        string markup =
            @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"" vertical-content-alignment=""end"">
                <image scale=""3.5"" sprite={{@Mods/TestMod/TestSprite}} />
                <label font=""dialogue"" text={{HeaderText}} />
            </lane>";
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document.Root);
        tree.Update();

        var rootView = tree.View as Lane;
        Assert.NotNull(rootView);
        Assert.Equal(Orientation.Vertical, rootView.Orientation);
        Assert.Equal(Alignment.Middle, rootView.HorizontalContentAlignment);
        Assert.Equal(Alignment.End, rootView.VerticalContentAlignment);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var image = Assert.IsType<Image>(child);
                Assert.Equal(3.5f, image.Scale);
                Assert.Equal(UiSprites.SmallTrashCan, image.Sprite);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Game1.dialogueFont, label.Font);
                // TODO: Should really be empty string, not null, because type is non-nullable. But
                // this is hard to figure out with reflection-based bindings.
                // Assert.Equal("", label.Text);
                Assert.Null(label.Text);
            });

        var model = new { HeaderText = "Some text" };
        tree.Context = model;
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }
}
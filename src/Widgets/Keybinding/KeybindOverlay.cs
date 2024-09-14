﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Overlay control for editing a keybinding, or list of bindings.
/// </summary>
/// <param name="spriteMap">Map of bindable buttons to sprite representations.</param>
/// <param name="emptyText">Text to display when the keybind is empty (has no buttons).</param>
public class KeybindOverlay(ISpriteMap<SButton> spriteMap, string emptyText) : FullScreenOverlay
{
    /// <summary>
    /// The current keybinds to display in the list.
    /// </summary>
    public KeybindList KeybindList
    {
        get => keybindList;
        set
        {
            if (value == keybindList)
            {
                return;
            }
            keybindList = value;
            UpdateKeybinds();
        }
    }

    private static readonly HashSet<SButton> bannedButtons =
    [
        SButton.Escape,
        SButton.LeftThumbstickUp,
        SButton.LeftThumbstickDown,
        SButton.LeftThumbstickLeft,
        SButton.LeftThumbstickRight,
        SButton.RightThumbstickUp,
        SButton.RightThumbstickDown,
        SButton.RightThumbstickLeft,
        SButton.RightThumbstickRight,
    ];

    private readonly HashSet<SButton> capturedButtons = [];

    private readonly Image horizontalDivider =
        new()
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = new(0, 16),
            Fit = ImageFit.Stretch,
            Sprite = UiSprites.GenericHorizontalDivider,
        };

    private KeybindList keybindList = new();

    // Initialized in CreateView
    private Button addButton = null!;
    private Animator<Frame, Color> capturingAnimator = null!;
    private KeybindView currentKeybindView = null!;
    private Frame keybindEntryHighlighter = null!;
    private Lane keybindsLane = null!;

    protected override IView CreateView()
    {
        currentKeybindView = new(spriteMap, "");
        addButton = new()
        {
            Layout = LayoutParameters.FitContent(),
            Content = new Image()
            {
                Layout = LayoutParameters.FixedSize(20, 20),
                Margin = new(0, 4),
                Sprite = UiSprites.SmallGreenPlus,
                Tint = new(0.2f, 0.5f, 1f),
            },
        };
        addButton.LeftClick += AddButton_LeftClick;
        var currentKeybindLane = new Lane()
        {
            Layout = new()
            {
                Width = Length.Stretch(),
                Height = Length.Content(),
                MinHeight = 64,
            },
            VerticalContentAlignment = Alignment.Middle,
            Children = [currentKeybindView, new Spacer() { Layout = LayoutParameters.AutoRow() }, addButton],
        };
        keybindEntryHighlighter = new Frame()
        {
            Layout = LayoutParameters.AutoRow(),
            Background = new(Game1.staminaRect),
            BackgroundTint = Color.Transparent,
            Content = currentKeybindLane,
        };
        capturingAnimator = new(
            keybindEntryHighlighter,
            frame => frame.BackgroundTint,
            AlphaLerp,
            (frame, color) => frame.BackgroundTint = color
        )
        {
            AutoReverse = true,
            Loop = true,
        };
        keybindsLane = new() { Layout = LayoutParameters.AutoRow(), Orientation = Orientation.Vertical };
        UpdateKeybinds();
        return new Frame()
        {
            Layout = new() { Width = Length.Px(640), Height = Length.Content() },
            Border = UiSprites.ControlBorder,
            Padding = UiSprites.ControlBorder.FixedEdges! + new Edges(8),
            Content = new Lane()
            {
                Layout = LayoutParameters.AutoRow(),
                Orientation = Orientation.Vertical,
                Children = [keybindEntryHighlighter, horizontalDivider, keybindsLane],
            },
        };
    }

    public override void Update(TimeSpan elapsed)
    {
        if (!CapturingInput)
        {
            return;
        }
        var cancellationButtons = Game1.options.menuButton.Select(ib => ib.ToSButton()).Append(SButton.ControllerB);
        foreach (var button in cancellationButtons)
        {
            if (UI.InputHelper.IsDown(button))
            {
                StopCapturing();
                UI.InputHelper.Suppress(button);
                return;
            }
        }
        int previousCapturedCount = capturedButtons.Count;
        var pressedButtons = GetPressedButtons().Where(IsBindable);
        bool anyPressed = false;
        foreach (var button in pressedButtons)
        {
            capturedButtons.Add(button);
            anyPressed = true;
        }
        if (capturedButtons.Count != previousCapturedCount)
        {
            currentKeybindView.Keybind = new([.. capturedButtons]);
        }
        if (capturedButtons.Count > 0 && !anyPressed)
        {
            var capturedKeybind = new Keybind([.. capturedButtons]);
            StopCapturing();
            if (capturedKeybind.IsBound)
            {
                KeybindList = new([.. KeybindList.Keybinds, capturedKeybind]);
            }
        }
    }

    private void AddButton_LeftClick(object? sender, ClickEventArgs e)
    {
        if (!e.IsPrimaryButton())
        {
            return;
        }
        Game1.playSound("drumkit6");
        UI.InputHelper.Suppress(e.Button);
        StartCapturing();
    }

    private void AddKeybindRow(Keybind keybind)
    {
        var keybindView = new KeybindView(spriteMap, emptyText)
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = keybindsLane.Children.Count > 0 ? new(Top: 16) : Edges.NONE,
            Keybind = keybind,
        };
        var deleteButton = new Image()
        {
            Layout = new() { Width = Length.Content(), Height = Length.Px(40) },
            Sprite = UiSprites.SmallTrashCan,
            ShadowAlpha = 0.35f,
            ShadowOffset = new(-3, 4),
            IsFocusable = true,
            Tags = Tags.Create(keybind),
        };
        deleteButton.LeftClick += DeleteButton_LeftClick;
        var row = new Lane()
        {
            Layout = LayoutParameters.AutoRow(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [keybindView, new Spacer() { Layout = LayoutParameters.AutoRow() }, deleteButton],
        };
        keybindsLane.Children.Add(row);
    }

    private static Color AlphaLerp(Color color1, Color color2, float amount)
    {
        amount = MathHelper.Clamp(amount, 0f, 1f);
        var alpha = MathHelper.Lerp(color1.A, color2.A, amount) / 255f;
        return new((int)(color2.R * alpha), (int)(color2.G * alpha), (int)(color2.B * alpha), (int)(alpha * 255));
    }

    private void DeleteButton_LeftClick(object? sender, ClickEventArgs e)
    {
        if (sender is not IView view)
        {
            return;
        }
        var keybind = view.Tags.Get<Keybind>();
        KeybindList = new(KeybindList.Keybinds.Where(kb => kb != keybind).ToArray());
    }

    private static IEnumerable<SButton> GetPressedButtons()
    {
        foreach (var key in Game1.input.GetKeyboardState().GetPressedKeys())
        {
            yield return key.ToSButton();
        }
        if (Game1.options.gamepadControls)
        {
            var gamepadState = Game1.input.GetGamePadState();
            var heldButtons = Utility.getHeldButtons(gamepadState).GetEnumerator();
            while (heldButtons.MoveNext())
            {
                yield return heldButtons.Current.ToSButton();
            }
            if (gamepadState.Buttons.LeftStick == ButtonState.Pressed)
            {
                yield return SButton.LeftStick;
            }
            if (gamepadState.Buttons.RightStick == ButtonState.Pressed)
            {
                yield return SButton.RightStick;
            }
        }
    }

    private static bool IsBindable(SButton button)
    {
        return !bannedButtons.Contains(button);
    }

    private void StartCapturing()
    {
        if (CapturingInput)
        {
            return;
        }
        addButton.Visibility = Visibility.Hidden;
        capturedButtons.Clear();
        currentKeybindView.Keybind = new();
        var endColor = Color.DarkOrange;
        var startColor = AlphaLerp(Color.Transparent, endColor, 0.3f);
        capturingAnimator.Start(startColor, endColor, TimeSpan.FromSeconds(1));
        CapturingInput = true;
    }

    private void StopCapturing()
    {
        Game1.playSound("drumkit6");
        CapturingInput = false;
        capturingAnimator.Stop();
        keybindEntryHighlighter.BackgroundTint = Color.Transparent;
        addButton.Visibility = Visibility.Visible;
        capturedButtons.Clear();
        currentKeybindView.Keybind = new();
    }

    private void UpdateKeybinds()
    {
        if (keybindsLane is null)
        {
            return;
        }
        keybindsLane.Children.Clear();
        foreach (var keybind in KeybindList.Keybinds)
        {
            if (keybind.IsBound)
            {
                AddKeybindRow(keybind);
            }
        }
    }
}
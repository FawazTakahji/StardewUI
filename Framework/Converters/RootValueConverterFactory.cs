﻿using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Converter factory used as the root in StardewUI apps, including all built-in converters.
/// </summary>
internal class RootValueConverterFactory : ValueConverterFactory
{
    /// <summary>
    /// Initializes a new <see cref="RootValueConverterFactory"/> with the specified add-on factories.
    /// </summary>
    /// <param name="addonFactories">Converter factories registered by add-ons, in order of priority. All add-on
    /// factories are considered after the standard conversions, but before fallback conversions (duck-type,
    /// ToString).</param>
    public RootValueConverterFactory(IEnumerable<IValueConverterFactory> addonFactories)
    {
        RegisterDefaults();
        Factories.AddRange(addonFactories);
        RegisterFallbackDefaults();
    }

    // High-priority defaults that take precedence over user-defined conversions. These are all documented, tested,
    // etc., and users are likely to depend on their default behavior; therefore we register them first so that add-ons
    // cannot override them.
    private void RegisterDefaults()
    {
        // Automatically register string to primitive conversions.
        TryRegister<string, byte>(byte.Parse);
        TryRegister<string, sbyte>(sbyte.Parse);
        TryRegister<string, short>(short.Parse);
        TryRegister<string, ushort>(ushort.Parse);
        TryRegister<string, int>(int.Parse);
        TryRegister<string, uint>(uint.Parse);
        TryRegister<string, long>(long.Parse);
        TryRegister<string, ulong>(ulong.Parse);
        TryRegister<string, decimal>(decimal.Parse);
        TryRegister<string, float>(float.Parse);
        TryRegister<string, double>(double.Parse);
        TryRegister<string, bool>(bool.Parse);

        // Convenience defaults for non-primitive types that are commonly specified as literals.
        TryRegister(new ColorConverter());
        TryRegister<string, Edges>(Edges.Parse);
        TryRegister(new GridItemLayoutConverter());
        TryRegister(new LayoutConverter());
        TryRegister<string, Length>(Length.Parse);
        TryRegister(new NamedFontConverter());
        TryRegister(new PointConverter());
        TryRegister(new RectangleConverter());
        TryRegister(new Vector2Converter());

        // Edges are better to bind as numbers, so we can use tuples and XNA equivalents in some cases.
        TryRegister<int, Edges>(all => new(all));
        TryRegister<Tuple<int, int>, Edges>(t => new(t.Item1, t.Item2));
        TryRegister<Point, Edges>(p => new(p.X, p.Y));
        TryRegister<Vector2, Edges>(v => new((int)v.X, (int)v.Y));
        TryRegister<Tuple<int, int, int, int>, Edges>(t => new(t.Item1, t.Item2, t.Item3, t.Item4));
        TryRegister<Tuple<Point, Point>, Edges>(t => new(t.Item1.X, t.Item1.Y, t.Item2.X, t.Item2.Y));
        TryRegister<Tuple<Vector2, Vector2>, Edges>(t =>
            new((int)t.Item1.X, (int)t.Item1.Y, (int)t.Item2.X, (int)t.Item2.Y)
        );
        TryRegister<Vector4, Edges>(v => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W));
        // And the reverse conversions, where applicable...
        TryRegister<Edges, Tuple<int, int, int, int>>(e => Tuple.Create(e.Left, e.Top, e.Right, e.Bottom));
        TryRegister<Edges, Vector4>(e => new(e.Left, e.Top, e.Right, e.Bottom));

        // Bounds are similar to edges, except we never accept them as inputs, so only need reverse conversions.
        TryRegister<Bounds, Tuple<float, float, float, float>>(b => Tuple.Create(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Tuple<Vector2, Vector2>>(b => Tuple.Create(b.Position, b.Size));
        TryRegister<Bounds, Vector4>(b => new(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Rectangle>(b => new(b.Position.ToPoint(), b.Size.ToPoint()));

        // Several converters for just the Sprite type, as it can be surprisingly complex and users won't have the exact
        // type.
        TryRegister(new ItemSpriteConverter());
        TryRegister(new TextureSpriteConverter());
        TryRegister(new TextureRectSpriteConverter());

        // Most enums are fine using the standard string-to-enum conversion.
        Register(new EnumNameConverterFactory());

        // If source and destination are the same, use a pass-through converter.
        Register(new IdentityValueConverterFactory());
        Register(new AnyCastConverterFactory());
        Register(new CastingValueConverterFactory());
        Register(new NullableConverterFactory(this));
    }

    // Converters that have lower priority than user-defined conversions; we only use these when there is definitely no
    // better option available.
    private void RegisterFallbackDefaults()
    {
        // Anything can generally be converted to a string using the default converter.
        Register(new StringConverterFactory());

        // Duck typing is the most expensive to even attempt, regardless of success. Don't do it unless we're sure
        // there aren't any other conversion options.
        Register(new DuckTypeEnumConverterFactory());
        Register(new DuckTypeClassConverterFactory(this));
    }
}

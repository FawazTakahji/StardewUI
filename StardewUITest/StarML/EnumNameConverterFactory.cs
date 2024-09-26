﻿using System.Diagnostics.CodeAnalysis;

namespace StardewUITest.StarML;

/// <summary>
/// Factory that automatically implements string-to-enum conversions based on the case-insensitive enum names.
/// </summary>
public class EnumNameConverterFactory : IValueConverterFactory
{
    public bool TryGetConverter<TSource, TDestination>([MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter)
    {
        if (typeof(TSource) != typeof(string) || !typeof(TDestination).IsEnum)
        {
            converter = null;
            return false;
        }
        converter = (IValueConverter<TSource, TDestination>)typeof(Converter<>).MakeGenericType(typeof(TDestination))
            .GetConstructor([])!
            .Invoke([]);
        return true;
    }

    class Converter<T> : IValueConverter<string, T> where T : struct, Enum
    {
        public T Convert(string value)
        {
            return Enum.Parse<T>(value, true);
        }
    }
}
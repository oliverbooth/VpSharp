namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter.
/// </summary>
public abstract class ValueConverter
{
    /// <summary>
    ///     Gets the type that the converter converts to.
    /// </summary>
    /// <value>The type that the converter converts to.</value>
    public abstract Type Type { get; }

    /// <summary>
    ///     Determines whether the converter can convert the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <returns>
    ///     <see langword="true" /> if the converter can convert the specified type; otherwise, <see langword="false" />.
    /// </returns>
    public abstract bool CanConvert(Type typeToConvert);

    /// <summary>
    ///     Reads the specified value and converts it to the converter's type.
    /// </summary>
    /// <param name="reader">The reader from which the value will be read.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The converted value.</returns>
    public abstract object? ReadValue(ref Utf16ValueStringReader reader, Type typeToConvert, ActionSerializerOptions options);
}

/// <summary>
///     Represents a value converter.
/// </summary>
public abstract class ValueConverter<T> : ValueConverter
{
    /// <inheritdoc />
    public override Type Type { get; } = typeof(T);

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(T).IsAssignableFrom(typeToConvert);
    }

    /// <inheritdoc />
    public override object? ReadValue(ref Utf16ValueStringReader reader, Type typeToConvert, ActionSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new InvalidOperationException($"Cannot convert to type '{typeToConvert}'.");
        }

        return ReadValue(ref reader, options);
    }

    /// <summary>
    ///     Reads the specified value and converts it to the converter's type.
    /// </summary>
    /// <param name="reader">The reader from which the value will be read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The converted value.</returns>
    public abstract T ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options);
}

using VpSharp.Internal.ValueConverters;

#pragma warning disable 612
namespace VpSharp.Internal.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class ValueConverterAttribute : Attribute
{
    /// <inheritdoc />
    public ValueConverterAttribute(Type converterType)
        : this(converterType, null)
    {
        UseArgs = false;
    }

    /// <inheritdoc />
    public ValueConverterAttribute(Type converterType, params object?[]? args)
    {
        if (converterType is null)
            throw new ArgumentNullException(nameof(converterType));

        if (converterType.IsAbstract)
            throw new ArgumentException("Cannot use abstract converter.");

        if (!converterType.IsSubclassOf(typeof(ValueConverter)))
            throw new ArgumentException($"Converter does not inherit {typeof(ValueConverter)}");

        ConverterType = converterType;
        Args = args;
        UseArgs = true;
    }

    /// <summary>
    ///     Gets the converter type.
    /// </summary>
    /// <value>The converter type.</value>
    public Type ConverterType { get; }

    /// <summary>
    ///     Gets the arguments to pass to the constructor, if any.
    /// </summary>
    /// <value>Arguments to pass to the constructor.</value>
    public object?[]? Args { get; }

    /// <summary>
    ///     Gets a value indicating whether arguments were supplied.
    /// </summary>
    /// <value><see langword="true" /> if arguments were supplied; otherwise, <see langword="false" />.</value>
    public bool UseArgs { get; }
}

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Specifies the type of the value converter to use for a property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ValueConverterAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValueConverterAttribute" /> class.
    /// </summary>
    /// <param name="converterType">The type of the value converter.</param>
    public ValueConverterAttribute(Type converterType)
    {
        ConverterType = converterType;
    }

    /// <summary>
    ///     Gets the type of the value converter.
    /// </summary>
    /// <value>The type of the value converter.</value>
    public Type ConverterType { get; }
}

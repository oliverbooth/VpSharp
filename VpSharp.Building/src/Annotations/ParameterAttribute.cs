namespace VpSharp.Building.Annotations;

/// <summary>
///     Denotes an ordered parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ParameterAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ParameterAttribute" /> class.
    /// </summary>
    /// <param name="value">The index value greater than or equal to zero.</param>
    /// <param name="fromEnd"><see langword="true" /> to index from the end; otherwise, <see langword="false" />.</param>
    public ParameterAttribute(int value, bool fromEnd = false)
    {
        Index = new Index(value, fromEnd);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ParameterAttribute" /> class.
    /// </summary>
    /// <param name="index">The parameter index.</param>
    public ParameterAttribute(Index index)
    {
        Index = index;
    }

    /// <summary>
    ///     Gets the parameter index.
    /// </summary>
    /// <value>The parameter index.</value>
    public Index Index { get; }

    /// <summary>
    ///     Gets or initializes a value indicating whether this parameter is optional.
    /// </summary>
    /// <value><see langword="true" /> if this parameter is optional; otherwise, <see langword="false" />.</value>
    public bool IsOptional { get; init; }
}

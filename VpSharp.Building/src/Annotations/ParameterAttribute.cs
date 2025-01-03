using VpSharp.Building.Commands;

namespace VpSharp.Building.Annotations;

/// <summary>
///     Specifies that this member is a parameter in a <see cref="VirtualParadiseCommand" />.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ParameterAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ParameterAttribute" /> class.
    /// </summary>
    /// <param name="order">The order of the parameter.</param>
    public ParameterAttribute(int order)
    {
        Order = order;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether this parameter is optional.
    /// </summary>
    /// <value><see langword="true" /> if this parameter is optional; otherwise, <see langword="false" />.</value>
    public bool IsOptional { get; init; }

    /// <summary>
    ///     Gets the order of the parameter.
    /// </summary>
    /// <value>The order of the parameter.</value>
    public int Order { get; }
}

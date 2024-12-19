namespace VpSharp.Building.Annotations;

/// <summary>
///     Specifies that this member is an action property in the form of <c>key=value</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class PropertyAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyAttribute" /> class.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public PropertyAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    ///     Gets the property name.
    /// </summary>
    /// <value>The property name.</value>
    public string Name { get; }
}

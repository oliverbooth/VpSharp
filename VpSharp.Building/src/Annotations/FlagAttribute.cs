namespace VpSharp.Building.Annotations;

/// <summary>
///     Specifies that a property represents a flag, whose presence indicates a <see langword="true" /> value.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FlagAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FlagAttribute" /> class.
    /// </summary>
    /// <param name="name">The name of the flag.</param>
    public FlagAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     Gets the name of the flag.
    /// </summary>
    /// <value>The name of the flag.</value>
    public string Name { get; }
}

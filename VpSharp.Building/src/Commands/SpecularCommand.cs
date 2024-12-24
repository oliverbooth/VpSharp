using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>specular</c> command.
/// </summary>
[Command("specular")]
public sealed class SpecularCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets a value indicating whether to apply specular highlights onto alpha transparent objects.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if specular highlights are applied onto alpha transparent objects; otherwise,
    ///     <see langword="false" />.
    /// </value>
    [Flag("alpha")]
    public bool Alpha { get; set; }

    /// <summary>
    ///     Gets or sets the specular value.
    /// </summary>
    /// <value>The specular value.</value>
    [Parameter(0)]
    public double Intensity { get; set; } = 1.0;

    /// <summary>
    ///     Gets or sets the shininess value.
    /// </summary>
    /// <value>The shininess value.</value>
    [Parameter(1, IsOptional = true)]
    public double Shininess { get; set; } = 30.0;

    /// <summary>
    ///     Gets or sets the tag to which the specular intensity is applied.
    /// </summary>
    /// <value>The tag to which the specular intensity is applied.</value>
    [Property("tag")]
    public string? Tag { get; set; }
}

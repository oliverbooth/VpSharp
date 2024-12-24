using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents a command.
/// </summary>
public abstract class VirtualParadiseCommand
{
    /// <summary>
    ///     Gets the raw arguments passed to this command.
    /// </summary>
    /// <value>The raw arguments.</value>
    public IList<string> RawArguments { get; internal set; } = [];

    /// <summary>
    ///     Gets the raw arguments passed to this command.
    /// </summary>
    /// <value>The raw arguments.</value>
    public IDictionary<string, string> RawProperties { get; internal set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Gets the raw argument string passed to this command.
    /// </summary>
    /// <value>The raw argument string.</value>
    public string RawArgumentString
    {
        get => string.Join(' ', RawArguments);
    }

    /// <summary>
    ///     Gets the target name.
    /// </summary>
    /// <value>The target name.</value>
    [Property("name")]
    public string? ExecuteAs { get; internal set; }
}

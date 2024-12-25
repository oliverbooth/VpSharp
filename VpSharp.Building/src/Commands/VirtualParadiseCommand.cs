using Optional;
using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents a command.
/// </summary>
public abstract class VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets a value indicating whether the command is globally applied.
    /// </summary>
    /// <value><see langword="true" /> if the command is globally applied; otherwise, <see langword="false" />.</value>
    [Flag("global")]
    public bool IsGlobal { get; set; }

    /// <summary>
    ///     Gets the raw arguments passed to this command.
    /// </summary>
    /// <value>The raw arguments.</value>
    public IList<string> RawArguments { get; protected internal set; } = [];

    /// <summary>
    ///     Gets the raw arguments passed to this command.
    /// </summary>
    /// <value>The raw arguments.</value>
    public IDictionary<string, string> RawProperties { get; protected internal set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Gets the raw argument string passed to this command.
    /// </summary>
    /// <value>The raw argument string.</value>
    public string RawArgumentString
    {
        get => string.Join(' ', RawArguments);
        protected internal set => RawArguments = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    ///     Gets the target name.
    /// </summary>
    /// <value>The target name.</value>
    [Property("name")]
    public Option<string> ExecuteAs { get; internal set; }
}

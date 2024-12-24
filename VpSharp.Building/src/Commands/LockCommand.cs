using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>lock</c> command.
/// </summary>
[Command("lock")]
public sealed class LockCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the ID of the owners who can use the locked object.
    /// </summary>
    /// <value>The ID of the owners who can use the locked object.</value>
    public IReadOnlyList<int> Owners
    {
        get => RawOwners.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        set => RawOwners = string.Join(':', value);
    }

    [Property("owners")] private string RawOwners { get; set; } = string.Empty;
}

namespace VpSharp.Entities;

/// <summary>
///     Represents a zone object.
/// </summary>
public sealed class VirtualParadiseZoneObject : VirtualParadiseObject
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseZoneObject" /> class.
    /// </summary>
    /// <param name="client">The owning client.</param>
    /// <param name="id">The object ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="client" /> is <see langword="null" />.</exception>
    internal VirtualParadiseZoneObject(VirtualParadiseClient client, int id)
        : base(client, id)
    {
    }
}

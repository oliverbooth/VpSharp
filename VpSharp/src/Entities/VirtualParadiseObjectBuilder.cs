using VpSharp.Internal;

namespace VpSharp.Entities;

/// <summary>
///     Represents the base class for object builders.
/// </summary>
public abstract class VirtualParadiseObjectBuilder
{
    private protected VirtualParadiseObjectBuilder(VirtualParadiseClient client, ObjectBuilderMode mode)
    {
        Client = client;
        Mode = mode;
    }

    private protected VirtualParadiseClient Client { get; }

    private protected ObjectBuilderMode Mode { get; }
}

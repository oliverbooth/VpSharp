using VpSharp.Internal;

namespace VpSharp.Entities;

/// <summary>
///     Represents the base class for object builders.
/// </summary>
public abstract class VirtualParadiseObjectBuilder
{
    private protected VirtualParadiseObjectBuilder(
        VirtualParadiseClient client,
        VirtualParadiseObject targetObject,
        ObjectBuilderMode mode
    )
    {
        Client = client;
        TargetObject = targetObject;
        Mode = mode;
    }

    private protected VirtualParadiseClient Client { get; }

    private protected ObjectBuilderMode Mode { get; }

    private protected VirtualParadiseObject TargetObject { get; }
}

using Optional;
using VpSharp.Internal;
using static VpSharp.Internal.NativeAttributes.DataAttribute;
using static VpSharp.Internal.NativeAttributes.FloatAttribute;
using static VpSharp.Internal.NativeAttributes.IntegerAttribute;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp.Entities;

/// <summary>
///     Represents the base class for object builders.
/// </summary>
public abstract class ObjectBuilder
{
    private protected ObjectBuilder(
        VirtualParadiseClient client,
        VirtualParadiseObject targetObject,
        ObjectBuilderMode mode
    )
    {
        Client = client;
        TargetObject = targetObject;
        Mode = mode;
    }

    /// <summary>
    ///     Gets or sets the date and time at which this object was last modified.
    /// </summary>
    /// <value>
    ///     The date and time at which this object was last modified, or <see langword="default" /> to leave unchanged.
    /// </value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public Option<DateTimeOffset> ModificationTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the owner of this object.
    /// </summary>
    /// <value>The owner of this object, or <see langword="default" /> to leave unchanged.</value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public Option<User> Owner { get; set; }

    /// <summary>
    ///     Gets or sets the position of the object.
    /// </summary>
    /// <value>The position of the object, or <see langword="default" /> to leave unchanged.</value>
    public Option<Vector3d> Position { get; set; }

    /// <summary>
    ///     Gets or sets the rotation of the object.
    /// </summary>
    /// <value>The rotation of the object, or <see langword="default" /> to leave unchanged.</value>
    public Option<Rotation> Rotation { get; set; }

    internal Option<IReadOnlyList<byte>> Data { get; set; }

    private protected VirtualParadiseClient Client { get; }

    private protected ObjectBuilderMode Mode { get; }

    private protected VirtualParadiseObject TargetObject { get; }

    internal virtual void ApplyChanges()
    {
        ApplyPosition();
        ApplyRotation();
        ApplyModificationTimestamp();
        ApplyOwner();
        ApplyData();
    }

    private void ApplyData()
    {
        nint handle = Client.NativeInstanceHandle;
        byte[] data = Data.ValueOr(ArraySegment<byte>.Empty).ToArray();
        _ = vp_data_set(handle, ObjectData, data.Length, data);
    }

    private void ApplyOwner()
    {
        nint handle = Client.NativeInstanceHandle;

        if (Owner.HasValue && Mode != ObjectBuilderMode.Load)
        {
            throw new InvalidOperationException("Owner can only be assigned during an object load.");
        }

        User oldOwner = TargetObject.Owner;
        _ = vp_int_set(handle, ObjectUserId, Owner.ValueOr(oldOwner).Id);
    }

    private void ApplyModificationTimestamp()
    {
        nint handle = Client.NativeInstanceHandle;

        if (ModificationTimestamp.HasValue && Mode != ObjectBuilderMode.Load)
        {
            throw new InvalidOperationException("Modification timestamp can only be assigned during an object load.");
        }

        DateTimeOffset oldTimestamp = TargetObject.ModificationTimestamp;
        _ = vp_int_set(handle, ObjectTime, (int)ModificationTimestamp.ValueOr(oldTimestamp).ToUnixTimeSeconds());
    }

    private void ApplyPosition()
    {
        nint handle = Client.NativeInstanceHandle;

        if (!Position.HasValue && Mode == ObjectBuilderMode.Create)
        {
            throw new ArgumentException("Position must be assigned when creating a new object.");
        }

        (double x, double y, double z) = Position.ValueOr(TargetObject.Location.Position);
        _ = vp_double_set(handle, ObjectX, x);
        _ = vp_double_set(handle, ObjectY, y);
        _ = vp_double_set(handle, ObjectZ, z);
    }

    private void ApplyRotation()
    {
        nint handle = Client.NativeInstanceHandle;

        if (!Rotation.HasValue && Mode == ObjectBuilderMode.Create)
        {
            Rotation = Option.Some(VpSharp.Rotation.None);
        }

        (double x, double y, double z, double angle) = Rotation.ValueOr(TargetObject.Location.Rotation);
        _ = vp_double_set(handle, ObjectRotationX, x);
        _ = vp_double_set(handle, ObjectRotationY, y);
        _ = vp_double_set(handle, ObjectRotationZ, z);
        _ = vp_double_set(handle, ObjectRotationAngle, angle);
    }
}

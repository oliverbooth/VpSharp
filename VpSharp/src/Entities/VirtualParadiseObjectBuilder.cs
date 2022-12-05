using System.Numerics;
using VpSharp.Extensions;
using VpSharp.Internal;
using X10D.Math;
using static VpSharp.Internal.NativeAttributes.DataAttribute;
using static VpSharp.Internal.NativeAttributes.FloatAttribute;
using static VpSharp.Internal.NativeAttributes.IntegerAttribute;
using static VpSharp.Internal.NativeMethods;

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

    /// <summary>
    ///     Gets or sets the date and time at which this object was last modified.
    /// </summary>
    /// <value>
    ///     The date and time at which this object was last modified, or <see langword="null" /> to leave unchanged.
    /// </value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public Optional<DateTimeOffset> ModificationTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the owner of this object.
    /// </summary>
    /// <value>The owner of this object, or <see langword="null" /> to leave unchanged.</value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public Optional<VirtualParadiseUser> Owner { get; set; }

    /// <summary>
    ///     Gets or sets the position of the object.
    /// </summary>
    /// <value>The position of the object, or <see langword="null" /> to leave unchanged.</value>
    public Optional<Vector3d> Position { get; set; }

    /// <summary>
    ///     Gets or sets the rotation of the object.
    /// </summary>
    /// <value>The rotation of the object, or <see langword="null" /> to leave unchanged.</value>
    public Optional<Quaternion> Rotation { get; set; }

    internal Optional<IReadOnlyList<byte>> Data { get; set; }

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
        if (Data.HasValue)
        {
            IReadOnlyList<byte> data = Data.Value!;
            _ = vp_data_set(handle, ObjectData, data.Count, data.ToArray());
        }
        else
        {
            _ = vp_data_set(handle, ObjectData, 0, Array.Empty<byte>());
        }
    }

    private void ApplyOwner()
    {
        nint handle = Client.NativeInstanceHandle;
        if (Owner.HasValue)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Owner can only be assigned during an object load.");
            }

            _ = vp_int_set(handle, ObjectUserId, Owner.Value!.Id);
        }
        else
        {
            _ = vp_int_set(handle, ObjectUserId, TargetObject.Owner.Id);
        }
    }

    private void ApplyModificationTimestamp()
    {
        nint handle = Client.NativeInstanceHandle;

        if (ModificationTimestamp.HasValue)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Modification timestamp can only be assigned during an object load.");
            }

            _ = vp_int_set(handle, ObjectTime, (int)ModificationTimestamp.Value.ToUnixTimeSeconds());
        }
        else
        {
            _ = vp_int_set(handle, ObjectTime, (int)TargetObject.ModificationTimestamp.ToUnixTimeSeconds());
        }
    }

    private void ApplyPosition()
    {
        nint handle = Client.NativeInstanceHandle;

        if (Position.HasValue)
        {
            (double x, double y, double z) = Position.Value;
            _ = vp_double_set(handle, ObjectX, x);
            _ = vp_double_set(handle, ObjectY, y);
            _ = vp_double_set(handle, ObjectZ, z);
        }
        else if (Mode == ObjectBuilderMode.Create)
        {
            throw new ArgumentException("Position must be assigned when creating a new object.");
        }
        else
        {
            (double x, double y, double z) = TargetObject.Location.Position;
            _ = vp_double_set(handle, ObjectX, x);
            _ = vp_double_set(handle, ObjectY, y);
            _ = vp_double_set(handle, ObjectZ, z);
        }
    }

    private void ApplyRotation()
    {
        nint handle = Client.NativeInstanceHandle;

        if (!Rotation.HasValue && Mode == ObjectBuilderMode.Create)
        {
            Rotation = Quaternion.Identity;
        }

        if (Rotation.HasValue)
        {
            (double x, double y, double z) = Vector3d.Zero;
            double angle = double.PositiveInfinity;
            if (Rotation.Value != Quaternion.Identity)
            {
                Rotation.Value.ToAxisAngle(out Vector3d axis, out angle);
                (x, y, z) = axis;
                angle = angle.RadiansToDegrees();
            }

            if (double.IsPositiveInfinity(angle))
            {
                x = x.RadiansToDegrees();
                y = y.RadiansToDegrees();
                z = z.RadiansToDegrees();
            }

            _ = vp_double_set(handle, ObjectRotationX, x);
            _ = vp_double_set(handle, ObjectRotationY, y);
            _ = vp_double_set(handle, ObjectRotationZ, z);
            _ = vp_double_set(handle, ObjectRotationAngle, angle);
        }
        else
        {
            TargetObject.Location.Rotation.ToAxisAngle(out Vector3d axis, out double angle);
            _ = vp_double_set(handle, ObjectRotationX, axis.X);
            _ = vp_double_set(handle, ObjectRotationY, axis.Y);
            _ = vp_double_set(handle, ObjectRotationZ, axis.Z);
            _ = vp_double_set(handle, ObjectRotationAngle, angle);
        }
    }
}

using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Channels;
using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<int, TaskCompletionSource<(ReasonCode, VirtualParadiseObject?)>>
        _objectCompletionSources = new();
    private readonly ConcurrentDictionary<int, VirtualParadiseObject> _objects = new();

    /// <summary>
    ///     Enumerates all objects within a specified cell.
    /// </summary>
    /// <param name="cell">The cell whose objects to enumerate.</param>
    /// <param name="revision">The cell revision.</param>
    /// <returns>An enumerable of <see cref="VirtualParadiseObject" />.</returns>
    public IAsyncEnumerable<VirtualParadiseObject> EnumerateObjectsAsync(Cell cell, int? revision = null)
    {
        if (_cellChannels.TryGetValue(cell, out Channel<VirtualParadiseObject>? channel))
        {
            return channel.Reader.ReadAllAsync();
        }

        channel = Channel.CreateUnbounded<VirtualParadiseObject>();
        _cellChannels.TryAdd(cell, channel);

        lock (Lock)
        {
            _ = revision is not null
                ? vp_query_cell_revision(NativeInstanceHandle, cell.X, cell.Z, revision.Value)
                : vp_query_cell(NativeInstanceHandle, cell.X, cell.Z);
        }

        return channel.Reader.ReadAllAsync();
    }

    /// <summary>
    ///     Enumerates all objects within a specified range of cell.
    /// </summary>
    /// <param name="center">The cell whose objects to enumerate.</param>
    /// <param name="radius">The range of cells to query.</param>
    /// <param name="revision">The cell revision.</param>
    /// <returns>An enumerable of <see cref="VirtualParadiseObject" />.</returns>
    public async IAsyncEnumerable<VirtualParadiseObject> EnumerateObjectsAsync(Cell center, int radius, int? revision = null)
    {
        if (radius < 0)
        {
            throw new ArgumentException("Range must be greater than or equal to 1.");
        }

        var cells = new HashSet<Cell>();

        for (int x = center.X - radius; x < center.X + radius; x++)
        for (int z = center.Z - radius; z < center.Z + radius; z++)
        {
            cells.Add(new Cell(x, z));
        }

        foreach (Cell cell in cells.OrderBy(c => Vector2.Distance(c, center)))
        {
            await foreach (VirtualParadiseObject vpObject in EnumerateObjectsAsync(cell))
            {
                yield return vpObject;
            }
        }
    }

    /// <summary>
    ///     Gets an object by its ID.
    /// </summary>
    /// <param name="id">The ID of the object to retrieve.</param>
    /// <returns>The retrieved object.</returns>
    /// <exception cref="Exception">
    ///     <para>An error occurred communicating with the database.</para>
    ///     -or-
    ///     <para>An unknown error occurred retrieving the object.</para>
    /// </exception>
    /// <exception cref="ObjectNotFoundException">No object with the ID <paramref name="id" /> was found.</exception>
    public async Task<VirtualParadiseObject> GetObjectAsync(int id)
    {
        if (_objects.TryGetValue(id, out VirtualParadiseObject? virtualParadiseObject))
        {
            return virtualParadiseObject;
        }

        ReasonCode reason;

        if (!_objectCompletionSources.TryGetValue(id,
                out TaskCompletionSource<(ReasonCode, VirtualParadiseObject)>? taskCompletionSource))
        {
            taskCompletionSource = new TaskCompletionSource<(ReasonCode, VirtualParadiseObject?)>();
            _objectCompletionSources.TryAdd(id, taskCompletionSource);

            lock (Lock)
            {
                _ = vp_int_set(NativeInstanceHandle, IntegerAttribute.ReferenceNumber, id);
                reason = (ReasonCode)vp_object_get(NativeInstanceHandle, id);
                if (reason != ReasonCode.Success)
                {
                    goto PreReturn;
                }
            }
        }

        (reason, virtualParadiseObject) = await taskCompletionSource.Task;
        _objectCompletionSources.TryRemove(id, out _);

        if (virtualParadiseObject is not null)
        {
            _objects.TryAdd(id, virtualParadiseObject);
        }

        PreReturn:
        return reason switch
        {
            ReasonCode.DatabaseError =>
                throw new VirtualParadiseException(ReasonCode.DatabaseError, "Error communicating with the database."),
            ReasonCode.ObjectNotFound => throw new ObjectNotFoundException(),
            ReasonCode.UnknownError =>
                throw new VirtualParadiseException(ReasonCode.UnknownError, "An unknown error occurred retrieving the object."),
            _ when reason != ReasonCode.Success => throw new VirtualParadiseException(reason, $"{reason:D} ({reason:G})"),
            _ => virtualParadiseObject!
        };
    }

    private VirtualParadiseObject AddOrUpdateObject(VirtualParadiseObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return _objects.AddOrUpdate(obj.Id, obj, (_, existing) =>
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            existing ??= obj;
            existing.ExtractFromOther(obj);
            return existing;
        });
    }

    private async Task<VirtualParadiseObject> ExtractObjectAsync(nint sender)
    {
        var type = (ObjectType)vp_int(sender, IntegerAttribute.ObjectType);
        int id = vp_int(sender, IntegerAttribute.ObjectId);
        int owner = vp_int(sender, IntegerAttribute.ObjectUserId);

        double x = vp_double(sender, FloatAttribute.ObjectX);
        double y = vp_double(sender, FloatAttribute.ObjectY);
        double z = vp_double(sender, FloatAttribute.ObjectZ);
        var position = new Vector3d(x, y, z);

        float rotX = vp_float(sender, FloatAttribute.ObjectRotationX);
        float rotY = vp_float(sender, FloatAttribute.ObjectRotationY);
        float rotZ = vp_float(sender, FloatAttribute.ObjectRotationZ);
        float angle = vp_float(sender, FloatAttribute.ObjectRotationAngle);
        Quaternion rotation;

        if (double.IsPositiveInfinity(angle))
        {
            rotation = Quaternion.CreateFromYawPitchRoll(rotY, rotX, rotZ);
        }
        else
        {
            var axis = new Vector3(rotX, rotY, rotZ);
            rotation = Quaternion.CreateFromAxisAngle(axis, angle);
        }

        VirtualParadiseObject virtualParadiseObject = type switch
        {
            ObjectType.Model => new VirtualParadiseModelObject(this, id),
            ObjectType.ParticleEmitter => new VirtualParadiseParticleEmitterObject(this, id),
            ObjectType.Path => new VirtualParadisePathObject(this, id),
            var _ => throw new NotSupportedException("Unsupported object type.")
        };

        virtualParadiseObject.ExtractFromInstance(sender);

        var location = new Location(CurrentWorld!, position, rotation);
        virtualParadiseObject.Location = location;
        virtualParadiseObject.Owner = await GetUserAsync(owner);
        return virtualParadiseObject;
    }
}

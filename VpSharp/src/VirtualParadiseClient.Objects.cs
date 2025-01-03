using System.Collections.Concurrent;
using System.Threading.Channels;
using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<int, TaskCompletionSource<(ReasonCode, VirtualParadiseObject?)>>
        _objectCompletionSources = new();
    private readonly ConcurrentDictionary<int, VirtualParadiseObject> _objects = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<ReasonCode>> _objectUpdates = new();


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
    /// <returns>An enumerable of <see cref="VirtualParadiseObject" />.</returns>
    public async IAsyncEnumerable<VirtualParadiseObject> EnumerateObjectsAsync(Cell center, int radius)
    {
        if (radius < 0)
        {
            throw new ArgumentException(ExceptionMessages.RadiusMustBeGreaterThan1, nameof(radius));
        }

        var hashSet = new HashSet<Cell>();

        for (int x = center.X - radius; x < center.X + radius; x++)
        for (int z = center.Z - radius; z < center.Z + radius; z++)
        {
            hashSet.Add(new Cell(x, z));
        }

        var centerPosition = center.ToVector3d();
        var cells = new List<Cell>(hashSet);
        cells.Sort((a, b) => Vector3d.DistanceSquared(a.ToVector3d(), centerPosition).CompareTo(Vector3d.DistanceSquared(b.ToVector3d(), centerPosition)));

        var objects = new List<VirtualParadiseObject>();
        var tasks = new List<Task>();

        foreach (Cell[] chunk in cells.Chunk(64))
        {
            Task task = Parallel.ForEachAsync(chunk, async (cell, token) =>
            {
                await foreach (VirtualParadiseObject current in EnumerateObjectsAsync(cell).WithCancellation(token))
                {
                    objects.Add(current);
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        foreach (VirtualParadiseObject current in objects)
        {
            yield return current;
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

        if (!_objectCompletionSources.TryGetValue(id, out var taskCompletionSource))
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
            ReasonCode.DatabaseError => throw new VirtualParadiseException(ReasonCode.DatabaseError),
            ReasonCode.ObjectNotFound => throw new ObjectNotFoundException(),
            ReasonCode.UnknownError => throw new VirtualParadiseException(ReasonCode.UnknownError),
            _ when reason != ReasonCode.Success => throw new VirtualParadiseException(reason),
            _ => virtualParadiseObject!
        };
    }


    internal void AddObjectUpdateCompletionSource(int id, TaskCompletionSource<ReasonCode> taskCompletionSource)
    {
        _objectUpdates.AddOrUpdate(id, _ => taskCompletionSource, (_, _) => taskCompletionSource);
    }

    internal void RemoveObjectUpdateCompletionSource(int id)
    {
        _objectUpdates.TryRemove(id, out _);
    }

    private VirtualParadiseObject AddOrUpdateObject(VirtualParadiseObject obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

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
        ObjectType type;
        int id;
        int time;
        int owner;
        Rotation rotation;
        Vector3d position;

        lock (Lock)
        {
            type = (ObjectType)vp_int(sender, IntegerAttribute.ObjectType);
            id = vp_int(sender, IntegerAttribute.ObjectId);
            owner = vp_int(sender, IntegerAttribute.ObjectUserId);

            double x = vp_double(sender, FloatAttribute.ObjectX);
            double y = vp_double(sender, FloatAttribute.ObjectY);
            double z = vp_double(sender, FloatAttribute.ObjectZ);
            position = new Vector3d(x, y, z);

            float rotX = vp_float(sender, FloatAttribute.ObjectRotationX);
            float rotY = vp_float(sender, FloatAttribute.ObjectRotationY);
            float rotZ = vp_float(sender, FloatAttribute.ObjectRotationZ);
            float angle = vp_float(sender, FloatAttribute.ObjectRotationAngle);

            if (double.IsPositiveInfinity(angle))
            {
                rotation = Rotation.CreateFromTiltYawRoll(rotX, rotY, rotZ);
            }
            else
            {
                var axis = new Vector3d(rotX, rotY, rotZ);
                rotation = Rotation.CreateFromAxisAngle(axis, angle);
            }

            time = vp_int(sender, IntegerAttribute.ObjectTime);
        }

        VirtualParadiseObject virtualParadiseObject = type switch
        {
            ObjectType.Model => new VirtualParadiseModelObject(this, id),
            ObjectType.ParticleEmitter => new VirtualParadiseParticleEmitterObject(this, id),
            ObjectType.Path => new VirtualParadisePathObject(this, id),
            _ => throw new NotSupportedException(ExceptionMessages.UnsupportedObjectType)
        };

        virtualParadiseObject.ExtractFromInstance(sender);

        var location = new Location(CurrentWorld!, position, rotation);
        virtualParadiseObject.Location = location;
        virtualParadiseObject.ModificationTimestamp = DateTimeOffset.FromUnixTimeSeconds(time);

        var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        virtualParadiseObject.Owner = (await GetUserAsync(owner, ctx.Token).ConfigureAwait(false))!;

        return virtualParadiseObject;
    }
}

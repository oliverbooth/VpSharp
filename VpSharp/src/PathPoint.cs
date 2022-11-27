using System.Numerics;
using VpSharp.Entities;

namespace VpSharp;

/// <summary>
///     Represents a point along a <see cref="VirtualParadisePath" />.
/// </summary>
public readonly struct PathPoint
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PathPoint" /> struct.
    /// </summary>
    /// <param name="offset">The time offset of the point.</param>
    /// <param name="position">The position of the point.</param>
    /// <param name="rotation">The rotation of the point.</param>
    public PathPoint(TimeSpan offset, Vector3d position, Quaternion rotation)
    {
        Offset = offset;
        Position = position;
        Rotation = rotation;
    }

    /// <summary>
    ///     Gets the time offset of the point.
    /// </summary>
    /// <value>The time offset of the point.</value>
    public TimeSpan Offset { get; }

    /// <summary>
    ///     Gets the position of the point.
    /// </summary>
    /// <value>The position of the point.</value>
    public Vector3d Position { get; }

    /// <summary>
    ///     Gets the rotation of the point.
    /// </summary>
    /// <value>The rotation of the point.</value>
    public Quaternion Rotation { get; }
}

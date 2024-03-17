namespace VpSharp.Entities;

/// <summary>
///     Represents a path contained by a <see cref="PathObject" />.
/// </summary>
public sealed class Path : ICloneable
{
    internal Path(PathEasing easing, string name, IEnumerable<PathPoint> points, bool isClosed)
    {
        Easing = easing;
        Name = name;
        IsClosed = isClosed;
        Points = points.ToArray();
    }

    /// <summary>
    ///     Gets the path type.
    /// </summary>
    /// <value>The path type.</value>
    public PathEasing Easing { get; }

    /// <summary>
    ///     Gets a value indicating whether this path is closed.
    /// </summary>
    /// <value><see langword="true" /> if this path is closed; otherwise, <see langword="false" />.</value>
    public bool IsClosed { get; }

    /// <summary>
    ///     Gets the name of this path.
    /// </summary>
    /// <value>The name of this path.</value>
    public string Name { get; }

    /// <summary>
    ///     Gets the points along this path.
    /// </summary>
    /// <value>The points along this path.</value>
    public IReadOnlyList<PathPoint> Points { get; }

    /// <inheritdoc />
    public object Clone()
    {
        return new Path(Easing, Name, Points, IsClosed);
    }
}

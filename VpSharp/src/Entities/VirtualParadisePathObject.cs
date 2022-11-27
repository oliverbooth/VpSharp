using System.Numerics;
using System.Text;
using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Entities;

/// <summary>
///     Represents a path object.
/// </summary>
public sealed class VirtualParadisePathObject : VirtualParadiseObject
{
    /// <inheritdoc />
    internal VirtualParadisePathObject(VirtualParadiseClient client, int id)
        : base(client, id)
    {
    }

    /// <summary>
    ///     Gets the path in this object.
    /// </summary>
    /// <value>The path in this object.</value>
    public VirtualParadisePath Path { get; set; }

    /// <inheritdoc />
    protected internal override void ExtractFromOther(VirtualParadiseObject virtualParadiseObject)
    {
        if (virtualParadiseObject is not VirtualParadisePathObject path)
            return;

        Path = (VirtualParadisePath)path.Path.Clone();
    }

    protected override void ExtractFromData(ReadOnlySpan<byte> data)
    {
        Span<char> chars = stackalloc char[data.Length];
        Encoding.UTF8.GetChars(data, chars);

        using var buffer = new Utf8ValueStringBuilder(false);
        int index;

        // version
        var version = 0;
        for (index = 0; index < chars.Length; index++)
        {
            if (chars[index] == '\n')
            {
                version = buffer.AsSpan().ToInt32();
                break;
            }

            buffer.Append(chars[index]);
        }

        buffer.Clear();

        if (version != 1)
            throw new NotSupportedException($"Unsupported path version {version}");

        // path name
        var name = string.Empty;
        for (index += 1; index < chars.Length; index++)
        {
            if (chars[index] == '\n')
            {
                name = buffer.ToString();
                break;
            }

            buffer.Append(chars[index]);
        }

        buffer.Clear();

        // type
        ++index;
        int spaceIndex = chars[index..].IndexOf(' ');
        int newLineIndex = chars[index..].IndexOf('\n');

        int pathType = int.Parse(chars[index..(index + spaceIndex)]);
        int closed = int.Parse(chars[(index + spaceIndex + 1)..(index + newLineIndex)]);
        index += newLineIndex;

        // points from here onwards
        var points = new List<PathPoint>();
        TimeSpan? offset = null;
        double? x = null, y = null, z = null;
        float? rx = null, ry = null, rz = null, ra = null;

        for (index += 1; index < chars.Length; index++)
        {
            if (char.IsWhiteSpace(chars[index]))
            {
                if (offset is null)
                {
                    offset = TimeSpan.FromSeconds(buffer.AsSpan().ToDouble());
                    buffer.Clear();
                }
                else if (x is null)
                {
                    x = buffer.AsSpan().ToDouble();
                    buffer.Clear();
                }
                else if (y is null)
                {
                    y = buffer.AsSpan().ToDouble();
                    buffer.Clear();
                }
                else if (z is null)
                {
                    z = buffer.AsSpan().ToDouble();
                    buffer.Clear();
                }
                else if (rx is null)
                {
                    rx = buffer.AsSpan().ToSingle();
                    buffer.Clear();
                }
                else if (ry is null)
                {
                    ry = buffer.AsSpan().ToSingle();
                    buffer.Clear();
                }
                else if (rz is null)
                {
                    rz = buffer.AsSpan().ToSingle();
                    buffer.Clear();
                }
                else if (ra is null)
                {
                    ra = buffer.AsSpan().ToSingle();
                    buffer.Clear();
                }
            }

            if (chars[index] == '\n' || index == chars.Length - 1)
            {
                var position = new Vector3d(x ?? 0, y ?? 0, z ?? 0);
                var axis = new Vector3(rx ?? 0, ry ?? 0, rz ?? 0);

                Quaternion rotation = double.IsPositiveInfinity(ra ?? 0)
                    ? Quaternion.CreateFromYawPitchRoll(axis.Y, axis.X, axis.Z)
                    : Quaternion.CreateFromAxisAngle(axis, ra ?? 0);

                var point = new PathPoint(offset ?? TimeSpan.Zero, position, rotation);
                points.Add(point);

                x = y = z = rx = ry = rz = ra = null;
                offset = null;
            }

            buffer.Append(chars[index]);
        }

        Path = new VirtualParadisePath((PathEasing)pathType, name, points, closed == 1);
    }
}
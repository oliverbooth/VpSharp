using System.Numerics;

namespace VpSharp.Extensions;

/// <summary>
///     Extension methods for <see cref="Quaternion" />.
/// </summary>
public static class QuaternionExtensions
{
    /// <summary>
    ///     Converts this quaternion to a <see cref="Vector3d" /> containing an Euler representation of the rotation. 
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <returns>The Euler representation of <paramref name="value" />.</returns>
    /// <see href="https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/" />
    public static Vector3d ToEulerAngles(this Quaternion value)
    {
        value = Quaternion.Normalize(value);
        double x = Math.Atan2(2 * (value.X * value.W - value.Y * value.Z), 1 - 2 * (value.X * value.X + value.Z * value.Z));
        double y = Math.Asin(2 * (value.X * value.Z + value.Y * value.W));
        double z = Math.Atan2(2 * (value.Z * value.W - value.X * value.Y), 1 - 2 * (value.Y * value.Y + value.Z * value.Z));
        return new Vector3d(x, y, z) * (180 / Math.PI);
    }

    /// <summary>
    ///     Converts this quaternion to an axis/angle pair.
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
#pragma warning disable CA1021
    public static void ToAxisAngle(this Quaternion value, out Vector3d axis, out double angle)
#pragma warning restore CA1021
    {
        angle = 2 * Math.Acos(value.W);
        axis = Vector3d.Normalize(new Vector3d(value.X, value.Y, value.Z));
    }
}

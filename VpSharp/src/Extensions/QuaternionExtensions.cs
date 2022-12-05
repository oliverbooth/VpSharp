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
    ///     Converts this quaternion to a <see cref="Vector3d" /> containing an Euler representation of the rotation. 
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <returns>The Euler representation of <paramref name="value" />.</returns>
    public static Vector3 ToEulerAnglesF(this Quaternion value)
    {
        value = Quaternion.Normalize(value);
        float x = MathF.Atan2(2 * (value.X * value.W - value.Y * value.Z), 1 - 2 * (value.X * value.X + value.Z * value.Z));
        float y = MathF.Asin(2 * (value.X * value.Z + value.Y * value.W));
        float z = MathF.Atan2(2 * (value.Z * value.W - value.X * value.Y), 1 - 2 * (value.Y * value.Y + value.Z * value.Z));
        return new Vector3(x, y, z) * (180 / MathF.PI);
    }

#pragma warning disable CA1021
    /// <summary>
    ///     Converts this quaternion to an axis/angle pair.
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
    /// <see href="https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm"/>
    public static void ToAxisAngle(this Quaternion value, out Vector3 axis, out float angle)
    {
        angle = 2 * MathF.Acos(value.W);
        axis = Vector3.Normalize(new Vector3(value.X, value.Y, value.Z));
    }

    /// <summary>
    ///     Converts this quaternion to an axis/angle pair.
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
    public static void ToAxisAngle(this Quaternion value, out Vector3d axis, out double angle)
    {
        angle = 2 * Math.Acos(value.W);
        axis = Vector3d.Normalize(new Vector3d(value.X, value.Y, value.Z));
    }
#pragma warning restore CA1021
}

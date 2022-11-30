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
    /// <param name="radians">
    ///     <see langword="true" /> if the resulting vector should be in radians; or <see langword="false" /> if the resulting
    ///     vector should be in degrees.
    /// </param>
    /// <returns>The Euler representation of <paramref name="value" />.</returns>
    /// <see href="https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/" />
    public static Vector3d ToEulerAngles(this Quaternion value, bool radians = true)
    {
        double a = (2.0 * value.Y * value.W) - (2.0 * value.X * value.Z);
        double b = 1.0 - (2.0 * value.Y * value.Y) - (2.0 * value.Z * value.Z);
        double y = -Math.Atan2(a, b);

        a = 2.0 * value.X * value.Y;
        b = 2.0 * value.Z * value.W;
        double z = Math.Asin(a + b);

        a = (2.0 * value.X * value.W) - (2.0 * value.Y * value.Z);
        b = 1.0 - (2.0 * value.X * value.X) - (2.0 * value.Z * value.Z);
        double x = Math.Atan2(a, b);

        if (!radians)
        {
            x = 180.0 / Math.PI * x;
            y = 180.0 / Math.PI * y;
            z = 180.0 / Math.PI * z;
        }

        return new Vector3d(x, y, z);
    }

    /// <summary>
    ///     Converts this quaternion to a <see cref="Vector3d" /> containing an Euler representation of the rotation. 
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="radians">
    ///     <see langword="true" /> if the resulting vector should be in radians; or <see langword="false" /> if the resulting
    ///     vector should be in degrees.
    /// </param>
    /// <returns>The Euler representation of <paramref name="value" />.</returns>
    public static Vector3d ToEulerAnglesF(this Quaternion value, bool radians = true)
    {
        float a = (2.0f * value.Y * value.W) - (2.0f * value.X * value.Z);
        float b = 1.0f - (2.0f * value.Y * value.Y) - (2.0f * value.Z * value.Z);
        float y = -MathF.Atan2(a, b);

        a = 2.0f * value.X * value.Y;
        b = 2.0f * value.Z * value.W;
        float z = MathF.Asin(a + b);

        a = (2.0f * value.X * value.W) - (2.0f * value.Y * value.Z);
        b = 1.0f - (2.0f * value.X * value.X) - (2.0f * value.Z * value.Z);
        float x = MathF.Atan2(a, b);

        if (!radians)
        {
            x = 180.0f / MathF.PI * x;
            y = 180.0f / MathF.PI * y;
            z = 180.0f / MathF.PI * z;
        }

        return new Vector3d(x, y, z);
    }

    // 
    /// <summary>
    ///     Converts this quaternion to an axis/angle pair.
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
    /// <see href="https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm"/>
    public static void ToAxisAngle(this Quaternion value, out Vector3 axis, out float angle)
    {
        angle = 2.0f * MathF.Acos(value.W);

        float x = value.X / MathF.Sqrt(1.0f - (value.W * value.W));
        float y = value.Y / MathF.Sqrt(1.0f - (value.W * value.W));
        float z = value.Z / MathF.Sqrt(1.0f - (value.W * value.W));

        axis = new Vector3(x, y, z);
    }

    /// <summary>
    ///     Converts this quaternion to an axis/angle pair.
    /// </summary>
    /// <param name="value">The quaternion to convert.</param>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
    public static void ToAxisAngle(this Quaternion value, out Vector3d axis, out double angle)
    {
        angle = 2.0 * Math.Acos(value.W);

        double x = value.X / Math.Sqrt(1.0 - (value.W * value.W));
        double y = value.Y / Math.Sqrt(1.0 - (value.W * value.W));
        double z = value.Z / Math.Sqrt(1.0 - (value.W * value.W));

        axis = new Vector3d(x, y, z);
    }
}

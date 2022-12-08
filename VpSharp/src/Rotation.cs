using System.Globalization;
using System.Numerics;
using Cysharp.Text;
using X10D.Math;
using X10D.Numerics;

namespace VpSharp;

/// <summary>
///     Represents a rotation.
/// </summary>
public readonly struct Rotation : IEquatable<Rotation>, IFormattable
{
    /// <summary>
    ///     Represents no rotation.
    /// </summary>
    public static readonly Rotation None = new(0, 0, 0, double.PositiveInfinity);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rotation" /> struct.
    /// </summary>
    /// <param name="tilt">The tilt.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="roll">The roll.</param>
    /// <param name="angle">The angle.</param>
    public Rotation(double tilt, double yaw, double roll, double angle)
    {
        Angle = angle;
        Roll = roll;
        Tilt = tilt;
        Yaw = yaw;
    }

    /// <summary>
    ///     Gets or initializes the angle component of this rotation.
    /// </summary>
    /// <value>The angle component.</value>
    public double Angle { get; init; }

    /// <summary>
    ///     Gets or initializes the roll component of this rotation.
    /// </summary>
    /// <value>The roll component.</value>
    /// <remarks>This value is the rotation on the Z axis.</remarks>
    public double Roll { get; init; }

    /// <summary>
    ///     Gets or initializes the tilt component of this rotation.
    /// </summary>
    /// <value>The tilt component.</value>
    /// <remarks>This value is the rotation on the X axis.</remarks>
    public double Tilt { get; init; }

    /// <summary>
    ///     Gets or initializes the yaw component of this rotation.
    /// </summary>
    /// <value>The yaw component.</value>
    /// <remarks>This value is the rotation on the Y axis.</remarks>
    public double Yaw { get; init; }

    /// <summary>
    ///     Returns a value indicating whether the two given rotations are equal.
    /// </summary>
    /// <param name="left">The first rotation to compare.</param>
    /// <param name="right">The second rotation to compare.</param>
    /// <returns><see langword="true" /> if the two rotations are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Rotation left, Rotation right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns a value indicating whether the two given rotations are not equal.
    /// </summary>
    /// <param name="left">The first rotation to compare.</param>
    /// <param name="right">The second rotation to compare.</param>
    /// <returns><see langword="true" /> if the two rotations are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Rotation left, Rotation right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Creates a <see cref="Rotation" /> from the specified axis and angle, as represented in degrees.
    /// </summary>
    /// <param name="axis">The axis value.</param>
    /// <param name="angle">The angle value.</param>
    /// <returns>A new instance of <see cref="Rotation" />.</returns>
    public static Rotation CreateFromAxisAngle(Vector3d axis, double angle)
    {
        return new Rotation(axis.X, axis.Y, axis.Z, angle.DegreesToRadians());
    }

    /// <summary>
    ///     Creates a <see cref="Rotation" /> from the specified quaternion.
    /// </summary>
    /// <param name="quaternion">The quaternion.</param>
    /// <returns>A new instance of <see cref="Rotation" />.</returns>
    public static Rotation CreateFromQuaternion(Quaternion quaternion)
    {
        (Vector3d axis, double angle) = quaternion.ToAxisAngle();
        return new Rotation(axis.X, axis.Y, axis.Z, angle);
    }

    /// <summary>
    ///     Creates a <see cref="Rotation" /> from the specified Euler rotation vector, as represented in degrees.
    /// </summary>
    /// <param name="vector">The Euler rotation vector.</param>
    /// <returns>A new instance of <see cref="Rotation" />.</returns>
    public static Rotation CreateFromTiltYawRoll(Vector3d vector)
    {
        return new Rotation(vector.X, vector.Y, vector.Z, double.PositiveInfinity);
    }

    /// <summary>
    ///     Creates a <see cref="Rotation" /> from the specified tilt, yaw, and roll, as represented in degrees.
    /// </summary>
    /// <param name="tilt">The tilt.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="roll">The roll.</param>
    /// <returns>A new instance of <see cref="Rotation" />.</returns>
    public static Rotation CreateFromTiltYawRoll(double tilt, double yaw, double roll)
    {
        return new Rotation(tilt, yaw, roll, double.PositiveInfinity);
    }

    /// <summary>
    ///     Deconstructs this rotation.
    /// </summary>
    /// <param name="tilt">When this method returns, contains the <see cref="Tilt" /> component value.</param>
    /// <param name="yaw">When this method returns, contains the <see cref="Yaw" /> component value.</param>
    /// <param name="roll">When this method returns, contains the <see cref="Roll" /> component value.</param>
    public void Deconstruct(out double tilt, out double yaw, out double roll)
    {
        Deconstruct(out tilt, out yaw, out roll, out _);
    }

    /// <summary>
    ///     Deconstructs this rotation.
    /// </summary>
    /// <param name="tilt">When this method returns, contains the <see cref="Tilt" /> component value.</param>
    /// <param name="yaw">When this method returns, contains the <see cref="Yaw" /> component value.</param>
    /// <param name="roll">When this method returns, contains the <see cref="Roll" /> component value.</param>
    /// <param name="angle">When this method returns, contains the <see cref="Angle" /> component value.</param>
    public void Deconstruct(out double tilt, out double yaw, out double roll, out double angle)
    {
        tilt = Tilt;
        yaw = Yaw;
        roll = Roll;
        angle = Angle;
    }

    /// <summary>
    ///     Returns a value indicating whether this rotation and another rotation are equal.
    /// </summary>
    /// <param name="other">The rotation to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two rotations are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Rotation other)
    {
        return Angle.Equals(other.Angle) && Roll.Equals(other.Roll) && Tilt.Equals(other.Tilt) && Yaw.Equals(other.Yaw);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Rotation other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Angle, Roll, Tilt, Yaw);
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public override string ToString()
    {
        return ToString("{0}");
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <param name="format">The format to apply to each component.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        format ??= "{0}";
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        using var builder = ZString.CreateUtf8StringBuilder();
        builder.Append('<');
        builder.Append(string.Format(formatProvider, format, Tilt));
        builder.Append(separator);
        builder.Append(' ');
        builder.Append(string.Format(formatProvider, format, Yaw));
        builder.Append(separator);
        builder.Append(' ');
        builder.Append(string.Format(formatProvider, format, Roll));
        builder.Append('>');

        return builder.ToString();
    }
}

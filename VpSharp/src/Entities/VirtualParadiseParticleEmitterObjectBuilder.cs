// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;
using Optional;
using VpSharp.Internal;
using VpSharp.Internal.Attributes;
using VpSharp.Internal.ValueConverters;

namespace VpSharp.Entities;

/// <summary>
///     Provides mutability for a <see cref="VirtualParadiseParticleEmitterObject" />.
/// </summary>
public sealed class VirtualParadiseParticleEmitterObjectBuilder : VirtualParadiseObjectBuilder
{
    internal VirtualParadiseParticleEmitterObjectBuilder(
        VirtualParadiseClient client,
        VirtualParadiseParticleEmitterObject targetObject,
        ObjectBuilderMode mode
    )
        : base(client, targetObject, mode)
    {
    }

    /// <summary>
    ///     Gets or sets the maximum acceleration.
    /// </summary>
    /// <value>The maximum acceleration, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("acceleration_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> AccelerationMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum acceleration.
    /// </summary>
    /// <value>The minimum acceleration, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("acceleration_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> AccelerationMin { get; set; }

    /// <summary>
    ///     Gets or sets the blend mode.
    /// </summary>
    /// <value>The blend mode, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("blend")]
    [ValueConverter(typeof(StringToEnumConverter<ParticleBlendMode>))]
    public Option<ParticleBlendMode> BlendMode { get; set; }

    /// <summary>
    ///     Gets or sets the maximum color.
    /// </summary>
    /// <value>The maximum color, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("color_max")]
    [ValueConverter(typeof(HexToColorConverter))]
    public Option<Color> ColorMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum color.
    /// </summary>
    /// <value>The minimum color, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("color_min")]
    [ValueConverter(typeof(HexToColorConverter))]
    public Option<Color> ColorMin { get; set; }

    /// <summary>
    ///     Gets or sets the emitter lifespan.
    /// </summary>
    /// <value>The emitter lifespan, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("emitter_lifespan")]
    [ValueConverter(typeof(MillisecondToTimeSpanConverter))]
    public Option<TimeSpan> EmitterLifespan { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this emitter interpolates its values.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if this emitter interpolates its values; otherwise, <see langword="false" />, or
    ///     <see langword="default" /> to leave unchanged.
    /// </value>
    [SerializationKey("interpolate")]
    [ValueConverter(typeof(IntToBoolConverter))]
    public Option<bool> Interpolate { get; set; }

    /// <summary>
    ///     Gets or sets the opacity.
    /// </summary>
    /// <value>The opacity, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("opacity")]
    public Option<double> Opacity { get; set; }

    /// <summary>
    ///     Gets or sets the particle lifespan.
    /// </summary>
    /// <value>The particle lifespan, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("particle_lifespan")]
    [ValueConverter(typeof(MillisecondToTimeSpanConverter))]
    public Option<TimeSpan> ParticleLifespan { get; set; }

    /// <summary>
    ///     Gets or sets the particle type.
    /// </summary>
    /// <value>The particle type, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("particle_type")]
    [ValueConverter(typeof(StringToEnumConverter<ParticleType>))]
    public Option<ParticleType> ParticleType { get; set; }

    /// <summary>
    ///     Gets or sets the release count.
    /// </summary>
    /// <value>The release count, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("release_count")]
    public Option<int> ReleaseCount { get; set; }

    /// <summary>
    ///     Gets or sets the release time.
    /// </summary>
    /// <value>The release time, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("release_time")]
    [ValueConverter(typeof(MillisecondToTimeSpanConverter))]
    public Option<TimeSpan> ReleaseTime { get; set; }

    /// <summary>
    ///     Gets or sets the maximum size.
    /// </summary>
    /// <value>The maximum size, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("size_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SizeMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum size.
    /// </summary>
    /// <value>The minimum size, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("size_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SizeMin { get; set; }

    /// <summary>
    ///     Gets or sets the maximum speed.
    /// </summary>
    /// <value>The maximum speed, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("speed_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SpeedMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum speed.
    /// </summary>
    /// <value>The minimum speed, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("speed_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SpeedMin { get; set; }

    /// <summary>
    ///     Gets or sets the maximum spin.
    /// </summary>
    /// <value>The maximum spin, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("spin_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SpinMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum spin.
    /// </summary>
    /// <value>The minimum spin, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("spin_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> SpinMin { get; set; }

    /// <summary>
    ///     Gets or sets the maximum start angle.
    /// </summary>
    /// <value>The maximum start angle, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("start_angle_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> StartAngleMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum start angle.
    /// </summary>
    /// <value>The minimum start angle, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("start_angle_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> StartAngleMin { get; set; }

    /// <summary>
    ///     Gets or sets the maximum volume.
    /// </summary>
    /// <value>The maximum volume, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("volume_max")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> VolumeMax { get; set; }

    /// <summary>
    ///     Gets or sets the minimum volume.
    /// </summary>
    /// <value>The minimum volume, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("volume_min")]
    [ValueConverter(typeof(Vector3dConverter))]
    public Option<Vector3d> VolumeMin { get; set; }

    /// <summary>
    ///     Gets or sets the tag.
    /// </summary>
    /// <value>The tag, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("tag")]
    public Option<string> Tag { get; set; }

    /// <summary>
    ///     Gets or sets the texture.
    /// </summary>
    /// <value>The texture, or <see langword="default" /> to leave unchanged.</value>
    [SerializationKey("texture")]
    public Option<string> Texture { get; set; }

    /// <summary>
    ///     Sets the maximum volume.
    /// </summary>
    /// <param name="value">The maximum volume, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithAccelerationMax(Option<Vector3d> value)
    {
        AccelerationMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum acceleration.
    /// </summary>
    /// <param name="value">The minimum acceleration, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithAccelerationMin(Option<Vector3d> value)
    {
        AccelerationMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the blend mode.
    /// </summary>
    /// <param name="value">The blend mode, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithBlendMode(Option<ParticleBlendMode> value)
    {
        BlendMode = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum color.
    /// </summary>
    /// <param name="value">The maximum color, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithColorMax(Option<Color> value)
    {
        ColorMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum color.
    /// </summary>
    /// <param name="value">The minimum color, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithColorMin(Option<Color> value)
    {
        ColorMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the emitter lifespan.
    /// </summary>
    /// <param name="value">The emitter lifespan, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithEmitterLifespan(Option<TimeSpan> value)
    {
        EmitterLifespan = value;
        return this;
    }

    /// <summary>
    ///     Sets the interpolation.
    /// </summary>
    /// <param name="value">
    ///     <see langword="true" /> to enable interpolation, <see langword="false" /> to disable interpolation, or
    ///     <see langword="default" /> to leave unchanged.
    /// </param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithInterpolation(Option<bool> value)
    {
        Interpolate = value;
        return this;
    }

    /// <summary>
    ///     Sets the opacity.
    /// </summary>
    /// <param name="value">The opacity, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithOpacity(Option<double> value)
    {
        Opacity = value;
        return this;
    }

    /// <summary>
    ///     Sets the particle lifespan.
    /// </summary>
    /// <param name="value">The particle lifespan, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithParticleLifespan(Option<TimeSpan> value)
    {
        ParticleLifespan = value;
        return this;
    }

    /// <summary>
    ///     Sets the particle type.
    /// </summary>
    /// <param name="value">The particle type, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithParticleType(Option<ParticleType> value)
    {
        ParticleType = value;
        return this;
    }

    /// <summary>
    ///     Sets the release count.
    /// </summary>
    /// <param name="value">The release count, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithReleaseCount(Option<int> value)
    {
        ReleaseCount = value;
        return this;
    }

    /// <summary>
    ///     Sets the release time.
    /// </summary>
    /// <param name="value">The release time, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithReleaseTime(Option<TimeSpan> value)
    {
        ReleaseTime = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum size.
    /// </summary>
    /// <param name="value">The maximum size, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSizeMax(Option<Vector3d> value)
    {
        SizeMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum size.
    /// </summary>
    /// <param name="value">The minimum size, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSizeMin(Option<Vector3d> value)
    {
        SizeMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum speed.
    /// </summary>
    /// <param name="value">The maximum speed, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSpeedMax(Option<Vector3d> value)
    {
        SpeedMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum speed.
    /// </summary>
    /// <param name="value">The minimum speed, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSpeedMin(Option<Vector3d> value)
    {
        SpeedMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum spin.
    /// </summary>
    /// <param name="value">The maximum spin, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSpinMax(Option<Vector3d> value)
    {
        SpinMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum spin.
    /// </summary>
    /// <param name="value">The minimum spin, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithSpinMin(Option<Vector3d> value)
    {
        SpinMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum start angle.
    /// </summary>
    /// <param name="value">The maximum start angle, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithStartAngleMax(Option<Vector3d> value)
    {
        StartAngleMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum start angle.
    /// </summary>
    /// <param name="value">The minimum start angle, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithStartAngleMin(Option<Vector3d> value)
    {
        StartAngleMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the maximum volume.
    /// </summary>
    /// <param name="value">The maximum volume, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithVolumeMax(Option<Vector3d> value)
    {
        VolumeMax = value;
        return this;
    }

    /// <summary>
    ///     Sets the minimum volume.
    /// </summary>
    /// <param name="value">The minimum volume, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithVolumeMin(Option<Vector3d> value)
    {
        VolumeMin = value;
        return this;
    }

    /// <summary>
    ///     Sets the tag.
    /// </summary>
    /// <param name="value">The tag, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithTag(Option<string> value)
    {
        Tag = value;
        return this;
    }

    /// <summary>
    ///     Sets the texture.
    /// </summary>
    /// <param name="value">The texture, or <see langword="default" /> to leave unchanged.</param>
    /// <returns>The current instance.</returns>
    public VirtualParadiseParticleEmitterObjectBuilder WithTexture(Option<string> value)
    {
        Texture = value;
        return this;
    }
}

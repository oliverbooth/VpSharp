using System.Drawing;
using System.Numerics;
using VpSharp.Internal.Attributes;
using VpSharp.Internal.ValueConverters;

// ReSharper disable StringLiteralTypo

namespace VpSharp;

/// <summary>
///     Represents world settings.
/// </summary>
public sealed class WorldSettings
{
    /// <summary>
    ///     Gets a value indicating whether the debug menu is enabled.
    /// </summary>
    /// <value><see langword="true" /> if the debug menu is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_debug_menu")]
    public bool AllowDebugMenu { get; internal set; } = true;

    /// <summary>
    ///     Gets a value indicating whether flying is enabled.
    /// </summary>
    /// <value><see langword="true" /> if flying is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_flight")]
    public bool AllowFlight { get; internal set; } = true;

    /// <summary>
    ///     Gets a value indicating whether pass-through is enabled.
    /// </summary>
    /// <value><see langword="true" /> if pass-through is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_passthrough")]
    public bool AllowPassThrough { get; internal set; } = true;

    /// <summary>
    ///     Gets the name of the avatar list file in the object path.
    /// </summary>
    /// <value>The name of the avatar list file in the object path..</value>
    [SerializationKey("avatar")]
    public string AvatarsFile { get; internal set; } = "avatars";

    /// <summary>
    ///     Gets the entry position of the world, also known as "landing zone" (LZ) or "ground zero" (GZ).
    /// </summary>
    /// <value>The entry position of the world.</value>
    [SerializationKey("entry_point")]
    [ValueConverter(typeof(Vector4ToVector3Converter))]
    public Vector3 EntryPosition { get; internal set; } = Vector3.Zero;

    /// <summary>
    ///     Gets the entry yaw of the world.
    /// </summary>
    /// <value>The entry yaw of the world.</value>
    [SerializationKey("entry_point")]
    [ValueConverter(typeof(VectorToNthComponentConverter), 4)]
    public float EntryYaw { get; internal set; } = 0.0f;

    /// <summary>
    ///     Gets the camera's far-plane distance in decameters.
    /// </summary>
    /// <value>The camera's far-plane distance, in decameters.</value>
    [SerializationKey("farplane")]
    public float FarPlane { get; internal set; }

    /// <summary>
    ///     Gets the camera's field of view.
    /// </summary>
    /// <value>The camera's field of view.</value>
    [SerializationKey("fov")]
    public float FieldOfView { get; internal set; }

    /// <summary>
    ///     Gets the distance at which fog begins.
    /// </summary>
    /// <value>The distance at which fog begins.</value>
    [SerializationKey("fog_begin")]
    public float FogBegin { get; internal set; }

    /// <summary>
    ///     Gets the fog color.
    /// </summary>
    /// <value>The fog color.</value>
    [SerializationKey("fog_color")]
    [ValueConverter(typeof(HexToColorConverter))]
    public Color FogColor { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the density of the fog.
    /// </summary>
    /// <value>The density of the fog.</value>
    [SerializationKey("fog_density")]
    public float FogDensity { get; internal set; }

    /// <summary>
    ///     Gets the distance at which fog ends.
    /// </summary>
    /// <value>The distance at which fog ends.</value>
    [SerializationKey("fog_end")]
    public float FogEnd { get; internal set; }

    /// <summary>
    ///     Gets the fog mode.
    /// </summary>
    /// <value>The fog mode.</value>
    [SerializationKey("fog_mode")]
    [ValueConverter(typeof(StringToEnumConverter<FogMode>))]
    public FogMode FogMode { get; internal set; } = FogMode.None;

    /// <summary>
    ///     Gets the ground object.
    /// </summary>
    /// <value>The ground object. This value will be empty if <see cref="Terrain" /> is <see langword="true" />.</value>
    [SerializationKey("ground")]
    public string Ground { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets a value indicating whether the ground object specified by <see cref="Ground" /> should repeat.
    /// </summary>
    /// <value><see langword="true" /> if the ground should repeat; otherwise, <see langword="false" />.</value>
    [SerializationKey("groundrepeats")]
    public bool GroundRepeats { get; internal set; }

    /// <summary>
    ///     Gets the camera's near-plane distance in decameters.
    /// </summary>
    /// <value>The camera's near-plane distance, in decameters.</value>
    [SerializationKey("nearplane")]
    public float NearPlane { get; internal set; }

    /// <summary>
    ///     Gets the password for extracting password-protected ZIP files from the object path.
    /// </summary>
    /// <value>The password for extracting password-protected ZIP files from the object path.</value>
    [SerializationKey("objectpassword")]
    public string ObjectPassword { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the object path.
    /// </summary>
    /// <value>The object path.</value>
    [SerializationKey("objectpath")]
    public string ObjectPath { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets a value indicating whether per-pixel lighting is recommended.
    /// </summary>
    /// <value><see langword="true" /> if per-pixel lighting is recommended; otherwise, <see langword="false" />.</value>
    [SerializationKey("recommended_per_pixel_lighting")]
    public bool PerPixelLightingRecommended { get; internal set; } = false;

    /// <summary>
    ///     Gets the speed at which users run.
    /// </summary>
    /// <value>The speed at which users run.</value>
    [SerializationKey("run_speed")]
    public float RunSpeed { get; internal set; }

    /// <summary>
    ///     Gets a value indicating whether gradient sky is enabled.
    /// </summary>
    /// <value><see langword="true" /> if gradient sky is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("sky")]
    public bool Sky { get; internal set; } = false;

    /// <summary>
    ///     Gets the color for the first sky cloud layer.
    /// </summary>
    /// <value>The color for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_color")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyClouds1Color { get; internal set; } = ColorF.FromArgb(0, 1, 1, 1);

    /// <summary>
    ///     Gets the color for the second sky cloud layer.
    /// </summary>
    /// <value>The color for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_color")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyClouds2Color { get; internal set; } = ColorF.FromArgb(0, 1, 1, 1);

    /// <summary>
    ///     Gets the scale for the first sky cloud layer.
    /// </summary>
    /// <value>The scale for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_scale")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2 SkyClouds1Scale { get; internal set; } = Vector2.One;

    /// <summary>
    ///     Gets the scale for the second sky cloud layer.
    /// </summary>
    /// <value>The scale for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_scale")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2 SkyClouds2Scale { get; internal set; } = Vector2.One;

    /// <summary>
    ///     Gets the texture for the first sky cloud layer.
    /// </summary>
    /// <value>The texture for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1")]
    public string SkyClouds1Texture { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the texture for the second sky cloud layer.
    /// </summary>
    /// <value>The texture for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2")]
    public string SkyClouds2Texture { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the velocity for the first sky cloud layer.
    /// </summary>
    /// <value>The velocity for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_velocity")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2 SkyClouds1Velocity { get; internal set; } = Vector2.Zero;

    /// <summary>
    ///     Gets the velocity for the second sky cloud layer.
    /// </summary>
    /// <value>The velocity for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_velocity")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2 SkyClouds2Velocity { get; internal set; } = Vector2.Zero;

    /// <summary>
    ///     Gets the first sky gradient color.
    /// </summary>
    /// <value>The first sky gradient color.</value>
    [SerializationKey("sky_color1")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyColor1 { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the second sky gradient color.
    /// </summary>
    /// <value>The second sky gradient color.</value>
    [SerializationKey("sky_color2")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyColor2 { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the third sky gradient color.
    /// </summary>
    /// <value>The third sky gradient color.</value>
    [SerializationKey("sky_color3")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyColor3 { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the fourth sky gradient color.
    /// </summary>
    /// <value>The fourth sky gradient color.</value>
    [SerializationKey("sky_color4")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF SkyColor4 { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the sky color space.
    /// </summary>
    /// <value>The sky color space.</value>
    [SerializationKey("sky_srgb_colors")]
    [ValueConverter(typeof(IntToEnumConverter<ColorSpace>))]
    public ColorSpace SkyColorSpace { get; internal set; } = ColorSpace.sRGB;

    /// <summary>
    ///     Gets the skybox texture.
    /// </summary>
    /// <value>The skybox texture.</value>
    [SerializationKey("skybox")]
    public string Skybox { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the file extension for skybox textures.
    /// </summary>
    /// <value>The file extension for skybox textures.</value>
    [SerializationKey("skybox_extension")]
    public string SkyboxExtension { get; internal set; } = "jpg";

    /// <summary>
    ///     Gets a value indicating whether the X axis of skybox textures should be swapped.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the X axis of skybox textures should be swapped; otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>If <see langword="true" />, _lf and _rt faces are swapped.</remarks>
    [SerializationKey("skybox_extension")]
    public bool SkyboxSwapX { get; internal set; } = false;

    /// <summary>
    ///     Gets a value indicating whether terrain is enabled.
    /// </summary>
    /// <value><see langword="true" /> if terrain is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("terrain")]
    public bool Terrain { get; internal set; } = false;

    /// <summary>
    ///     Gets the vertical offset for the terrain.
    /// </summary>
    /// <value>The vertical offset for the terrain.</value>
    [SerializationKey("terrainoffset")]
    public float TerrainOffset { get; internal set; }

    /// <summary>
    ///     Gets the scale factor for the terrain grid.
    /// </summary>
    /// <value>The scale factor for the terrain grid.</value>
    [SerializationKey("terrainscale")]
    public float TerrainScale { get; internal set; } = 1.0f;

    /// <summary>
    ///     Gets the speed at which users walk.
    /// </summary>
    /// <value>The speed at which users walk.</value>
    [SerializationKey("walk_speed")]
    public float WalkSpeed { get; internal set; }

    /// <summary>
    ///     Gets the URL of the in-world web overlay.
    /// </summary>
    /// <value>The URL of the in-world web overlay.</value>
    [SerializationKey("web_overlay")]
    public string WebOverlay { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the welcome message.
    /// </summary>
    /// <value>The welcome message.</value>
    [SerializationKey("welcome")]
    public string WelcomeMessage { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the ambient light color of the world.
    /// </summary>
    /// <value>The ambient light color.</value>
    [SerializationKey("worldlight_ambient")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF WorldLightAmbient { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the diffuse light color of the world.
    /// </summary>
    /// <value>The diffuse light color.</value>
    [SerializationKey("worldlight_diffuse")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF WorldLightDiffuse { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the world light position.
    /// </summary>
    /// <value>The world light position.</value>
    [SerializationKey("worldlight_position")]
    [ValueConverter(typeof(Vector3Converter))]
    public Vector3 WorldLightPosition { get; internal set; } = new(0.4f, 1.0f, -0.08f);

    /// <summary>
    ///     Gets the specular light color of the world.
    /// </summary>
    /// <value>The specular light color.</value>
    [SerializationKey("worldlight_specular")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF WorldLightSpecular { get; internal set; } = Color.White;

    /// <summary>
    ///     Gets the name of the world.
    /// </summary>
    /// <value>The name of the world.</value>
    [SerializationKey("worldname")]
    public string WorldName { get; internal set; } = string.Empty;
}

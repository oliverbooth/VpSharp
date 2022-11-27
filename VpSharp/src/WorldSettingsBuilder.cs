using System.Drawing;
using System.Numerics;
using VpSharp.Internal;
using VpSharp.Internal.Attributes;
using VpSharp.Internal.ValueConverters;

namespace VpSharp;

/// <summary>
///     Provides mutability for <see cref="WorldSettings" />.
/// </summary>
public sealed class WorldSettingsBuilder
{
    private readonly VirtualParadiseClient _client;
    private readonly int? _session;

    internal WorldSettingsBuilder(VirtualParadiseClient client, int? session = null)
    {
        _client = client;
        _session = session;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the debug menu is enabled.
    /// </summary>
    /// <value><see langword="true" /> if the debug menu is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_debug_menu")]
    public bool? AllowDebugMenu { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether flying is enabled.
    /// </summary>
    /// <value><see langword="true" /> if flying is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_flight")]
    public bool? AllowFlight { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether pass-through is enabled.
    /// </summary>
    /// <value><see langword="true" /> if pass-through is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("allow_passthrough")]
    public bool? AllowPassThrough { get; set; }

    /// <summary>
    ///     Gets or sets the name of the avatar list file in the object path.
    /// </summary>
    /// <value>The name of the avatar list file in the object path..</value>
    [SerializationKey("avatar")]
    public string? AvatarsFile { get; set; }

    /// <summary>
    ///     Gets or sets the entry position of the world, also known as "landing zone" (LZ) or "ground zero" (GZ).
    /// </summary>
    /// <value>The entry position of the world.</value>
    [SerializationKey("entry_point")]
    [ValueConverter(typeof(Vector4ToVector3Converter))]
    public Vector3? EntryPosition { get; set; }

    /// <summary>
    ///     Gets or sets the entry yaw of the world.
    /// </summary>
    /// <value>The entry yaw of the world.</value>
    [SerializationKey("entry_point")]
    [ValueConverter(typeof(VectorToNthComponentConverter), 4)]
    public float? EntryYaw { get; set; }

    /// <summary>
    ///     Gets or sets the camera's far-plane distance in decameters.
    /// </summary>
    /// <value>The camera's far-plane distance, in decameters.</value>
    [SerializationKey("farplane")]
    public float? FarPlane { get; set; }

    /// <summary>
    ///     Gets or sets the camera's field of view.
    /// </summary>
    /// <value>The camera's field of view.</value>
    [SerializationKey("fov")]
    public float? FieldOfView { get; set; }

    /// <summary>
    ///     Gets or sets the distance at which fog begins.
    /// </summary>
    /// <value>The distance at which fog begins.</value>
    [SerializationKey("fog_begin")]
    public float? FogBegin { get; set; }

    /// <summary>
    ///     Gets or sets the fog color.
    /// </summary>
    /// <value>The fog color.</value>
    [SerializationKey("fog_color")]
    [ValueConverter(typeof(HexToColorConverter))]
    public Color? FogColor { get; set; }

    /// <summary>
    ///     Gets or sets the density of the fog.
    /// </summary>
    /// <value>The density of the fog.</value>
    [SerializationKey("fog_density")]
    public float? FogDensity { get; set; }

    /// <summary>
    ///     Gets or sets the distance at which fog ends.
    /// </summary>
    /// <value>The distance at which fog ends.</value>
    [SerializationKey("fog_end")]
    public float? FogEnd { get; set; }

    /// <summary>
    ///     Gets or sets the fog mode.
    /// </summary>
    /// <value>The fog mode.</value>
    [SerializationKey("fog_mode")]
    [ValueConverter(typeof(StringToEnumConverter<FogMode>))]
    public FogMode? FogMode { get; set; }

    /// <summary>
    ///     Gets or sets the ground object.
    /// </summary>
    /// <value>The ground object. This value will be empty if <see cref="Terrain" /> is <see langword="true" />.</value>
    [SerializationKey("ground")]
    public string? Ground { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the ground object specified by <see cref="Ground" /> should repeat.
    /// </summary>
    /// <value><see langword="true" /> if the ground should repeat; otherwise, <see langword="false" />.</value>
    [SerializationKey("groundrepeats")]
    public bool? GroundRepeats { get; set; }

    /// <summary>
    ///     Gets or sets the camera's near-plane distance in decameters.
    /// </summary>
    /// <value>The camera's near-plane distance, in decameters.</value>
    [SerializationKey("nearplane")]
    public float? NearPlane { get; set; }

    /// <summary>
    ///     Gets or sets the password for extracting password-protected ZIP files from the object path.
    /// </summary>
    /// <value>The password for extracting password-protected ZIP files from the object path.</value>
    [SerializationKey("objectpassword")]
    public string? ObjectPassword { get; set; }

    /// <summary>
    ///     Gets or sets the object path.
    /// </summary>
    /// <value>The object path.</value>
    [SerializationKey("objectpath")]
    public string? ObjectPath { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether per-pixel lighting is recommended.
    /// </summary>
    /// <value><see langword="true" /> if per-pixel lighting is recommended; otherwise, <see langword="false" />.</value>
    [SerializationKey("recommended_per_pixel_lighting")]
    public bool? PerPixelLightingRecommended { get; set; }

    /// <summary>
    ///     Gets or sets the speed at which users run.
    /// </summary>
    /// <value>The speed at which users run.</value>
    [SerializationKey("run_speed")]
    public float? RunSpeed { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether gradient sky is enabled.
    /// </summary>
    /// <value><see langword="true" /> if gradient sky is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("sky")]
    public bool? Sky { get; set; }

    /// <summary>
    ///     Gets or sets the color for the first sky cloud layer.
    /// </summary>
    /// <value>The color for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_color")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyClouds1Color { get; set; }

    /// <summary>
    ///     Gets or sets the color for the second sky cloud layer.
    /// </summary>
    /// <value>The color for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_color")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyClouds2Color { get; set; }

    /// <summary>
    ///     Gets or sets the scale for the first sky cloud layer.
    /// </summary>
    /// <value>The scale for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_scale")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2? SkyClouds1Scale { get; set; }

    /// <summary>
    ///     Gets or sets the scale for the second sky cloud layer.
    /// </summary>
    /// <value>The scale for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_scale")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2? SkyClouds2Scale { get; set; }

    /// <summary>
    ///     Gets or sets the texture for the first sky cloud layer.
    /// </summary>
    /// <value>The texture for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1")]
    public string SkyClouds1Texture { get; set; }

    /// <summary>
    ///     Gets or sets the texture for the second sky cloud layer.
    /// </summary>
    /// <value>The texture for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2")]
    public string SkyClouds2Texture { get; set; }

    /// <summary>
    ///     Gets or sets the velocity for the first sky cloud layer.
    /// </summary>
    /// <value>The velocity for the first sky cloud layer.</value>
    [SerializationKey("sky_clouds1_velocity")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2? SkyClouds1Velocity { get; set; }

    /// <summary>
    ///     Gets or sets the velocity for the second sky cloud layer.
    /// </summary>
    /// <value>The velocity for the second sky cloud layer.</value>
    [SerializationKey("sky_clouds2_velocity")]
    [ValueConverter(typeof(Vector2Converter))]
    public Vector2? SkyClouds2Velocity { get; set; }

    /// <summary>
    ///     Gets or sets the first sky gradient color.
    /// </summary>
    /// <value>The first sky gradient color.</value>
    [SerializationKey("sky_color1")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyColor1 { get; set; }

    /// <summary>
    ///     Gets or sets the second sky gradient color.
    /// </summary>
    /// <value>The second sky gradient color.</value>
    [SerializationKey("sky_color2")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyColor2 { get; set; }

    /// <summary>
    ///     Gets or sets the third sky gradient color.
    /// </summary>
    /// <value>The third sky gradient color.</value>
    [SerializationKey("sky_color3")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyColor3 { get; set; }

    /// <summary>
    ///     Gets or sets the fourth sky gradient color.
    /// </summary>
    /// <value>The fourth sky gradient color.</value>
    [SerializationKey("sky_color4")]
    [ValueConverter(typeof(Vector4ToColorConverter))]
    public ColorF? SkyColor4 { get; set; }

    /// <summary>
    ///     Gets or sets the sky color space.
    /// </summary>
    /// <value>The sky color space.</value>
    [SerializationKey("sky_srgb_colors")]
    [ValueConverter(typeof(IntToEnumConverter<ColorSpace>))]
    public ColorSpace? SkyColorSpace { get; set; }

    /// <summary>
    ///     Gets or sets the skybox texture.
    /// </summary>
    /// <value>The skybox texture.</value>
    [SerializationKey("skybox")]
    public string Skybox { get; set; }

    /// <summary>
    ///     Gets or sets the file extension for skybox textures.
    /// </summary>
    /// <value>The file extension for skybox textures.</value>
    [SerializationKey("skybox_extension")]
    public string SkyboxExtension { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the X axis of skybox textures should be swapped.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the X axis of skybox textures should be swapped; otherwise, <see langword="false" />.
    /// </value>
    /// <remarks>If <see langword="true" />, _lf and _rt faces are swapped.</remarks>
    [SerializationKey("skybox_extension")]
    public bool? SkyboxSwapX { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether terrain is enabled.
    /// </summary>
    /// <value><see langword="true" /> if terrain is enabled; otherwise, <see langword="false" />.</value>
    [SerializationKey("terrain")]
    public bool? Terrain { get; set; }

    /// <summary>
    ///     Gets or sets the vertical offset for the terrain.
    /// </summary>
    /// <value>The vertical offset for the terrain.</value>
    [SerializationKey("terrainoffset")]
    public float? TerrainOffset { get; set; }

    /// <summary>
    ///     Gets or sets the scale factor for the terrain grid.
    /// </summary>
    /// <value>The scale factor for the terrain grid.</value>
    [SerializationKey("terrainscale")]
    public float? TerrainScale { get; set; }

    /// <summary>
    ///     Gets or sets the speed at which users walk.
    /// </summary>
    /// <value>The speed at which users walk.</value>
    [SerializationKey("walk_speed")]
    public float? WalkSpeed { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the in-world web overlay.
    /// </summary>
    /// <value>The URL of the in-world web overlay.</value>
    [SerializationKey("web_overlay")]
    public string WebOverlay { get; set; }

    /// <summary>
    ///     Gets or sets the welcome message.
    /// </summary>
    /// <value>The welcome message.</value>
    [SerializationKey("welcome")]
    public string WelcomeMessage { get; set; }

    /// <summary>
    ///     Gets or sets the ambient light color of the world.
    /// </summary>
    /// <value>The ambient light color.</value>
    [SerializationKey("worldlight_ambient")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF? WorldLightAmbient { get; set; }

    /// <summary>
    ///     Gets or sets the diffuse light color of the world.
    /// </summary>
    /// <value>The diffuse light color.</value>
    [SerializationKey("worldlight_diffuse")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF? WorldLightDiffuse { get; set; }

    /// <summary>
    ///     Gets or sets the world light position.
    /// </summary>
    /// <value>The world light position.</value>
    [SerializationKey("worldlight_position")]
    [ValueConverter(typeof(Vector3Converter))]
    public Vector3? WorldLightPosition { get; set; }

    /// <summary>
    ///     Gets or sets the specular light color of the world.
    /// </summary>
    /// <value>The specular light color.</value>
    [SerializationKey("worldlight_specular")]
    [ValueConverter(typeof(Vector3ToColorConverter))]
    public ColorF? WorldLightSpecular { get; set; }

    /// <summary>
    ///     Gets or sets the name of the world.
    /// </summary>
    /// <value>The name of the world.</value>
    [SerializationKey("worldname")]
    public string WorldName { get; set; }

    internal void SendChanges()
    {
        int session = _session ?? 0;
        IReadOnlyDictionary<string, string?> dictionary = WorldSettingsConverter.ToDictionary(this);

        lock (_client.Lock)
        {
            foreach ((string key, string? value) in dictionary)
            {
                if (value is null)
                    continue;

                var reason = (ReasonCode)Native.vp_world_setting_set(_client.NativeInstanceHandle, key, value, session);

                if (reason == ReasonCode.NotAllowed)
                    throw new UnauthorizedAccessException("Not allowed to modify world settings.");
            }
        }
    }
}
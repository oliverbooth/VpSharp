namespace VpSharp;

/// <summary>
///     Represents configuration for <see cref="VirtualParadiseClient" />.
/// </summary>
public sealed class VirtualParadiseConfiguration
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseConfiguration" /> class.
    /// </summary>
    public VirtualParadiseConfiguration()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseConfiguration" /> class by copying the values from
    ///     another instance.
    /// </summary>
    /// <param name="configuration">The configuration to copy.</param>
    public VirtualParadiseConfiguration(VirtualParadiseConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (configuration.Application is ({ } name, { } version))
        {
            Application = new Application(name, version);
        }

        AutoQuery = configuration.AutoQuery;
        BotName = new string(configuration.BotName);
        Services = configuration.Services;
        Password = new string(configuration.Password);
        Username = new string(configuration.Username);
    }

    /// <summary>
    ///     Sets the application.
    /// </summary>
    /// <value>The application.</value>
    public Application? Application { internal get; set; }

    /// <summary>
    ///     Sets a value indicating whether or not the client should automatically query and cache the whole world upon entry.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the client should query the world on entry; otherwise, <see langword="false" />.
    /// </value>
    public bool AutoQuery { internal get; set; }

    /// <summary>
    ///     Sets the bot name.
    /// </summary>
    /// <value>The bot name.</value>
    public string BotName { internal get; set; } = string.Empty;

    /// <summary>
    ///     Sets the login password.
    /// </summary>
    /// <value>The login password.</value>
    public string Password { internal get; set; } = string.Empty;

    /// <summary>
    ///     Sets the service provider.
    /// </summary>
    /// <value>The service provider.</value>
    public IServiceProvider? Services { internal get; set; }

    /// <summary>
    ///     Sets the login username.
    /// </summary>
    /// <value>The login username.</value>
    public string Username { internal get; set; } = string.Empty;
}

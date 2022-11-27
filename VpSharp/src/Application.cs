namespace VpSharp;

/// <summary>
///     Represents an application.
/// </summary>
public sealed class Application
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Application" /> class.
    /// </summary>
    /// <param name="name">The name of this application.</param>
    /// <param name="version">The version of this application.</param>
    public Application(string name, string version)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));

        if (string.IsNullOrWhiteSpace(version))
        {
            Version = null;
        }
        else
        {
            Version = version;
        }
    }

    /// <summary>
    ///     Gets the name of this application.
    /// </summary>
    /// <value>The name of this application.</value>
    public string Name { get; }

    /// <summary>
    ///     Gets the version of this application.
    /// </summary>
    /// <value>The version of this application.</value>
    public string? Version { get; }

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="name">The application name.</param>
    /// <param name="version">The application version./</param>
    public void Deconstruct(out string name, out string? version)
    {
        name = Name;
        version = Version;
    }
}

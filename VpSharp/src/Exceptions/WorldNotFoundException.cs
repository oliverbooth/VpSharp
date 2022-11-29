namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an attempt to access a world does not exist.
/// </summary>
public sealed class WorldNotFoundException : Exception
{
    /// <inheritdoc />
    public WorldNotFoundException()
    {
    }

    /// <inheritdoc />
    public WorldNotFoundException(string worldName)
        : base($"No world with the name {worldName} was found.")
    {
    }

    /// <inheritdoc />
    public WorldNotFoundException(string worldName, Exception innerException)
        : base($"No world with the name {worldName} was found.", innerException)
    {
    }
}

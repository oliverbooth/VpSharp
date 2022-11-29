namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when a user could not be found.
/// </summary>
public sealed class UserNotFoundException : Exception
{
    /// <inheritdoc />
    public UserNotFoundException()
    {
    }

    /// <inheritdoc />
    public UserNotFoundException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

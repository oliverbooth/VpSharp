namespace VpSharp.Exceptions;

public sealed class UserNotFoundException : Exception
{
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
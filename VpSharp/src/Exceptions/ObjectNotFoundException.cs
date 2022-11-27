namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an operation was performed on an object that does not exist.
/// </summary>
public sealed class ObjectNotFoundException : Exception
{
    /// <inheritdoc />
    public ObjectNotFoundException()
    {
    }

    /// <inheritdoc />
    public ObjectNotFoundException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public ObjectNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

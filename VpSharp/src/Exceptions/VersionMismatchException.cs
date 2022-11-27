namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when the version of the .NET wrapper and the version of the native SDK do not match.
/// </summary>
public sealed class VersionMismatchException : Exception
{
    /// <inheritdoc />
    public VersionMismatchException()
        : this("The version of the .NET wrapper and the version of the native SDK do not match.")
    {
    }

    /// <inheritdoc />
    public VersionMismatchException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public VersionMismatchException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
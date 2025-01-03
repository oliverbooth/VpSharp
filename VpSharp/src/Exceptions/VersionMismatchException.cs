using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when the version of the .NET wrapper and the version of the native SDK do not match.
/// </summary>
public sealed class VersionMismatchException : VirtualParadiseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VersionMismatchException" /> class.
    /// </summary>
    public VersionMismatchException()
        : this("The version of the .NET wrapper and the version of the native SDK do not match.")
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VersionMismatchException" /> class.
    /// </summary>
    public VersionMismatchException(string message)
        : base(ReasonCode.VersionMismatch, message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VersionMismatchException" /> class.
    /// </summary>
    public VersionMismatchException(string message, Exception innerException)
        : base(ReasonCode.VersionMismatch, message, innerException)
    {
    }
}

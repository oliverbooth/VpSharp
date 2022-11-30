using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an operation was attempted that requires a connection to a world, but the instance is
///     not currently connected to one.
/// </summary>
public sealed class NotInWorldException : VirtualParadiseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NotInWorldException" /> class.
    /// </summary>
    public NotInWorldException()
        : base(ReasonCode.NotInWorld)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotInWorldException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotInWorldException(string message)
        : base(ReasonCode.NotInWorld, message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotInWorldException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public NotInWorldException(string message, Exception innerException)
        : base(ReasonCode.NotInWorld, message, innerException)
    {
    }
}

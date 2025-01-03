using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when a user could not be found.
/// </summary>
public sealed class UserNotFoundException : VirtualParadiseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotFoundException" /> class.
    /// </summary>
    public UserNotFoundException()
        : base(ReasonCode.NoSuchUser)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotFoundException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UserNotFoundException(string message)
        : base(ReasonCode.NoSuchUser, message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotFoundException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public UserNotFoundException(string message, Exception innerException)
        : base(ReasonCode.NoSuchUser, message, innerException)
    {
    }
}

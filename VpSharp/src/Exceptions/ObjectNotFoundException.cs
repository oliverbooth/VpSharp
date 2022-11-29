using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an operation was performed on an object that does not exist.
/// </summary>
public sealed class ObjectNotFoundException : VirtualParadiseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectNotFoundException" /> class.
    /// </summary>
    public ObjectNotFoundException()
        : base(ReasonCode.ObjectNotFound)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectNotFoundException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ObjectNotFoundException(string message)
        : base(ReasonCode.ObjectNotFound, message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectNotFoundException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public ObjectNotFoundException(string message, Exception innerException)
        : base(ReasonCode.ObjectNotFound, message, innerException)
    {
    }
}

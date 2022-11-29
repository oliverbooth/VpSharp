using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an operation fails in the native SDK.
/// </summary>
public class VirtualParadiseException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    public VirtualParadiseException()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VirtualParadiseException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public VirtualParadiseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    /// <param name="reasonCode">The underlying reason code.</param>
    internal VirtualParadiseException(ReasonCode reasonCode)
    {
        ReasonCode = reasonCode;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    /// <param name="reasonCode">The underlying reason code.</param>
    /// <param name="message">The message that describes the error.</param>
    internal VirtualParadiseException(ReasonCode reasonCode, string message)
        : base(message)
    {
        ReasonCode = reasonCode;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseException" /> class.
    /// </summary>
    /// <param name="reasonCode">The underlying reason code.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    internal VirtualParadiseException(ReasonCode reasonCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ReasonCode = reasonCode;
    }

    /// <summary>
    ///     Gets the reason code.
    /// </summary>
    /// <value>The reason code.</value>
    internal ReasonCode ReasonCode { get; }
}

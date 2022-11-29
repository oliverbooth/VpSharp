using VpSharp.Internal;

namespace VpSharp.Exceptions;

/// <summary>
///     The exception that is thrown when an attempt to access a world does not exist.
/// </summary>
public sealed class WorldNotFoundException : VirtualParadiseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WorldNotFoundException" /> class.
    /// </summary>
    public WorldNotFoundException() : base(ReasonCode.WorldNotFound)
    {
        WorldName = string.Empty;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorldNotFoundException" /> class.
    /// </summary>
    /// <param name="worldName">The name of the world that wasn't found.</param>
    public WorldNotFoundException(string worldName)
        : this(worldName, $"No world with the name {worldName} was found.")
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorldNotFoundException" /> class.
    /// </summary>
    /// <param name="worldName">The name of the world that wasn't found.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public WorldNotFoundException(string worldName, Exception innerException)
        : this(worldName, $"No world with the name {worldName} was found.", innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorldNotFoundException" /> class.
    /// </summary>
    /// <param name="worldName">The name of the world that wasn't found.</param>
    /// <param name="message">The message that describes the error.</param>
    public WorldNotFoundException(string worldName, string message)
        : base(ReasonCode.WorldNotFound, message)
    {
        WorldName = worldName;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorldNotFoundException" /> class.
    /// </summary>
    /// <param name="worldName">The name of the world that wasn't found.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
    ///     exception is specified.
    /// </param>
    public WorldNotFoundException(string worldName, string message, Exception innerException)
        : base(ReasonCode.WorldNotFound, message, innerException)
    {
        WorldName = worldName;
    }

    /// <summary>
    ///     Gets the name of the world that wasn't found.
    /// </summary>
    /// <value>The name of the world that wasn't found.</value>
    public string WorldName { get; }
}

namespace VpSharp.Entities;

/// <summary>
///     Represents a message.
/// </summary>
public interface IMessage
{
    /// <summary>
    ///     Gets the author of the message.
    /// </summary>
    /// <value>The avatar which authored the message.</value>
    IAvatar Author { get; }

    /// <summary>
    ///     Gets the content of the message.
    /// </summary>
    /// <value>The content.</value>
    string Content { get; }

    /// <summary>
    ///     Gets the apparent sender's name of the message.
    /// </summary>
    /// <value>The sender's name.</value>
    string Name { get; }
}

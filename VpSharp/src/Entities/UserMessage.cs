namespace VpSharp.Entities;

/// <summary>
///     Represents a chat message that was sent by a user.
/// </summary>
public sealed class UserMessage : Message, IUserMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserMessage" /> class.
    /// </summary>
    /// <param name="author">The author of the message.</param>
    /// <param name="content">The content of the message.</param>
    public UserMessage(IAvatar author, string content)
        : base(author, author.Name, content)
    {
    }
}

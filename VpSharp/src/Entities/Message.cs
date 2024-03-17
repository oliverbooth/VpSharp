namespace VpSharp.Entities;

/// <summary>
///     Represents a message.
/// </summary>
public abstract class Message : IMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Message" /> class.
    /// </summary>
    /// <param name="author">The author of the message.</param>
    /// <param name="name">The apparent sender's name of the message.</param>
    /// <param name="content">The content of the message.</param>
    protected Message(IAvatar author, string name, string content)
    {
        Author = author;
        Content = content;
        Name = name;
    }

    /// <inheritdoc />
    public IAvatar Author { get; }

    /// <inheritdoc />
    public string Content { get; }

    /// <inheritdoc />
    public string Name { get; }
}

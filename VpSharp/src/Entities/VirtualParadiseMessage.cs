using System.Drawing;

namespace VpSharp.Entities;

/// <summary>
///     Represents a message.
/// </summary>
public sealed class VirtualParadiseMessage
{
    internal VirtualParadiseMessage(
        MessageType type,
        string? name,
        string content,
        VirtualParadiseAvatar author,
        FontStyle style,
        Color color)
    {
        Type = type;
        Name = string.IsNullOrWhiteSpace(name) ? null : name;
        Content = content;
        Author = author;
        Style = style;
        Color = color;
    }

    /// <summary>
    ///     Gets the message author.
    /// </summary>
    /// <value>The message author.</value>
    public VirtualParadiseAvatar Author { get; }

    /// <summary>
    ///     Gets the message content.
    /// </summary>
    /// <value>The message content.</value>
    public string Content { get; }

    /// <summary>
    ///     Gets the message name.
    /// </summary>
    /// <value>The message name. This will always be equal to the name of the <see cref="Author" /> for chat messages.</value>
    public string? Name { get; }

    /// <summary>
    ///     Gets the message color.
    /// </summary>
    /// <value>The message color. This will always be <see cref="System.Drawing.Color.Black" /> for chat messages.</value>
    public Color Color { get; }

    /// <summary>
    ///     Gets the message font style.
    /// </summary>
    /// <value>The message font style. This will always be <see cref="FontStyle.Regular" /> for chat messages.</value>
    public FontStyle Style { get; }

    /// <summary>
    ///     Gets the type of this message.
    /// </summary>
    /// <value>The type of this message.</value>
    public MessageType Type { get; }
}

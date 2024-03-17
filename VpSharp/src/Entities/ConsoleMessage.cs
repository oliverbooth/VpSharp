using System.Drawing;

namespace VpSharp.Entities;

/// <summary>
///     Represents a chat message that was sent by a user.
/// </summary>
public sealed class ConsoleMessage : Message, IConsoleMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserMessage" /> class.
    /// </summary>
    /// <param name="author">The author of the message.</param>
    /// <param name="name">The apparent sender's name of the message.</param>
    /// <param name="content">The content of the message.</param>
    /// <param name="color">A <see cref="System.Drawing.Color" /> value representing the color of the message.</param>
    /// <param name="fontStyle">A <see cref="FontStyle" /> value representing the font style of the message.</param>
    public ConsoleMessage(IAvatar author, string name, string content, Color color, FontStyle fontStyle)
        : base(author, name, content)
    {
        Color = color;
        Style = fontStyle;
    }

    /// <inheritdoc />
    public Color Color { get; }

    /// <inheritdoc />
    public FontStyle Style { get; }
}

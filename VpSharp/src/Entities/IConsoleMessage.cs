using System.Drawing;

namespace VpSharp.Entities;

/// <summary>
///     Represents a console message.
/// </summary>
public interface IConsoleMessage : IMessage
{
    /// <summary>
    ///     Gets the color of the message.
    /// </summary>
    /// <value>A <see cref="System.Drawing.Color" /> value representing the color.</value>
    Color Color { get; }

    /// <summary>
    ///     Gets the font style of the message.
    /// </summary>
    /// <value>A <see cref="FontStyle" /> value representing the font style.</value>
    FontStyle Style { get; }
}

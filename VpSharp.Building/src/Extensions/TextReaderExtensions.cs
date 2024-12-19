using Cysharp.Text;

namespace VpSharp.Building.Extensions;

/// <summary>
///     Extension methods for <see cref="TextReader" />.
/// </summary>
internal static class TextReaderExtensions
{
    /// <summary>
    ///     Reads a token from the <see cref="TextReader" />.
    /// </summary>
    /// <param name="reader">The reader from which the token will be read.</param>
    /// <returns>The next token from the reader.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <see langword="null" />.</exception>
    public static string? ReadToken(this TextReader reader)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        using Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        Span<char> buffer = stackalloc char[1];

        bool isQuoted = false;
        bool isEscaped = false;
        while (reader.Read(buffer) > 0)
        {
            char character = buffer[0];
            if (char.IsWhiteSpace(character))
            {
                break;
            }

            if (character is '"')
            {
                isQuoted = !isQuoted;
            }
            else if (character is '\\' && !isEscaped)
            {
                isEscaped = true;
                continue;
            }
            else if (isEscaped)
            {
                isEscaped = false;
            }
            else if ((character is ',' or ';') && !isQuoted)
            {
                break;
            }

            builder.Append(character);
        }

        return builder.Length == 0 ? null : builder.ToString();
    }

    /// <summary>
    ///     Skips any whitespace characters in the <see cref="TextReader" />.
    /// </summary>
    /// <param name="reader">The reader from which to skip whitespace.</param>
    /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <see langword="null" />.</exception>
    public static void SkipWhitespace(this TextReader reader)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        int c;
        while ((c = reader.Peek()) != -1)
        {
            if (!char.IsWhiteSpace((char)c))
            {
                break;
            }

            reader.Read();
        }
    }
}

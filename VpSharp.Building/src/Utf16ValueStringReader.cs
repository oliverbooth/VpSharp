using Cysharp.Text;

namespace VpSharp.Building;

/// <summary>
///     Represents an allocation-free <see cref="StringReader" />.
/// </summary>
public ref struct Utf16ValueStringReader
{
    private readonly ReadOnlySpan<char> _source;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Utf16ValueStringReader" /> structure.
    /// </summary>
    /// <param name="source">The span of characters to read.</param>
    public Utf16ValueStringReader(ReadOnlySpan<char> source)
    {
        _source = source;
    }

    /// <summary>
    ///     Gets the current position of the reader.
    /// </summary>
    /// <value>The current position of the reader.</value>
    public int Position { get; private set; }

    /// <summary>
    ///     Reads the next character without changing the state of the reader or the character source. Returns the next available
    ///     character without actually reading it from the reader.
    /// </summary>
    /// <returns>An integer representing the next character to be read, or -1 if no more characters are available.</returns>
    public int Peek()
    {
        if (Position >= _source.Length)
        {
            return -1;
        }

        return _source[Position];
    }

    /// <summary>
    ///     Reads the next character from the reader and advances the character position by one character.
    /// </summary>
    /// <returns>The next character from the reader, or -1 if no more characters are available.</returns>
    public int Read()
    {
        if (Position >= _source.Length)
        {
            return -1;
        }

        return _source[Position++];
    }

    /// <summary>
    ///     Reads all the characters from the input span of characters, starting at the current position, and advances the
    ///     current position to the end of the input span of characters.
    /// </summary>
    /// <param name="destination">
    ///     When this method returns, contains the characters read from the current source. If the total number of characters read
    ///     is zero, the span remains unmodified.
    /// </param>
    /// <returns>The total number of characters read into the buffer.</returns>
    public int Read(scoped Span<char> destination)
    {
        int charsRead = Math.Min(destination.Length, _source.Length - Position);
        _source.Slice(Position, charsRead).CopyTo(destination);
        Position += charsRead;
        return charsRead;
    }

    /// <summary>
    ///     Reads a line of characters from the current span of characters and returns the data as a
    ///     <see cref="ReadOnlySpan{T}" /> of <see cref="char" />.
    /// </summary>
    /// <returns>
    ///     The next line from the current span of characters, or <see cref="ReadOnlySpan{T}.Empty" /> if the end of the span of
    ///     characters has been reached.
    /// </returns>
    public ReadOnlySpan<char> ReadLine()
    {
        int start = Position;

        while (Position < _source.Length)
        {
            if (_source[Position] == '\n')
            {
                ReadOnlySpan<char> line = _source.Slice(start, Position - start);
                Position++;
                return line;
            }

            Position++;
        }

        return _source[start..];
    }

    /// <summary>
    ///     Reads all characters from the current position to the end of the string and returns them as a single span of
    ///     characters.
    /// </summary>
    /// <returns>The content from the current position to the end of the underlying span of characters.</returns>
    public ReadOnlySpan<char> ReadToEnd()
    {
        ReadOnlySpan<char> remaining = _source[Position..];
        Position = _source.Length;
        return remaining;
    }

    /// <summary>
    ///     Reads a token from the reader.
    /// </summary>
    /// <param name="destination">
    ///     When this method returns, contains the token read from the reader, if the token was read successfully; otherwise,
    ///     the span will be unchanged.
    /// </param>
    /// <returns>The next token from the reader.</returns>
    public int ReadToken(scoped Span<char> destination)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();

        bool isQuoted = false;
        bool isEscaped = false;

        int c;
        while ((c = Read()) != -1)
        {
            char character = (char)c;

            if (char.IsWhiteSpace(character))
            {
                break;
            }

            if (!HandleTokenCharacter(ref builder, character, ref isEscaped, ref isQuoted))
            {
                break;
            }
        }

        if (builder.Length == 0)
        {
            builder.Dispose();
            return 0;
        }

        int length = builder.Length;
        builder.AsSpan().CopyTo(destination[..length]);
        builder.Dispose();
        return length;
    }

    /// <summary>
    ///     Skips any whitespace characters in the reader.
    /// </summary>
    public void SkipWhitespace()
    {
        int c;
        while ((c = Peek()) != -1)
        {
            if (!char.IsWhiteSpace((char)c))
            {
                break;
            }

            Read();
        }
    }

    private static bool HandleTokenCharacter(ref Utf16ValueStringBuilder builder,
        char character,
        ref bool isEscaped,
        ref bool isQuoted)
    {
        switch (character)
        {
            case '"' when !isEscaped:
                isQuoted = !isQuoted;
                break;

            case '\\' when !isEscaped:
                isEscaped = true;
                break;

            case ',' or ';' when !isQuoted:
                return false;

            default:
                isEscaped = false;
                builder.Append(character);
                break;
        }

        return true;
    }
}

using System.Globalization;
using System.Text.Unicode;
using Cysharp.Text;

namespace VpSharp.Building.Serialization;

/// <summary>
///     Represents a UTF-8 Virtual Paradise action reader.
/// </summary>
public ref struct Utf8ActionReader
{
    private readonly ReadOnlySpan<byte> _bytes;
    private readonly Span<char> _buffer = new char[1024];
    private readonly Stream? _stream;
    private int _position;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Utf8ActionReader" /> structure.
    /// </summary>
    /// <param name="bytes">The span of bytes to read.</param>
    public Utf8ActionReader(ReadOnlySpan<byte> bytes)
    {
        _bytes = bytes;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Utf8ActionReader" /> structure.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="stream" /> is not readable.</para>
    ///     -or-
    ///     <para><paramref name="stream" /> is not seekable.</para>
    /// </exception>
    public Utf8ActionReader(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));

        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream is not readable.", nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new ArgumentException("The stream is not seekable.", nameof(stream));
        }
    }

    /// <summary>
    ///     Gets the current token.
    /// </summary>
    /// <value>The current token.</value>
    public Token CurrentToken { get; private set; }

    /// <summary>
    ///     Gets the current token as a boolean value.
    /// </summary>
    /// <returns>The current token as a boolean value.</returns>
    /// <exception cref="InvalidOperationException">The current token is parseable to a boolean value.</exception>
    public bool GetBoolean()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Number:
                return CurrentToken.ValueSpan is "1";

            case TokenType.Text when CurrentToken.ValueSpan is "on" or "yes" or "1":
                return true;

            case TokenType.Text when CurrentToken.ValueSpan is "off" or "no" or "0":
                return false;

            default:
                throw new InvalidOperationException("The current token is not a boolean.");
        }
    }

    /// <summary>
    ///     Reads the next token.
    /// </summary>
    /// <returns></returns>
    public Token Read()
    {
        if (IsAtEnd())
        {
            return CurrentToken = new Token();
        }

        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();

        bool isQuoted = false;
        bool hadQuote = false;

        char c;
        while ((c = ReadChar()) != 0xFFFF)
        {
            int length = builder.Length;

            switch (c)
            {
                case '"':
                    hadQuote = true;
                    isQuoted = !isQuoted;
                    break;

                case '=' when !isQuoted && builder.Length > 0:
                    builder.AsSpan().CopyTo(_buffer);
                    builder.Dispose();
                    return CurrentToken = new Token(TokenType.PropertyName, _buffer[..length]);

                case var _ when !isQuoted && char.IsWhiteSpace(c):
                case ',' or ';' when !isQuoted:
                    builder.AsSpan().CopyTo(_buffer);
                    builder.Dispose();

                    TokenType type = TokenType.Text;
                    if (hadQuote)
                    {
                        type = TokenType.String;
                    }
                    else if (double.TryParse(_buffer[..length], CultureInfo.InvariantCulture, out _))
                    {
                        type = TokenType.Number;
                    }

                    return CurrentToken = new Token(type, _buffer[..length]);

                default:
                    builder.Append(c);
                    break;
            }
        }

        if (builder.Length > 0)
        {
            int length = builder.Length;
            builder.AsSpan().CopyTo(_buffer);
            builder.Dispose();
            return CurrentToken = new Token(hadQuote ? TokenType.String : TokenType.Text, _buffer[..length]);
        }

        builder.Dispose();
        return CurrentToken = new Token();
    }

    /// <summary>
    ///     Tries to get the current token as a boolean value.
    /// </summary>
    /// <param name="value">
    ///     When this method returns, contains the current token as a boolean value if the token is parseable to a boolean
    ///     value; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the current token is parseable to a boolean value; otherwise, <see langword="false" />.
    /// </returns>
    public bool TryGetBoolean(out bool value)
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Number:
                value = CurrentToken.ValueSpan is "1";
                return true;

            case TokenType.Text when CurrentToken.ValueSpan is "on" or "yes" or "1":
                value = true;
                return true;

            case TokenType.Text when CurrentToken.ValueSpan is "off" or "no" or "0":
                value = false;
                return true;

            default:
                value = false;
                return false;
        }
    }

    private bool IsAtEnd()
    {
        if (_stream is not null)
        {
            return _stream.Position == _stream.Length;
        }

        return _position >= _bytes.Length;
    }

    private char ReadChar()
    {
        int nextByte = ReadByte();

        if (nextByte == -1)
        {
            return (char)0xFFFF;
        }

        var byteValue = (byte)nextByte;

        if (!IsMultiByteSequence(byteValue))
        {
            return (char)nextByte;
        }

        int continuationByteCount = GetContinuationByteCount(byteValue);
        Span<byte> buffer = stackalloc byte[continuationByteCount + 1];
        buffer[0] = byteValue;
        for (int index = 1; index <= continuationByteCount; index++)
        {
            nextByte = ReadByte();
            if (nextByte == -1)
            {
                return (char)0xFFFF;
            }

            buffer[index] = (byte)nextByte;
        }

        Span<char> destination = stackalloc char[1];
        Utf8.ToUtf16(buffer, destination, out _, out _);
        return destination[0];
    }

    private int PeekByte()
    {
        if (_stream is not null)
        {
            int result = _stream.ReadByte();

            if (result != -1)
            {
                _stream.Seek(-1, SeekOrigin.Current);
            }

            return result;
        }

        if (_position >= _bytes.Length)
        {
            return -1;
        }

        return _bytes[_position];
    }

    private int ReadByte()
    {
        if (_stream is not null)
        {
            return _stream.ReadByte();
        }

        if (_position >= _bytes.Length)
        {
            return -1;
        }

        return _bytes[_position++];
    }

    internal void SkipWhitespace()
    {
        int b;
        while ((b = PeekByte()) != -1)
        {
            if (!char.IsWhiteSpace((char)b))
            {
                break;
            }

            ReadByte();
        }
    }

    private static bool IsMultiByteSequence(byte value)
    {
        return (value & 0b11000000) != 0;
    }

    private static int GetContinuationByteCount(byte value)
    {
        return value switch
        {
            _ when (value & 0b11100000) == 0b11000000 => 1,
            _ when (value & 0b11110000) == 0b11100000 => 2,
            _ when (value & 0b11111000) == 0b11110000 => 3,
            _ when (value & 0b11111100) == 0b11111000 => 4,
            _ when (value & 0b11111110) == 0b11111100 => 5,
            _ when (value & 0b11111111) == 0b11111110 => 6,
            _ => 0
        };
    }
}

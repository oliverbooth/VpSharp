using System.Numerics;
using System.Text;

namespace VpSharp.Building.Serialization;

/// <summary>
///     Represents a writer that writes actions to a stream using the UTF-8 encoding.
/// </summary>
public sealed class Utf8ActionWriter
{
    private readonly Stream _stream;
    private bool _spaceNeeded = true;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Utf8ActionWriter" /> class.
    /// </summary>
    /// <param name="stream">The stream to which the action will be written.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
    public Utf8ActionWriter(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    /// <summary>
    ///     Gets or sets a value indicating whether to skip flags when writing properties.
    /// </summary>
    /// <value><see langword="true" /> to skip flags; otherwise, <see langword="false" />.</value>
    public bool SkipFlags { get; set; }

    /// <summary>
    ///     Writes a property with the specified name and value to the stream.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="value">The value of the property.</param>
    /// <exception cref="ArgumentNullException">
    ///     <para><paramref name="name" /> is <see langword="null" />.</para>
    ///     -or
    ///     <para><paramref name="value" /> is <see langword="null" />.</para>
    /// </exception>
    public void WriteProperty(string name, string value)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        WriteLeadingSpace();

        Span<byte> nameBytes = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
        Span<byte> valueBytes = stackalloc byte[Encoding.UTF8.GetByteCount(value)];

        Encoding.UTF8.GetBytes(name, nameBytes);
        Encoding.UTF8.GetBytes(value, valueBytes);

        _stream.Write(nameBytes);
        _stream.WriteByte((byte)'=');
        _stream.Write(valueBytes);

        _spaceNeeded = true;
    }

    /// <summary>
    ///     Writes a string to the stream.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public void WriteString(string value)
    {
        WriteLeadingSpace();

        Span<byte> valueBytes = stackalloc byte[value.Length + 2];
        valueBytes[0] = (byte)'"';
        valueBytes[^1] = (byte)'"';

        Encoding.UTF8.GetBytes(value, valueBytes[1..^2]);

        _spaceNeeded = true;
    }

    /// <summary>
    ///     Writes a number to the stream.
    /// </summary>
    /// <param name="value">The number to write.</param>
    /// <typeparam name="T">The type of the number.</typeparam>
    public void WriteNumber<T>(T value) where T : INumber<T>
    {
        string? valueString = value.ToString();
        if (valueString is null)
        {
            return;
        }

        WriteLeadingSpace();

        int byteCount = Encoding.UTF8.GetByteCount(valueString);
        Span<byte> bytes = stackalloc byte[byteCount];
        int bytesWritten = Encoding.UTF8.GetBytes(valueString, bytes);

        _stream.Write(bytes[..bytesWritten]);

        _spaceNeeded = true;
    }

    /// <summary>
    ///     Writes a boolean value to the stream.
    /// </summary>
    /// <param name="value">The number to write.</param>
    public void WriteBoolean(bool value)
    {
        WriteLeadingSpace();

        Span<byte> bytes = stackalloc byte[value ? 2 : 3];
        int bytesWritten = Encoding.UTF8.GetBytes(value ? "on" : "off", bytes);
        _stream.Write(bytes[..bytesWritten]);

        _spaceNeeded = true;
    }

    /// <summary>
    ///     Writes a span of characters to the stream.
    /// </summary>
    /// <param name="value">The span of characters to write.</param>
    public void Write(ReadOnlySpan<char> value)
    {
        WriteLeadingSpace();

        int byteCount = Encoding.UTF8.GetByteCount(value);
        Span<byte> bytes = stackalloc byte[byteCount];
        int bytesWritten = Encoding.UTF8.GetBytes(value, bytes);

        _stream.Write(bytes[..bytesWritten]);

        _spaceNeeded = true;
    }

    /// <summary>
    ///     Writes a value to the stream.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public void Write<T>(T? value)
    {
        if (value is null)
        {
            return;
        }

        string? valueString = value.ToString();
        if (valueString is null)
        {
            return;
        }

        WriteLeadingSpace();

        int byteCount = Encoding.UTF8.GetByteCount(valueString);
        Span<byte> bytes = stackalloc byte[byteCount];
        int bytesWritten = Encoding.UTF8.GetBytes(valueString, bytes);

        _stream.Write(bytes[..bytesWritten]);

        _spaceNeeded = true;
    }

    private void WriteLeadingSpace()
    {
        if (_spaceNeeded)
        {
            _stream.WriteByte((byte)' ');
        }

        _spaceNeeded = false;
    }
}

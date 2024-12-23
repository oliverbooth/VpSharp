using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Cysharp.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Extensions;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents a class that can serialize and deserialize action strings.
/// </summary>
public static partial class ActionSerializer
{
    /// <summary>
    ///     Deserializes an action from the specified span of characters.
    /// </summary>
    /// <param name="source">The span of characters to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(ReadOnlySpan<char> source, ActionSerializerOptions? options = null)
    {
        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        ReadOnlyCollection<LexingToken> tokens = Parse(source);
        return Lex(tokens, options);
    }

    /// <summary>
    ///     Deserializes an action from the specified string.
    /// </summary>
    /// <param name="text">The string to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(string text, ActionSerializerOptions? options = null)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        return Deserialize(text.AsSpan(), options);
    }

    /// <summary>
    ///     Deserializes an action from a <see cref="Stream" />.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="stream" /> is not readable.</exception>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(Stream stream, ActionSerializerOptions? options = null)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream does not support reading.", nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new ArgumentException("Stream does not support seeking.", nameof(stream));
        }

        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return Deserialize(reader, options);
    }

    /// <summary>
    ///     Deserializes an action from a <see cref="TextReader" />.
    /// </summary>
    /// <param name="reader">The text reader.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(TextReader reader, ActionSerializerOptions? options = null)
    {
        if (reader is null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        var tokens = new List<LexingToken>();
        Span<char> buffer = stackalloc char[256];

        while (reader.Peek() >= 0)
        {
            int read = reader.Read(buffer);
            if (read <= 0)
            {
                break;
            }

            ReadOnlySpan<char> span = buffer[..read];
            tokens.AddRange(Parse(span));
        }

        return Lex(tokens, options);
    }

    private static void AppendBuffer(ref Utf16ValueStringBuilder builder, ref bool isProperty, List<LexingToken> tokens)
    {
        if (builder.Length <= 0)
        {
            isProperty = false;
            return;
        }

        ReadOnlySpan<char> span = builder.AsSpan();
        LexingTokenType tokenType;

        if (isProperty)
        {
            tokenType = LexingTokenType.Property;
        }
        else if (double.TryParse(span, NumberFormatInfo.InvariantInfo, out _))
        {
            tokenType = LexingTokenType.Number;
        }
        else
        {
            tokenType = LexingTokenType.Text;
        }

        tokens.Add(new LexingToken(tokenType, span));
        builder.Clear();
        isProperty = false;
    }

    private static VirtualParadiseCommand? FindCommand(string tokenValue, IReadOnlyCollection<Type> commandTypes)
    {
        foreach (Type type in commandTypes)
        {
            if (type.GetCommandName().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseCommand;
            }
        }

        return null;
    }

    private static VirtualParadiseTrigger? FindTrigger(string tokenValue, IReadOnlyCollection<Type> triggerTypes)
    {
        foreach (Type type in triggerTypes)
        {
            if (type.GetTriggerName().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseTrigger;
            }
        }

        return null;
    }
}

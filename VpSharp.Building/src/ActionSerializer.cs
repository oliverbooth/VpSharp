using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using Cysharp.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Extensions;
using VpSharp.Building.Triggers;
using X10D.Reflection;

namespace VpSharp.Building;

/// <summary>
///     Represents a class that can serialize and deserialize action strings.
/// </summary>
public static class ActionSerializer
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

        ReadOnlyCollection<Token> tokens = Parse(source);
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

        using var reader = new StringReader(text);
        return Deserialize(reader, options);
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

        ReadOnlyCollection<Token> tokens = Parse(reader);
        return Lex(tokens, options);
    }

    private static void AppendBuffer(ref Utf16ValueStringBuilder builder, ref bool isProperty, List<Token> tokens)
    {
        if (builder.Length <= 0)
        {
            isProperty = false;
            return;
        }

        ReadOnlySpan<char> span = builder.AsSpan();
        TokenType tokenType;

        if (isProperty)
        {
            tokenType = TokenType.Property;
        }
        else if (double.TryParse(span, NumberFormatInfo.InvariantInfo, out _))
        {
            tokenType = TokenType.Number;
        }
        else
        {
            tokenType = TokenType.String;
        }

        tokens.Add(new Token(tokenType, span));
        builder.Clear();
        isProperty = false;
    }

    private static VirtualParadiseAction Lex(ReadOnlyCollection<Token> tokens, ActionSerializerOptions options)
    {
        var action = new VirtualParadiseAction();
        VirtualParadiseTrigger? currentTrigger = null;
        VirtualParadiseCommand? currentCommand = null;

        foreach (Token token in tokens)
        {
            HandleToken(action, token, ref currentTrigger, ref currentCommand, options);
        }

        ConvertCommand(action, options);
        return action;
    }

    private static void ConvertCommand(VirtualParadiseAction action, ActionSerializerOptions options)
    {
        foreach (VirtualParadiseCommand command in action.Triggers.SelectMany(t => t.Commands))
        {
            var type = command.GetType();
            var attribute = type.GetCustomAttribute<CommandAttribute>();
            if (attribute?.ConverterType is not { } converterType)
            {
                continue;
            }

            if (Activator.CreateInstance(converterType) is not CommandConverter converter || !converter.CanConvert(type))
            {
                continue;
            }

            string rawArguments = string.Join(' ', command.RawArguments);
            string arguments = rawArguments;
            int index = rawArguments.IndexOf('=');
            int spaceIndex = 0;
            if (index != -1)
            {
                spaceIndex = rawArguments.AsSpan()[..index].LastIndexOf(' ');
                arguments = rawArguments[..spaceIndex];
            }

            using (var reader = new StringReader(arguments))
            {
                reader.SkipWhitespace();
                converter.Read(reader, type, command, options);

                if (index == -1)
                {
                    continue;
                }
            }

            using (var reader = new StringReader(rawArguments[(spaceIndex + 1)..]))
            {
                reader.SkipWhitespace();
                converter.ReadProperties(reader, command, options);
            }
        }
    }

    private static void HandleToken(VirtualParadiseAction action,
        Token token,
        ref VirtualParadiseTrigger? currentTrigger,
        ref VirtualParadiseCommand? currentCommand,
        ActionSerializerOptions options)
    {
        switch (token.Type)
        {
            case var _ when currentTrigger is null:
                currentTrigger = FindTrigger(token.Value, options.TriggerTypes);
                if (currentTrigger is not null)
                {
                    action.Triggers = [..action.Triggers, currentTrigger];
                }

                break;

            case var _ when currentCommand is null:
                currentCommand = FindCommand(token.Value, options.CommandTypes);
                if (currentCommand is not null)
                {
                    currentTrigger.Commands = [..currentTrigger.Commands, currentCommand];
                }

                break;

            case TokenType.String or TokenType.Number or TokenType.Property:
                currentCommand.RawArguments = [..currentCommand.RawArguments, token.Value];
                break;

            case TokenType.Operator when token.Value == ",":
                currentCommand = null;
                break;

            case TokenType.Operator when token.Value == ";":
                currentTrigger = null;
                currentCommand = null;
                break;

            case TokenType.Eof:
                currentTrigger = null;
                currentCommand = null;
                break;
        }
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

    private static ReadOnlyCollection<Token> Parse(TextReader reader)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var tokens = new List<Token>();
        bool isProperty = false;

        while (reader.Peek() != -1)
        {
            var character = (char)reader.Read();

            switch (character)
            {
                case ',':
                case ';':
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new Token(TokenType.Operator, [character]));
                    break;

                case var _ when char.IsWhiteSpace(character):
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    break;

                case '=':
                    isProperty = true;
                    goto default;

                default:
                    builder.Append(character);
                    break;
            }
        }

        AppendBuffer(ref builder, ref isProperty, tokens);
        tokens.Add(new Token(TokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }

    private static ReadOnlyCollection<Token> Parse(ReadOnlySpan<char> source)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var tokens = new List<Token>();
        bool isProperty = false;

        foreach (char character in source)
        {
            switch (character)
            {
                case ',':
                case ';':
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new Token(TokenType.Operator, [character]));
                    break;

                case var _ when char.IsWhiteSpace(character):
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    break;

                case '=':
                    isProperty = true;
                    goto default;

                default:
                    builder.Append(character);
                    break;
            }
        }

        AppendBuffer(ref builder, ref isProperty, tokens);
        tokens.Add(new Token(TokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }
}

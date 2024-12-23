using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using Cysharp.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

public static partial class ActionSerializer
{
    private static VirtualParadiseAction Lex(IEnumerable<LexingToken> tokens, ActionSerializerOptions options)
    {
        var action = new VirtualParadiseAction();
        VirtualParadiseTrigger? currentTrigger = null;
        VirtualParadiseCommand? currentCommand = null;

        foreach (LexingToken token in tokens)
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

            ConvertCommand(command, converter, type, options);
        }
    }

    private static void ConvertCommand(VirtualParadiseCommand command,
        CommandConverter converter,
        Type type,
        ActionSerializerOptions options)
    {
        IList<string> rawArguments = command.RawArguments;

        Span<char> arguments = stackalloc char[rawArguments.Sum(arg => arg.Length) + rawArguments.Count];
        int offset = 0;
        foreach (ReadOnlySpan<char> argument in rawArguments)
        {
            argument.CopyTo(arguments[offset..]);
            offset += argument.Length;
            arguments[offset++] = ' ';
        }

        int byteCount = Encoding.UTF8.GetByteCount(arguments);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.UTF8.GetBytes(arguments, bytes);

        var reader = new Utf8ActionReader(bytes);
        reader.SkipWhitespace();
        converter.Read(ref reader, type, command, options);
    }

    private static void HandleToken(VirtualParadiseAction action,
        LexingToken token,
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

            case LexingTokenType.Text or LexingTokenType.Number or LexingTokenType.Property:
                currentCommand.RawArguments = [..currentCommand.RawArguments, token.Value];
                break;

            case LexingTokenType.Operator when token.Value == ",":
                currentCommand = null;
                break;

            case LexingTokenType.Operator when token.Value == ";":
                currentTrigger = null;
                currentCommand = null;
                break;

            case LexingTokenType.Eof:
                currentTrigger = null;
                currentCommand = null;
                break;
        }
    }

    private static ReadOnlyCollection<LexingToken> Parse(TextReader reader)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var tokens = new List<LexingToken>();
        bool isProperty = false;
        bool isQuoted = false;

        while (reader.Peek() != -1)
        {
            var character = (char)reader.Read();

            switch (character)
            {
                case '"':
                    isQuoted = !isQuoted;
                    break;

                case ',' when !isQuoted:
                case ';' when !isQuoted:
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new LexingToken(LexingTokenType.Operator, [character]));
                    break;

                case var _ when !isQuoted && char.IsWhiteSpace(character):
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    break;

                case '=' when !isQuoted:
                    isProperty = true;
                    goto default;

                default:
                    builder.Append(character);
                    break;
            }
        }

        AppendBuffer(ref builder, ref isProperty, tokens);
        tokens.Add(new LexingToken(LexingTokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }

    private static ReadOnlyCollection<LexingToken> Parse(ReadOnlySpan<char> source)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var tokens = new List<LexingToken>();
        bool isProperty = false;
        bool isQuoted = false;

        foreach (char character in source)
        {
            switch (character)
            {
                case '"':
                    isQuoted = !isQuoted;
                    break;

                case ',' when !isQuoted:
                case ';' when !isQuoted:
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new LexingToken(LexingTokenType.Operator, [character]));
                    break;

                case var _ when !isQuoted && char.IsWhiteSpace(character):
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    break;

                case '=' when !isQuoted:
                    isProperty = true;
                    goto default;

                default:
                    builder.Append(character);
                    break;
            }
        }

        AppendBuffer(ref builder, ref isProperty, tokens);
        tokens.Add(new LexingToken(LexingTokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }
}

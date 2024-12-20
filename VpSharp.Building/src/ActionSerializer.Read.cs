using System.Collections.ObjectModel;
using System.Reflection;
using Cysharp.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

public static partial class ActionSerializer
{
    private static VirtualParadiseAction Lex(ReadOnlyCollection<LexingToken> tokens, ActionSerializerOptions options)
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
        Span<char> rawArguments = stackalloc char[command.RawArguments.Sum(arg => arg.Length) + command.RawArguments.Count];
        int offset = 0;
        foreach (ReadOnlySpan<char> argument in command.RawArguments)
        {
            argument.CopyTo(rawArguments[offset..]);
            offset += argument.Length;
            rawArguments[offset++] = ' ';
        }

        ReadOnlySpan<char> arguments = rawArguments;
        int index = rawArguments.IndexOf('=');
        int spaceIndex = 0;
        if (index != -1)
        {
            spaceIndex = rawArguments[..index].LastIndexOf(' ');
            arguments = rawArguments[..spaceIndex];
        }

        var reader = new Utf16ValueStringReader(arguments);
        reader.SkipWhitespace();
        converter.Read(ref reader, type, command, options);

        if (index == -1)
        {
            return;
        }

        reader = new Utf16ValueStringReader(rawArguments[(spaceIndex + 1)..]);
        reader.SkipWhitespace();
        ReadProperties(ref reader, command);
    }

    private static void ReadProperties(ref Utf16ValueStringReader reader, VirtualParadiseCommand command)
    {
        Span<char> propertyName = stackalloc char[50];
        int propertyNameLength = 0;
        using Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();

        int c;
        while ((c = reader.Read()) != -1)
        {
            char character = (char)c;
            switch (character)
            {
                case var _ when char.IsWhiteSpace(character):
                    // populate property
                    SetProperty(command, propertyName[..propertyNameLength], builder.AsSpan());
                    builder.Clear();
                    break;

                case '=':
                    builder.AsSpan().CopyTo(propertyName);
                    propertyNameLength = builder.Length;
                    builder.Clear();
                    break;

                default:
                    builder.Append(character);
                    break;
            }
        }
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

            case LexingTokenType.String or LexingTokenType.Number or LexingTokenType.Property:
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

        while (reader.Peek() != -1)
        {
            var character = (char)reader.Read();

            switch (character)
            {
                case ',':
                case ';':
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new LexingToken(LexingTokenType.Operator, [character]));
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
        tokens.Add(new LexingToken(LexingTokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }

    private static ReadOnlyCollection<LexingToken> Parse(ReadOnlySpan<char> source)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var tokens = new List<LexingToken>();
        bool isProperty = false;

        foreach (char character in source)
        {
            switch (character)
            {
                case ',':
                case ';':
                    AppendBuffer(ref builder, ref isProperty, tokens);
                    tokens.Add(new LexingToken(LexingTokenType.Operator, [character]));
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
        tokens.Add(new LexingToken(LexingTokenType.Eof, ReadOnlySpan<char>.Empty));

        builder.Dispose();
        return tokens.AsReadOnly();
    }
}

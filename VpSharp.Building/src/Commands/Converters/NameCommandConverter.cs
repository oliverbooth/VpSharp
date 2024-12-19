using VpSharp.Building.Extensions;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="NameCommand" /> command.
/// </summary>
public sealed class NameCommandConverter : CommandConverter<NameCommand>
{
    /// <inheritdoc />
    public override void Read(TextReader reader, NameCommand command, ActionSerializerOptions options)
    {
        string? token = reader.ReadToken();
        if (token is null)
        {
            return;
        }

        command.Name = token;
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, NameCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Name);
    }
}

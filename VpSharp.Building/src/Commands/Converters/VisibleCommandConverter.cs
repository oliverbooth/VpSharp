using VpSharp.Building.Extensions;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="VisibleCommand" /> command.
/// </summary>
public sealed class VisibleCommandConverter : CommandConverter<VisibleCommand>
{
    /// <inheritdoc />
    public override void Read(TextReader reader, VisibleCommand command, ActionSerializerOptions options)
    {
        string? token = reader.ReadToken();
        if (token is null)
        {
            return;
        }

        command.TargetName = token;
        token = reader.ReadToken();

        if (token is null)
        {
            switch (command.TargetName)
            {
                case "on" or "yes" or "1":
                    command.IsVisible = true;
                    command.TargetName = null;
                    break;

                case "off" or "no" or "0":
                    command.IsVisible = false;
                    command.TargetName = null;
                    break;
            }
        }
        else
        {
            command.IsVisible = token switch
            {
                "on" or "yes" or "1" => true,
                "off" or "no" or "0" => false,
                _ => command.IsVisible
            };
        }
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, VisibleCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.GetCommandName());
        writer.Write(' ');
        writer.Write(command.IsVisible);
        WriteProperties(writer, command, options);
    }
}

using VpSharp.Building.Extensions;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="SolidCommand" /> command.
/// </summary>
public sealed class SolidCommandConverter : CommandConverter<SolidCommand>
{
    /// <inheritdoc />
    public override void Read(TextReader reader, SolidCommand command, ActionSerializerOptions options)
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
                    command.IsSolid = true;
                    command.TargetName = null;
                    break;

                case "off" or "no" or "0":
                    command.IsSolid = false;
                    command.TargetName = null;
                    break;
            }
        }
        else
        {
            command.IsSolid = token switch
            {
                "on" or "yes" or "1" => true,
                "off" or "no" or "0" => false,
                _ => command.IsSolid
            };
        }
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, SolidCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.IsSolid ? "on" : "off");
    }
}

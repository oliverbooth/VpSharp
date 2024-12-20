namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="SolidCommand" /> command.
/// </summary>
public sealed class SolidCommandConverter : CommandConverter<SolidCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, SolidCommand command, ActionSerializerOptions options)
    {
        Span<char> token = stackalloc char[50];
        int read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            return;
        }

        command.ExecuteAs = token.ToString();
        token.Clear();
        read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            switch (command.ExecuteAs)
            {
                case "on" or "yes" or "1":
                    command.IsSolid = true;
                    command.ExecuteAs = null;
                    break;

                case "off" or "no" or "0":
                    command.IsSolid = false;
                    command.ExecuteAs = null;
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

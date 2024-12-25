namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for <see cref="AnimateCommand" />.
/// </summary>
public sealed class AnimateCommandConverter : CommandConverter<AnimateCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, AnimateCommand command, ActionSerializerOptions options)
    {
        int argumentIndex;

        if (command.RawArguments[0].Equals("mask", StringComparison.OrdinalIgnoreCase))
        {
            command.IsMask = true;
            command.Name = command.RawArguments[1];
            argumentIndex = 2;
        }
        else
        {
            command.Name = command.RawArguments[0];
            argumentIndex = 1;
        }

        command.Animation = command.RawArguments[argumentIndex++];
        command.ImageCount = int.Parse(command.RawArguments[argumentIndex++]);
        command.FrameCount = int.Parse(command.RawArguments[argumentIndex++]);
        command.FrameDelay = TimeSpan.FromMilliseconds(int.Parse(command.RawArguments[argumentIndex++]));

        var frameList = new List<int>();

        for (var index = argumentIndex; index < command.RawArguments.Count; index++)
        {
            frameList.Add(int.Parse(command.RawArguments[index]));
        }

        command.FrameList = frameList.AsReadOnly();
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, AnimateCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (command.IsMask)
        {
            writer.Write("mask");
        }

        writer.Write(command.Name);
        writer.Write(command.Animation);
        writer.Write(command.ImageCount);
        writer.Write(command.FrameCount);
        writer.Write((int)command.FrameDelay.TotalMilliseconds);

        foreach (int frame in command.FrameList)
        {
            writer.Write(frame);
        }
    }
}

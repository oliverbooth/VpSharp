using VpSharp.Commands;
using VpSharp.Commands.Attributes;

namespace VpSharp.CSharp_Sample;

public class SayCommand : CommandModule
{
    [Command("say")]
    public Task SayAsync(CommandContext context, [Remainder] string message)
    {
        context.Respond(message);
        return Task.CompletedTask;
    }
}

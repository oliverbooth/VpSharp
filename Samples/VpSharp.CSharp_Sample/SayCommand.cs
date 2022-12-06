using VpSharp.Commands;
using VpSharp.Commands.Attributes;

namespace VpSharp.CSharp_Sample;

public class SayCommand : CommandModule
{
    [Command("say")]
    public async Task SayAsync(CommandContext context, [Remainder] string message)
    {
        await context.RespondAsync(message);
    }
}

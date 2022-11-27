using VpSharp.Commands;
using VpSharp.Commands.Attributes;

namespace VpSharp.IntegrationTests.CommandModules;

internal sealed class TestCommands : CommandModule
{
    [Command("echo")]
    [Aliases("say")]
    public async Task EchoCommand(CommandContext context, [Remainder] string message)
    {
        await context.RespondAsync(message);
    }

    [Command("ping")]
    [Aliases("pong", "pingpong")]
    public async Task PingAsync(CommandContext context)
    {
        await context.RespondAsync("Pong!");
    }
}

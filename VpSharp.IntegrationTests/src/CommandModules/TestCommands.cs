using VpSharp.Commands;
using VpSharp.Commands.Attributes;

namespace VpSharp.IntegrationTests.CommandModules;

internal sealed class TestCommands : CommandModule
{
    [Command("echo")]
    [Aliases("say")]
    public Task EchoCommand(CommandContext context, [Remainder] string message)
    {
        context.Respond(message);
        return Task.CompletedTask;
    }

    [Command("ping")]
    [Aliases("pong", "pingpong")]
    public Task PingAsync(CommandContext context)
    {
        context.Respond("Pong!");
        return Task.CompletedTask;
    }
}

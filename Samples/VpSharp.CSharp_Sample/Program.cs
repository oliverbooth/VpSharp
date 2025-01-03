using VpSharp.Commands;

namespace VpSharp.CSharp_Sample;

internal static class Program
{
    private static VirtualParadiseClient s_client = null!;

    private static async Task Main()
    {
        var configuration = new VirtualParadiseConfiguration
        {
            Username = "YOUR_VP_USERNAME",
            Password = "YOUR_VP_PASSWORD",
            BotName = "Greeter",
            Application = new Application("GreeterBot", "1.0")
        };

        s_client = new VirtualParadiseClient(configuration);
        var commands = s_client.UseCommands(new CommandsExtensionConfiguration { Prefixes = ["/"] });
        commands.RegisterCommands<SayCommand>();

        s_client.AvatarJoined.Subscribe(avatar => s_client.SendMessage($"Hello, {avatar.Name}!"));

        await s_client.ConnectAsync();
        await s_client.LoginAsync();
        await s_client.EnterAsync("WORLD_NAME");
        await s_client.CurrentAvatar!.TeleportAsync(Vector3d.Zero);

        await Task.Delay(-1);
    }
}

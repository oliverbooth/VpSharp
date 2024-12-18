using VpSharp;
using VpSharp.Commands;
using VpSharp.Entities;
using VpSharp.IntegrationTests.CommandModules;

string? username = Environment.GetEnvironmentVariable("username");
string? password = Environment.GetEnvironmentVariable("password");

if (string.IsNullOrWhiteSpace(username))
{
    Console.Error.WriteLine("username env variable cannot be empty");
    return;
}

if (string.IsNullOrWhiteSpace(password))
{
    Console.Error.WriteLine("password env variable cannot be empty");
    return;
}

var configuration = new VirtualParadiseConfiguration
{
    Username = username,
    Password = password,
    Application = new Application("VpSharp.IntegrationTests", "1.0.0"),
    AutoQuery = false,
    BotName = "TestBot"
};

using var client = new VirtualParadiseClient(configuration);
CommandsExtension commands = client.UseCommands(new CommandsExtensionConfiguration { Prefixes = ["!"] });
commands.RegisterCommands<TestCommands>();

Console.WriteLine(@"Connecting to universe");
await client.ConnectAsync().ConfigureAwait(false);

Console.WriteLine(@"Logging in");
await client.LoginAsync();

Console.WriteLine(@"Entering world");
VirtualParadiseWorld world = await client.EnterAsync(Environment.GetEnvironmentVariable("world") ?? "Blizzard");
Console.WriteLine(@"Entered world!");

VirtualParadiseAvatar avatar = client.CurrentAvatar!;

Console.WriteLine($@"My name is {avatar.Name} and I am at {avatar.Location}");
Console.WriteLine($@"Entered {world.Name} with size {world.Size}");

await Task.Delay(-1);

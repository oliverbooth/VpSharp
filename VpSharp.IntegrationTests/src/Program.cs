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
await client.ConnectAsync();

Console.WriteLine(@"Logging in");
await client.LoginAsync();

Console.WriteLine(@"Entering world");
VirtualParadiseWorld world = await client.EnterAsync(Environment.GetEnvironmentVariable("world") ?? "Blizzard");
Console.WriteLine(@"Entered world!");

VirtualParadiseAvatar avatar = client.CurrentAvatar!;

Console.WriteLine($@"My name is {avatar.Name} and I am at {avatar.Location}");
Console.WriteLine($@"Entered {world.Name} with size {world.Size}");

var user = await client.GetUserAsync(11);
Console.WriteLine(user is null);
Console.WriteLine(user is not null ? $"User 11 is {user.Name}" : "User 11 does not exist");

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
user = await client.GetUserAsync(9999, cts.Token);
Console.WriteLine(user is null);
Console.WriteLine(user is not null ? $"User 9999 is {user.Name}" : "User 9999 does not exist");

await Task.Delay(-1);

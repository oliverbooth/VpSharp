Imports VpSharp.Commands
Imports VpSharp.EventData
Imports VpSharp.Extensions

Module Program
    Private WithEvents _client As VirtualParadiseClient

    Public Sub Main(args As String())
        MainAsync().GetAwaiter().GetResult()
    End Sub

    Private Async Function MainAsync() As Task
        Dim configuration = new VirtualParadiseConfiguration _
                With {
                .Username = "YOUR_VP_USERNAME",
                .Password = "YOUR_VP_PASSWORD",
                .BotName = "Greeter",
                .Application = New Application("GreeterBot", "1.0")
                }

        _client = New VirtualParadiseClient(configuration)
        
        _client.AvatarJoined.SubscribeAsync(Async Function(avatar) Await _client.SendMessageAsync($"Hello, {avatar.Name}"))
        
        Dim commands = _client.UseCommands(New CommandsExtensionConfiguration With { .Prefixes = {"/"} })
        commands.RegisterCommands(GetType(SayCommand))

        Await _client.ConnectAsync()
        Await _client.LoginAsync()
        Await _client.EnterAsync("WORLD_NAME")
        Await _client.CurrentAvatar.TeleportAsync(Vector3d.Zero)

        Await Task.Delay(- 1)
    End Function
End Module

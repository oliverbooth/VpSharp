Imports VpSharp.Commands
Imports VpSharp.Commands.Attributes

Public Class SayCommand
    Inherits CommandModule

    <Command("say")>
    Public Function SayAsync(context as CommandContext, <Remainder> message As String) As Task
        context.Respond(message)
        Return Task.CompletedTask
    End Function
End Class
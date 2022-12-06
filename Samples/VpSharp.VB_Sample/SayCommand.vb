Imports VpSharp.Commands
Imports VpSharp.Commands.Attributes

Public Class SayCommand
    Inherits CommandModule
    
    <Command("say")>
    Public Async Function SayAsync(context as CommandContext, <Remainder> message As String) As Task
        Await context.RespondAsync(message)
    End Function
    
End Class
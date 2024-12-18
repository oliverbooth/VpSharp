using System.Diagnostics;
using VpSharp.Building.Extensions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace VpSharp.Building.Tests;

public class ActionSerializerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Deserialize_ShouldCreateObjects_GivenString()
    {
        string source = "create name sourceObj, solid off; activate solid on, move 0 9 0 name=targetObj; bump visible off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        foreach (VirtualParadiseTrigger trigger in action.Triggers)
        {
            Trace.WriteLine(trigger.GetTriggerName());
            foreach (VirtualParadiseCommand command in trigger.Commands)
            {
                Trace.WriteLine($"\u251c\u2500 {command.GetCommandName()}");
                foreach (string argument in command.RawArguments)
                {
                    Trace.WriteLine($"\u2502  \u251c\u2500 {argument}");
                }
            }
        }

        Assert.Pass();
    }
}

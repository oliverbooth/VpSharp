using System.Diagnostics;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests;

internal sealed class ActionSerializerTests
{
    [Test]
    public void Serialize_ShouldProduceCommand_GivenInput()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true, Radius = 10.0 },
                        new TextureCommand { Texture = "stone1" }
                    ]
                },
                new ActivateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false, IsGlobal = true }
                    ]
                },
            ]
        };

        string result = ActionSerializer.Serialize(action);
        Trace.WriteLine(result);
    }
}

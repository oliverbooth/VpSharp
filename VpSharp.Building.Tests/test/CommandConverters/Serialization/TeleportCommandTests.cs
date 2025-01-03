using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class TeleportCommandTests
{
    [Test]
    public void ActivateTeleport_ShouldSerializeToDefaultCoordinates_GivenGroundZero()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new TeleportCommand()
                    ]
                }
            ]
        };

        const string expected = "activate teleport 0.00n 0.00w 0.00a 0.00";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateTeleport_ShouldSerializeToCoordinates_GivenCoordinates()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new TeleportCommand { Coordinates = new Coordinates(-150, 5, 120, 90) }
                    ]
                }
            ]
        };

        const string expected = "activate teleport 120.00n 150.00e 5.00a 90.00";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

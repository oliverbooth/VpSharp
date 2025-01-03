using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class TeleportXyzCommandTests
{
    [Test]
    public void ActivateTeleportXyz_ShouldSerializeToDestination_GivenDestination()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new TeleportXyzCommand { Destination = new Vector3d(0, 12, 0) }
                    ]
                }
            ]
        };

        const string expected = "activate teleportxyz 0 12 0";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateTeleportXyz_ShouldSerializeToDestinationAndYaw_GivenDestinationAndYaw()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new TeleportXyzCommand { Destination = new Vector3d(0, 12, 0), Yaw = 90 }
                    ]
                }
            ]
        };

        const string expected = "activate teleportxyz 0 12 0 90";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

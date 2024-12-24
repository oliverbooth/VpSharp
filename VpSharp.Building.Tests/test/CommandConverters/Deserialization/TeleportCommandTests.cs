using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class TeleportCommandTests
{
    [Test]
    public void ActivateTeleport_ShouldDeserializeToDefaultCoordinates_GivenGroundZero()
    {
        const string source = "activate teleport 0n 0e 0a 0";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<TeleportCommand>());

            var command = (TeleportCommand)action.Activate.Commands[0];
            Assert.That(command.Coordinates.X, Is.EqualTo(0));
            Assert.That(command.Coordinates.Y, Is.EqualTo(0));
            Assert.That(command.Coordinates.Z, Is.EqualTo(0));
            Assert.That(command.Coordinates.Yaw, Is.EqualTo(0));
            Assert.That(command.Coordinates.ToString(), Is.EqualTo("0.00n 0.00w 0.00a 0.00"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void ActivateTeleport_ShouldDeserializeToCoordinates_GivenCoordinates()
    {
        const string source = "activate teleport 120n 150e 5a 90";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<TeleportCommand>());

            var command = (TeleportCommand)action.Activate.Commands[0];
            Assert.That(command.Coordinates, Is.EqualTo(new Coordinates(-150, 5, 120, 90)));
            Assert.That(command.Coordinates.X, Is.EqualTo(-150));
            Assert.That(command.Coordinates.Y, Is.EqualTo(5));
            Assert.That(command.Coordinates.Z, Is.EqualTo(120));
            Assert.That(command.Coordinates.Yaw, Is.EqualTo(90));
            Assert.That(command.Coordinates.ToString(), Is.EqualTo("120.00n 150.00e 5.00a 90.00"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

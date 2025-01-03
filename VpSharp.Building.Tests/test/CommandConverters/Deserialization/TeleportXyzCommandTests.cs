using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class TeleportXyzCommandTests
{
    [Test]
    public void ActivateTeleportXyz_ShouldDeserializeToDestination_GivenDestination()
    {
        const string source = "activate teleportxyz 0 12 0";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<TeleportXyzCommand>());

            var command = (TeleportXyzCommand)action.Activate.Commands[0];
            Assert.That(command.Destination, Is.EqualTo(new Vector3d(0, 12, 0)));
            Assert.That(command.Yaw, Is.EqualTo(0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void ActivateTeleportXyz_ShouldDeserializeToDestinationAndYaw_GivenDestinationAndYaw()
    {
        const string source = "activate teleportxyz 0 12 0 90";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<TeleportXyzCommand>());

            var command = (TeleportXyzCommand)action.Activate.Commands[0];
            Assert.That(command.Destination, Is.EqualTo(new Vector3d(0, 12, 0)));
            Assert.That(command.Yaw, Is.EqualTo(90));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

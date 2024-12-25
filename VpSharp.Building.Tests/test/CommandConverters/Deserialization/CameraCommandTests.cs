using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class CameraCommandTests
{
    [Test]
    public void CreateCamera_ShouldDeserialize_GivenTarget()
    {
        const string source = "create camera target=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<CameraCommand>());

            var command = (CameraCommand)action.Create.Commands[0];
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.Location, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateCamera_ShouldDeserialize_GivenLocation()
    {
        const string source = "create camera location=bar";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<CameraCommand>());

            var command = (CameraCommand)action.Create.Commands[0];
            Assert.That(command.Target, Is.Null);
            Assert.That(command.Location, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateCamera_ShouldDeserialize_GivenTargetAndLocation()
    {
        const string source = "create camera location=bar target=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<CameraCommand>());

            var command = (CameraCommand)action.Create.Commands[0];
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.Location, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class FramerateCommandTests
{
    [Test]
    public void CreateFramerateShouldDeserialize_CreateFramerate60()
    {
        const string source = "create framerate 60";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<FramerateCommand>());

            var command = (FramerateCommand)action.Create.Commands[0];
            Assert.That(command.Framerate, Is.EqualTo(60));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateFramerateShouldDeserialize_CreateFramerate30()
    {
        const string source = "create framerate 30";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<FramerateCommand>());

            var command = (FramerateCommand)action.Create.Commands[0];
            Assert.That(command.Framerate, Is.EqualTo(30));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateFramerateShouldDeserialize_CreateFramerate60_WithNameProperty()
    {
        const string source = "create framerate 60 name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<FramerateCommand>());

            var command = (FramerateCommand)action.Create.Commands[0];
            Assert.That(command.Framerate, Is.EqualTo(60));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }

    [Test]
    public void CreateFramerateShouldDeserialize_CreateFramerate30_WithNameProperty()
    {
        const string source = "create framerate 30 name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<FramerateCommand>());

            var command = (FramerateCommand)action.Create.Commands[0];
            Assert.That(command.Framerate, Is.EqualTo(30));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }
}

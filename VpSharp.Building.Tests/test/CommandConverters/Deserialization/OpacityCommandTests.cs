using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class OpacityCommandTests
{
    [Test]
    public void CreateOpacity_ShouldDeserialize_CreateOpacity80Percent()
    {
        const string source = "create opacity 0.8";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<OpacityCommand>());

            var command = (OpacityCommand)action.Create.Commands[0];
            Assert.That(command.Opacity, Is.EqualTo(0.8));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateOpacity_ShouldDeserialize_CreateOpacity20Percent()
    {
        const string source = "create opacity 0.2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<OpacityCommand>());

            var command = (OpacityCommand)action.Create.Commands[0];
            Assert.That(command.Opacity, Is.EqualTo(0.2));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateOpacity_ShouldDeserialize_CreateOpacity80Percent_WithNameProperty()
    {
        const string source = "create opacity 0.8 name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<OpacityCommand>());

            var command = (OpacityCommand)action.Create.Commands[0];
            Assert.That(command.Opacity, Is.EqualTo(0.8));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }

    [Test]
    public void CreateOpacity_ShouldDeserialize_CreateOpacity20Percent_WithNameProperty()
    {
        const string source = "create opacity 0.2 name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<OpacityCommand>());

            var command = (OpacityCommand)action.Create.Commands[0];
            Assert.That(command.Opacity, Is.EqualTo(0.2));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }
}

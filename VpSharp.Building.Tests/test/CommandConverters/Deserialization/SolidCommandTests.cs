using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class SolidCommandTests
{
    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOff()
    {
        const string source = "create solid off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn()
    {
        const string source = "create solid on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOff_WithNameArgument()
    {
        const string source = "create solid foo off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOff_WithNameProperty()
    {
        const string source = "create solid off name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn_WithNameArgument()
    {
        const string source = "create solid foo on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn_WithNameProperty()
    {
        const string source = "create solid on name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.Some("foo")));
        });
    }
}

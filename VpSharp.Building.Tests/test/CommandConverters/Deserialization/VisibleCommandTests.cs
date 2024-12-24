using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class VisibleCommandTests
{
    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleOff()
    {
        const string source = "create visible off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleOn()
    {
        const string source = "create visible on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleFooOff()
    {
        const string source = "create visible foo off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleOff_WithNameProperty()
    {
        const string source = "create visible off name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleFooOn()
    {
        const string source = "create visible foo on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.Target, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateVisible_ShouldDeserialize_CreateVisibleOn_WithNameProperty()
    {
        const string source = "create visible on name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.Target, Is.Null);
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }
}

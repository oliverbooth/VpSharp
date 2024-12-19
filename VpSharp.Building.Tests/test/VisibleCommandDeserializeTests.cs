using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests;

internal sealed class VisibleCommandDeserializeTests
{
    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOff()
    {
        const string source = "create visible off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOn()
    {
        const string source = "create visible on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOff_WithTargetNameAsArgument()
    {
        const string source = "create visible foo off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOff_WithTargetNameAsProperty()
    {
        const string source = "create visible off name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.False);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOn_WithTargetNameAsArgument()
    {
        const string source = "create visible foo on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateVisibleOn_WithTargetNameAsProperty()
    {
        const string source = "create visible on name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<VisibleCommand>());

            var command = (VisibleCommand)action.Create.Commands[0];
            Assert.That(command.IsVisible, Is.True);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }
}

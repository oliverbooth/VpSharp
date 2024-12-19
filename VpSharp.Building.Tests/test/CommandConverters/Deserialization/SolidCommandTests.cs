using VpSharp.Building.Commands;
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
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn()
    {
        const string source = "create solid on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOff_WithTargetNameAsArgument()
    {
        const string source = "create solid foo off";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOff_WithTargetNameAsProperty()
    {
        const string source = "create solid off name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.False);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn_WithTargetNameAsArgument()
    {
        const string source = "create solid foo on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateSolidOn_WithTargetNameAsProperty()
    {
        const string source = "create solid on name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SolidCommand>());

            var command = (SolidCommand)action.Create.Commands[0];
            Assert.That(command.IsSolid, Is.True);
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }
}

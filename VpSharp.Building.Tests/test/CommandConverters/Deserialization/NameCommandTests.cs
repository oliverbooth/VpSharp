using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class NameCommandTests
{
    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameFoo()
    {
        const string source = "create name foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameBar()
    {
        const string source = "create name bar";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.TargetName, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameFoo_WithTargetNameAsProperty()
    {
        const string source = "create name foo name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameBar_WithTargetNameAsProperty()
    {
        const string source = "create name bar name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Is.Not.Empty);
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Is.Not.Empty);
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.TargetName, Is.EqualTo("foo"));
        });
    }
}

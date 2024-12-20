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
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameBar()
    {
        const string source = "create name bar";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameFoo_WithNameProperty()
    {
        const string source = "create name foo name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void Deserialize_ShouldDeserialize_CreateNameBar_WithNameProperty()
    {
        const string source = "create name bar name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers.OfType<CreateTrigger>(), Is.Not.Null);
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }
}

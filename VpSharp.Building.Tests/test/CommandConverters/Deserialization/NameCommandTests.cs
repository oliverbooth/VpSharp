using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class NameCommandTests
{
    [Test]
    public void CreateName_ShouldDeserialize_GivenNameFoo()
    {
        const string source = "create name foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateName_ShouldDeserialize_GivenNameBar()
    {
        const string source = "create name bar";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateName_ShouldDeserialize_GivenNameFooAndNameProperty()
    {
        const string source = "create name foo name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void CreateName_ShouldDeserialize_GivenNameBarAndNameProperty()
    {
        const string source = "create name bar name=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NameCommand>());

            var command = (NameCommand)action.Create.Commands[0];
            Assert.That(command.Name, Is.EqualTo("bar"));
            Assert.That(command.ExecuteAs, Is.EqualTo("foo"));
        });
    }
}

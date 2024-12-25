using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class DiffuseCommandTests
{
    [Test]
    public void CreateDiffuse_ShouldDeserialize_WithIntensity50Percent()
    {
        const string source = "create diffuse 0.5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<DiffuseCommand>());

            var command = (DiffuseCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Tag, Is.EqualTo(Option.None<string>()));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateDiffuse_ShouldDeserialize_WithIntensity20Percent()
    {
        const string source = "create diffuse 0.2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<DiffuseCommand>());

            var command = (DiffuseCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.2));
            Assert.That(command.Tag, Is.EqualTo(Option.None<string>()));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateDiffuse_ShouldDeserialize_WithIntensity50PercentAndTagFoo()
    {
        const string source = "create diffuse 0.5 tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<DiffuseCommand>());

            var command = (DiffuseCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Tag, Is.EqualTo(Option.Some("foo")));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateDiffuse_ShouldDeserialize_WithIntensity20PercentAndTagFoo()
    {
        const string source = "create diffuse 0.2 tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<DiffuseCommand>());

            var command = (DiffuseCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.2));
            Assert.That(command.Tag, Is.EqualTo(Option.Some("foo")));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

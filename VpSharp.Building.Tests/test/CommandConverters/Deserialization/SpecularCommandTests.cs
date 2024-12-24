using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class SpecularCommandTests
{
    [Test]
    public void CreateSpecular_ShouldDeserialize_WithIntensity50Percent()
    {
        const string source = "create specular 0.5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SpecularCommand>());

            var command = (SpecularCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Shininess, Is.EqualTo(30.0));
            Assert.That(command.Alpha, Is.False);
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSpecular_ShouldDeserialize_WithIntensity50PercentAndShininess20Percent()
    {
        const string source = "create specular 0.5 0.2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SpecularCommand>());

            var command = (SpecularCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Shininess, Is.EqualTo(0.2));
            Assert.That(command.Alpha, Is.False);
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSpecular_ShouldDeserialize_WithIntensity50PercentAndTagFoo()
    {
        const string source = "create specular 0.5 tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SpecularCommand>());

            var command = (SpecularCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Shininess, Is.EqualTo(30.0));
            Assert.That(command.Alpha, Is.False);
            Assert.That(command.Tag, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSpecular_ShouldDeserialize_WithIntensity50PercentAndShininess20PercentAndTagFoo()
    {
        const string source = "create specular 0.5 0.2 tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SpecularCommand>());

            var command = (SpecularCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Shininess, Is.EqualTo(0.2));
            Assert.That(command.Alpha, Is.False);
            Assert.That(command.Tag, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSpecular_ShouldDeserialize_WithIntensity50PercentAndShininess20PercentAndTagFooAndAlphaFlag()
    {
        const string source = "create specular 0.5 0.2 alpha tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SpecularCommand>());

            var command = (SpecularCommand)action.Create.Commands[0];
            Assert.That(command.Intensity, Is.EqualTo(0.5));
            Assert.That(command.Shininess, Is.EqualTo(0.2));
            Assert.That(command.Alpha, Is.True);
            Assert.That(command.Tag, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

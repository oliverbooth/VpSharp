using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class ScaleCommandTests
{
    [Test]
    public void CreateScale_ShouldDeserializeScale_GivenXYZ()
    {
        const string source = "create scale 5 10 15";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ScaleCommand>());

            var command = (ScaleCommand)action.Create.Commands[0];
            Assert.That(command.Scale, Is.EqualTo(new Vector3d(5, 10, 15)));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateScale_ShouldDeserializeScale_GivenXY_AndNoZ()
    {
        const string source = "create scale 5 10";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ScaleCommand>());

            var command = (ScaleCommand)action.Create.Commands[0];
            Assert.That(command.Scale, Is.EqualTo(new Vector3d(5, 10, 1)));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateScale_ShouldDeserializeScale_GivenX_AndNoYZ()
    {
        const string source = "create scale 5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ScaleCommand>());

            var command = (ScaleCommand)action.Create.Commands[0];
            Assert.That(command.Scale, Is.EqualTo(new Vector3d(5, 1, 1)));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateScale_ShouldDeserializeScale_GivenNoArguments()
    {
        const string source = "create scale";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ScaleCommand>());

            var command = (ScaleCommand)action.Create.Commands[0];
            Assert.That(command.Scale, Is.EqualTo(Vector3d.One));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

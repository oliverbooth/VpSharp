using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class ShearCommandTests
{
    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZ()
    {
        const string source = "create shear 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.UnitZ));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZX()
    {
        const string source = "create shear 1 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.UnitX + Vector3d.UnitZ));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZXY()
    {
        const string source = "create shear 1 1 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.One));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZXY_AndNegativeY()
    {
        const string source = "create shear 1 1 1 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.One));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.UnitY));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZXY_AndNegativeYZ()
    {
        const string source = "create shear 1 1 1 1 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.One));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.UnitY + Vector3d.UnitZ));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateShear_ShouldDeserialize_GivenPositiveZXY_AndNegativeYZX()
    {
        const string source = "create shear 1 1 1 1 1 1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<ShearCommand>());

            var command = (ShearCommand)action.Create.Commands[0];
            Assert.That(command.PositiveShear, Is.EqualTo(Vector3d.One));
            Assert.That(command.NegativeShear, Is.EqualTo(Vector3d.One));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

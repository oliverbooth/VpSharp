using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class MoveCommandTests
{
    [Test]
    public void CreateMove_ShouldDeserialize_GivenXAxis()
    {
        const string source = "create move 90";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_GivenXYAxis()
    {
        const string source = "create move 45 90";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(45, 90, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_GivenXYZAxis()
    {
        const string source = "create move 45 90 270";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(45, 90, 270)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_LocalAxisFlag()
    {
        const string source = "create move 90 ltm";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.True);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_LoopingFlag()
    {
        const string source = "create move 90 loop";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.True);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_SmoothFlag()
    {
        const string source = "create move 90 smooth";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.True);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_ResetFlag()
    {
        const string source = "create move 90 reset";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.True);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_SyncFlag()
    {
        const string source = "create move 90 sync";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.True);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_TimeProperty()
    {
        const string source = "create move 90 time=10";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateMove_ShouldDeserialize_WaitProperty()
    {
        const string source = "create move 90 wait=10";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<MoveCommand>());

            var command = (MoveCommand)action.Create.Commands[0];
            Assert.That(command.Movement, Is.EqualTo(new Vector3d(90, 0, 0)));
            Assert.That(command.IsLocalAxis, Is.False);
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.IsSmooth, Is.False);
            Assert.That(command.Offset, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.ShouldReset, Is.False);
            Assert.That(command.ShouldSync, Is.False);
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Wait, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class PathCommandTests
{
    [Test]
    public void ActivatePath_ShouldDeserialize_GivenPathNameAndGlobal()
    {
        const string source = "activate path MyPath1 global;";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<PathCommand>());

            var command = (PathCommand)action.Activate.Commands[0];
            Assert.That(command.Name, Is.EqualTo("MyPath1"));
            Assert.That(command.ResetAfter, Is.EqualTo(Option.None<TimeSpan>()));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
            Assert.That(command.IsGlobal, Is.True);
        });
    }

    [Test]
    public void ActivatePath_ShouldDeserialize_GivenPathNameAndResetAfter()
    {
        const string source = "activate path MyPath1 resetafter=5;";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<PathCommand>());

            var command = (PathCommand)action.Activate.Commands[0];
            Assert.That(command.Name, Is.EqualTo("MyPath1"));
            Assert.That(command.ResetAfter, Is.EqualTo(Option.Some(TimeSpan.FromSeconds(5))));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
            Assert.That(command.IsGlobal, Is.False);
        });
    }
}

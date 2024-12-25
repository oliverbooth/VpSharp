using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class AstartCommandTests
{
    [Test]
    public void ActivateAstart_ShouldDeserializeName_GivenName()
    {
        const string source = "activate astart foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<AstartCommand>());

            var command = (AstartCommand)action.Activate.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void ActivateAstart_ShouldDeserializeNameAndLooping_GivenNameAndLooping()
    {
        const string source = "activate astart foo looping";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<AstartCommand>());

            var command = (AstartCommand)action.Activate.Commands[0];
            Assert.That(command.Name, Is.EqualTo("foo"));
            Assert.That(command.IsLooping, Is.True);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

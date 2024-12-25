using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class SoundCommandTests
{
    [Test]
    public void CreateSound_ShouldDeserialize_FileName()
    {
        const string source = "create sound ambient1.wav";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.Null);
            Assert.That(command.IsNonLooping, Is.False);
            Assert.That(command.Radius, Is.EqualTo(50.0));
            Assert.That(command.RightSpeaker, Is.Null);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSound_ShouldDeserialize_FileName_AndNoLoopFlag()
    {
        const string source = "create sound ambient1.wav noloop";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.Null);
            Assert.That(command.IsNonLooping, Is.True);
            Assert.That(command.Radius, Is.EqualTo(50.0));
            Assert.That(command.RightSpeaker, Is.Null);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSound_ShouldDeserialize_FileName_AndLeftSpeakerProperty()
    {
        const string source = "create sound ambient1.wav leftspk=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.EqualTo("foo"));
            Assert.That(command.IsNonLooping, Is.False);
            Assert.That(command.Radius, Is.EqualTo(50.0));
            Assert.That(command.RightSpeaker, Is.Null);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSound_ShouldDeserialize_FileName_AndRightSpeakerProperty()
    {
        const string source = "create sound ambient1.wav rightspk=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.Null);
            Assert.That(command.IsNonLooping, Is.False);
            Assert.That(command.Radius, Is.EqualTo(50.0));
            Assert.That(command.RightSpeaker, Is.EqualTo("foo"));
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSound_ShouldDeserialize_FileName_AndRadiusProperty()
    {
        const string source = "create sound ambient1.wav radius=5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.Null);
            Assert.That(command.IsNonLooping, Is.False);
            Assert.That(command.Radius, Is.EqualTo(5.0));
            Assert.That(command.RightSpeaker, Is.Null);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSound_ShouldDeserialize_FileName_AndVolumeProperty()
    {
        const string source = "create sound ambient1.wav volume=0.5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SoundCommand>());

            var command = (SoundCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.LeftSpeaker, Is.Null);
            Assert.That(command.IsNonLooping, Is.False);
            Assert.That(command.Radius, Is.EqualTo(50.0));
            Assert.That(command.RightSpeaker, Is.Null);
            Assert.That(command.Volume, Is.EqualTo(0.5));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

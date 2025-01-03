using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class NoiseCommandTests
{
    [Test]
    public void CreateNoise_ShouldDeserialize_FileName()
    {
        const string source = "create noise ambient1.wav";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NoiseCommand>());

            var command = (NoiseCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateNoise_ShouldDeserialize_FileName_AndLoopFlag()
    {
        const string source = "create noise ambient1.wav loop";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NoiseCommand>());

            var command = (NoiseCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.IsLooping, Is.True);
            Assert.That(command.Volume, Is.EqualTo(1.0));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateNoise_ShouldDeserialize_FileName_AndVolumeProperty()
    {
        const string source = "create noise ambient1.wav volume=0.5";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NoiseCommand>());

            var command = (NoiseCommand)action.Create.Commands[0];
            Assert.That(command.FileName, Is.EqualTo("ambient1.wav"));
            Assert.That(command.IsLooping, Is.False);
            Assert.That(command.Volume, Is.EqualTo(0.5));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

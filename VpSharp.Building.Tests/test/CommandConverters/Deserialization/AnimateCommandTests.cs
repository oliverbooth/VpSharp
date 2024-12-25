using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class AnimateCommandTests
{
    [Test]
    public void AdoneAnimate_ShouldDeserialize_GivenAnimateCommand()
    {
        const string source = "adone animate me . 1 8 100 1 2 3 4 5 6 7 8";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<AdoneTrigger>());
            Assert.That(action.Adone.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Adone.Commands[0], Is.InstanceOf<AnimateCommand>());

            var command = (AnimateCommand)action.Adone.Commands[0];
            Assert.That(command.IsMask, Is.False);
            Assert.That(command.Name, Is.EqualTo("me"));
            Assert.That(command.Animation, Is.EqualTo("."));
            Assert.That(command.ImageCount, Is.EqualTo(1));
            Assert.That(command.FrameCount, Is.EqualTo(8));
            Assert.That(command.FrameDelay, Is.EqualTo(TimeSpan.FromMilliseconds(100)));
            Assert.That(command.FrameList, Has.Count.EqualTo(8));
            Assert.That(command.FrameList, Is.EquivalentTo([1, 2, 3, 4, 5, 6, 7, 8]));
        });
    }

    [Test]
    public void AdoneAnimate_ShouldDeserialize_GivenAnimateCommandWithMask()
    {
        const string source = "adone animate mask me . 1 8 100 1 2 3 4 5 6 7 8";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<AdoneTrigger>());
            Assert.That(action.Adone.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Adone.Commands[0], Is.InstanceOf<AnimateCommand>());

            var command = (AnimateCommand)action.Adone.Commands[0];
            Assert.That(command.IsMask, Is.True);
            Assert.That(command.Name, Is.EqualTo("me"));
            Assert.That(command.Animation, Is.EqualTo("."));
            Assert.That(command.ImageCount, Is.EqualTo(1));
            Assert.That(command.FrameCount, Is.EqualTo(8));
            Assert.That(command.FrameDelay, Is.EqualTo(TimeSpan.FromMilliseconds(100)));
            Assert.That(command.FrameList, Has.Count.EqualTo(8));
            Assert.That(command.FrameList, Is.EquivalentTo([1, 2, 3, 4, 5, 6, 7, 8]));
        });
    }
}

using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SoundCommandTests
{
    [Test]
    public void CreateSound_ShouldSerialize_FileName()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav" }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSound_ShouldSerialize_FileName_AndNoLoopFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav", IsNonLooping = true }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav noloop";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSound_ShouldSerialize_FileName_AndLeftSpeakerProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav", LeftSpeaker = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav leftspk=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSound_ShouldSerialize_FileName_AndRightSpeakerProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav", RightSpeaker = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav rightspk=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSound_ShouldSerialize_FileName_AndRadiusProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav", Radius = 5 }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav radius=5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSound_ShouldSerialize_FileName_AndVolumeProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SoundCommand { FileName = "ambient1.wav", Volume = 0.5 }
                    ]
                }
            ]
        };

        const string expected = "create sound ambient1.wav volume=0.5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

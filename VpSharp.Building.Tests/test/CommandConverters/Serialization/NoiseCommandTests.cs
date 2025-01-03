using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class NoiseCommandTests
{
    [Test]
    public void CreateNoise_ShouldSerialize_FileName()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NoiseCommand { FileName = "ambient1.wav" }
                    ]
                }
            ]
        };

        const string expected = "create noise ambient1.wav";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateNoise_ShouldSerialize_FileName_AndLoopFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NoiseCommand { FileName = "ambient1.wav", IsLooping = true }
                    ]
                }
            ]
        };

        const string expected = "create noise ambient1.wav loop";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateNoise_ShouldSerialize_FileName_AndVolumeProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NoiseCommand { FileName = "ambient1.wav", Volume = 0.5 }
                    ]
                }
            ]
        };

        const string expected = "create noise ambient1.wav volume=0.5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

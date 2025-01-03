using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class FramerateCommandTests
{
    [Test]
    public void CreateFramerateShouldSerialize_CreateFramerate60()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new FramerateCommand { Framerate = 60 }
                    ]
                }
            ]
        };

        const string expected = "create framerate 60";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateFramerateShouldSerialize_CreateFramerate30()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new FramerateCommand { Framerate = 30 }
                    ]
                }
            ]
        };

        const string expected = "create framerate 30";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateFramerateShouldSerialize_CreateFramerate60_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new FramerateCommand { Framerate = 60, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create framerate 60 name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateFramerateShouldSerialize_CreateFramerate30_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new FramerateCommand { Framerate = 30, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create framerate 30 name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

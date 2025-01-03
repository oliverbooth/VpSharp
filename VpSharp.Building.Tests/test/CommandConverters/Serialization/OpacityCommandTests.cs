using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class OpacityCommandTests
{
    [Test]
    public void CreateOpacity_ShouldSerialize_CreateOpacity80Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new OpacityCommand { Opacity = 0.8 }
                    ]
                }
            ]
        };

        const string expected = "create opacity 0.8";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateOpacity_ShouldSerialize_CreateOpacity20Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new OpacityCommand { Opacity = 0.2 }
                    ]
                }
            ]
        };

        const string expected = "create opacity 0.2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateOpacity_ShouldSerialize_CreateOpacity80Percent_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new OpacityCommand { Opacity = 0.8, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create opacity 0.8 name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateOpacity_ShouldSerialize_CreateOpacity20Percent_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new OpacityCommand { Opacity = 0.2, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create opacity 0.2 name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class CameraCommandTests
{
    [Test]
    public void CreateCamera_ShouldSerialize_GivenTarget()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new CameraCommand
                        {
                            Target = Option.Some("foo"),
                            Location = Option.None<string>(),
                            ExecuteAs = Option.None<string>()
                        }
                    ]
                }
            ]
        };

        const string expected = "create camera target=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateCamera_ShouldSerialize_GivenLocation()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new CameraCommand
                        {
                            Target = Option.None<string>(),
                            Location = Option.Some("bar"),
                            ExecuteAs = Option.None<string>()
                        }
                    ]
                }
            ]
        };

        const string expected = "create camera location=bar";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateCamera_ShouldSerialize_GivenTargetAndLocation()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new CameraCommand
                        {
                            Target = Option.Some("foo"),
                            Location = Option.Some("bar"),
                            ExecuteAs = Option.None<string>()
                        }
                    ]
                }
            ]
        };

        const string expected = "create camera location=bar target=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

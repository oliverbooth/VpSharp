using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class DiffuseCommandTests
{
    [Test]
    public void CreateDiffuse_ShouldSerialize_WithIntensity50Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new DiffuseCommand { Intensity = 0.5 }
                    ]
                }
            ]
        };

        const string expected = "create diffuse 0.5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateDiffuse_ShouldSerialize_WithIntensity20Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new DiffuseCommand { Intensity = 0.2 }
                    ]
                }
            ]
        };

        const string expected = "create diffuse 0.2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateDiffuse_ShouldSerialize_WithIntensity50PercentAndTagFoo()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new DiffuseCommand { Intensity = 0.5, Tag = Option.Some<string>("foo") }
                    ]
                }
            ]
        };

        const string expected = "create diffuse 0.5 tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateDiffuse_ShouldSerialize_WithIntensity20PercentAndTagFoo()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new DiffuseCommand { Intensity = 0.2, Tag = Option.Some<string>("foo") }
                    ]
                }
            ]
        };

        const string expected = "create diffuse 0.2 tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SpecularCommandTests
{
    [Test]
    public void CreateSpecular_ShouldSerialize_WithIntensity50Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularCommand { Intensity = 0.5 }
                    ]
                }
            ]
        };

        const string expected = "create specular 0.5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecular_ShouldSerialize_WithIntensity50PercentAndShininess20Percent()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularCommand { Intensity = 0.5, Shininess = 0.2 }
                    ]
                }
            ]
        };

        const string expected = "create specular 0.5 0.2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecular_ShouldSerialize_WithIntensity50PercentAndTagFoo()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularCommand { Intensity = 0.5, Tag = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create specular 0.5 tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecular_ShouldSerialize_WithIntensity50PercentAndShininess20PercentAndTagFoo()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularCommand { Intensity = 0.5, Shininess = 0.2, Tag = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create specular 0.5 0.2 tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecular_ShouldSerialize_WithIntensity50PercentAndShininess20PercentAndTagFooAndAlphaFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularCommand { Intensity = 0.5, Shininess = 0.2, Alpha = true, Tag = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create specular 0.5 0.2 alpha tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

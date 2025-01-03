using System.Drawing;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class LightCommandTests
{
    [Test]
    public void CreateLight_ShouldSerialize_GivenAngle()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Angle = 90 }
                    ]
                }
            ]
        };

        const string expected = "create light angle=90";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenBrightness()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Brightness = 0.75 }
                    ]
                }
            ]
        };

        const string expected = "create light brightness=0.75";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenColor()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Color = Color.Red }
                    ]
                }
            ]
        };

        const string expected = "create light color=red";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenEffect()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Effect = LightEffect.Blink }
                    ]
                }
            ]
        };

        const string expected = "create light fx=blink";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenMaxDistance()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { MaxDistance = 500 }
                    ]
                }
            ]
        };

        const string expected = "create light maxdist=500";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenRadius()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Radius = 20 }
                    ]
                }
            ]
        };

        const string expected = "create light radius=20";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenTime()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Time = TimeSpan.FromSeconds(2) }
                    ]
                }
            ]
        };

        const string expected = "create light time=2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateLight_ShouldSerialize_GivenType()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new LightCommand { Type = LightType.Spot }
                    ]
                }
            ]
        };

        const string expected = "create light type=spot";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class ScaleCommandTests
{
    [Test]
    public void CreateScale_ShouldSerializeScale_GivenXYZ()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ScaleCommand { Scale = new Vector3d(5, 10, 15) }
                    ]
                }
            ]
        };

        const string expected = "create scale 5 10 15";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateScale_ShouldSerializeScale_GivenXY_AndNoZ()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ScaleCommand { Scale = new Vector3d(5, 10, 1) }
                    ]
                }
            ]
        };

        const string expected = "create scale 5 10";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateScale_ShouldSerializeScale_GivenX_AndNoYZ()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ScaleCommand { Scale = new Vector3d(5, 1, 1) }
                    ]
                }
            ]
        };

        const string expected = "create scale 5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateScale_ShouldDeserializeScale_GivenNoArguments()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ScaleCommand { Scale = Vector3d.One }
                    ]
                }
            ]
        };

        const string expected = "create scale 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

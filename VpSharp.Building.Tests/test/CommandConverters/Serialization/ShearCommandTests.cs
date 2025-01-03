using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class ShearCommandTests
{
    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZ()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = Vector3d.UnitZ }
                    ]
                }
            ]
        };

        const string expected = "create shear 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZX()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = new Vector3d(1, 0, 1) }
                    ]
                }
            ]
        };

        const string expected = "create shear 1 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZXY()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = Vector3d.One }
                    ]
                }
            ]
        };

        const string expected = "create shear 1 1 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZXY_AndNegativeY()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = Vector3d.One, NegativeShear = Vector3d.UnitY }
                    ]
                }
            ]
        };

        const string expected = "create shear 1 1 1 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZXY_AndNegativeYZ()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = Vector3d.One, NegativeShear = new Vector3d(0, 1, 1) }
                    ]
                }
            ]
        };

        const string expected = "create shear 1 1 1 1 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateShear_ShouldSerialize_GivenPositiveZXY_AndNegativeYZX()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new ShearCommand { PositiveShear = Vector3d.One, NegativeShear = Vector3d.One }
                    ]
                }
            ]
        };

        const string expected = "create shear 1 1 1 1 1 1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

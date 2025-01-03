using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SolidCommandTests
{
    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidOff()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = false }
                    ]
                }
            ]
        };

        const string expected = "create solid off";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidOn()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = true }
                    ]
                }
            ]
        };

        const string expected = "create solid on";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidFooOff()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = false, Target = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create solid foo off";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidOff_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = false, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create solid off name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidFooOn()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = true, Target = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create solid foo on";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolid_ShouldSerialize_CreateSolidOn_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SolidCommand { IsSolid = true, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create solid on name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

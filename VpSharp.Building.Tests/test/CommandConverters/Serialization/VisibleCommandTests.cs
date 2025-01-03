using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class VisibleCommandTests
{
    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleOff()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false }
                    ]
                }
            ]
        };

        const string expected = "create visible off";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleOn()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true }
                    ]
                }
            ]
        };

        const string expected = "create visible on";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleFooOff()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false, Target = "foo"}
                    ]
                }
            ]
        };

        const string expected = "create visible foo off";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleOff_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create visible off name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleFooOn()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true, Target = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create visible foo on";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateVisible_ShouldSerialize_CreateVisibleOn_WithNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true, ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create visible on name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

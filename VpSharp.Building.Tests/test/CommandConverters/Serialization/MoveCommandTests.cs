using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class MoveCommandTests
{
    [Test]
    public void CreateMove_ShouldSerialize_GivenXAxis()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0) }
                    ]
                }
            ]
        };

        const string expected = "create move 90";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_GivenXYAxis()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(45, 90, 0) }
                    ]
                }
            ]
        };

        const string expected = "create move 45 90";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_GivenXYZAxis()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(45, 90, 270) }
                    ]
                }
            ]
        };

        const string expected = "create move 45 90 270";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_LocalAxisFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), IsLocalAxis = true }
                    ]
                }
            ]
        };

        const string expected = "create move 90 ltm";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_LoopingFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), IsLooping = true }
                    ]
                }
            ]
        };

        const string expected = "create move 90 loop";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_SmoothFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), IsSmooth = true }
                    ]
                }
            ]
        };

        const string expected = "create move 90 smooth";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_ResetFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), ShouldReset = true }
                    ]
                }
            ]
        };

        const string expected = "create move 90 reset";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_SyncFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), ShouldSync = true }
                    ]
                }
            ]
        };

        const string expected = "create move 90 sync";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_TimeProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), Time = Option.Some(TimeSpan.FromSeconds(10)) }
                    ]
                }
            ]
        };

        const string expected = "create move 90 time=10";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateMove_ShouldSerialize_WaitProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new MoveCommand { Movement = new Vector3d(90, 0, 0), Wait = Option.Some(TimeSpan.FromSeconds(10)) }
                    ]
                }
            ]
        };

        const string expected = "create move 90 wait=10";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

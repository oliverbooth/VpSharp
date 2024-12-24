using System.Drawing;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class LightCommandTests
{
    [Test]
    public void CreateLight_ShouldDeserialize_GivenAngle()
    {
        const string source = "create light angle=90";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(90));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenBrightness()
    {
        const string source = "create light brightness=0.75";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.75));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenColor()
    {
        const string source = "create light color=red";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.Red));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenEffect()
    {
        const string source = "create light fx=blink";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.Blink));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenMaxDistance()
    {
        const string source = "create light maxdist=500";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(500.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenRadius()
    {
        const string source = "create light radius=20";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(20.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenTime()
    {
        const string source = "create light time=2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(2)));
            Assert.That(command.Type, Is.EqualTo(LightType.Point));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateLight_ShouldDeserialize_GivenType()
    {
        const string source = "create light type=spot";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<LightCommand>());

            var command = (LightCommand)action.Create.Commands[0];
            Assert.That(command.Angle, Is.EqualTo(45.0));
            Assert.That(command.Brightness, Is.EqualTo(0.5));
            Assert.That(command.Color, Is.EqualTo(Color.White));
            Assert.That(command.Effect, Is.EqualTo(LightEffect.None));
            Assert.That(command.MaxDistance, Is.EqualTo(1000.0));
            Assert.That(command.Radius, Is.EqualTo(10.0));
            Assert.That(command.Time, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(command.Type, Is.EqualTo(LightType.Spot));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

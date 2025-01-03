using System.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests;

internal sealed class ActionSerializerTests
{
    [Test]
    public void Deserialize_ShouldProduceVirtualParadiseActionResult_GivenValidAction()
    {
        const string source = "create visible on radius=10, texture stone1; activate visible off global";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action, Is.Not.Null);
            Assert.That(action.Triggers, Has.Count.EqualTo(2));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Triggers[1], Is.InstanceOf<ActivateTrigger>());

            CreateTrigger createTrigger = (CreateTrigger)action.Triggers[0];
            ActivateTrigger activateTrigger = (ActivateTrigger)action.Triggers[1];
            Assert.That(createTrigger.Commands, Has.Count.EqualTo(2));
            Assert.That(createTrigger.Commands[0], Is.InstanceOf<VisibleCommand>());
            Assert.That(createTrigger.Commands[1], Is.InstanceOf<TextureCommand>());

            VisibleCommand visibleCommand = (VisibleCommand)createTrigger.Commands[0];
            TextureCommand textureCommand = (TextureCommand)createTrigger.Commands[1];

            Assert.That(visibleCommand.IsVisible, Is.True);
            Assert.That(visibleCommand.Radius, Is.EqualTo(10.0));
            Assert.That(textureCommand.Texture, Is.EqualTo("stone1"));

            Assert.That(activateTrigger.Commands, Has.Count.EqualTo(1));
            Assert.That(activateTrigger.Commands[0], Is.InstanceOf<VisibleCommand>());

            visibleCommand = (VisibleCommand)activateTrigger.Commands[0];
            Assert.That(visibleCommand.IsVisible, Is.False);
            Assert.That(visibleCommand.IsGlobal, Is.True);
        });
    }

    [Test]
    public void Deserialize_ShouldThrowArgumentNullException_GivenNullStream()
    {
        Assert.Throws<ArgumentNullException>(() => ActionSerializer.Deserialize((Stream)null!, new ActionSerializerOptions()));
    }

    [Test]
    public void Serialize_ShouldProduceStringResult_GivenValidAction()
    {
        const string expected = "create visible on radius=10, texture stone1; activate visible off global";

        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true, Radius = 10.0 },
                        new TextureCommand { Texture = "stone1" }
                    ]
                },
                new ActivateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false, IsGlobal = true }
                    ]
                },
            ]
        };

        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_ShouldWriteToStream_GivenValidAction()
    {
        const string expected = "create visible on radius=10, texture stone1; activate visible off global";

        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = true, Radius = 10.0 },
                        new TextureCommand { Texture = "stone1" }
                    ]
                },
                new ActivateTrigger
                {
                    Commands =
                    [
                        new VisibleCommand { IsVisible = false, IsGlobal = true }
                    ]
                },
            ]
        };

        using var stream = new MemoryStream();
        ActionSerializer.Serialize(stream, action);

        byte[] buffer = stream.ToArray();
        string result = Encoding.UTF8.GetString(buffer);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_ShouldThrowArgumentNullException_GivenNullAction()
    {
        Assert.Throws<ArgumentNullException>(() => ActionSerializer.Serialize(null!));
        Assert.Throws<ArgumentNullException>(() => ActionSerializer.Serialize(Stream.Null, null!));
    }

    [Test]
    public void Serialize_ShouldThrowArgumentNullException_GivenNullStream()
    {
        Assert.Throws<ArgumentNullException>(() => ActionSerializer.Serialize(null!, new VirtualParadiseAction()));
    }
}

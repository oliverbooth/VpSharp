using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class WebCommandTests
{
    [Test]
    public void CreateWeb_ShouldDeserialize_GivenUrl()
    {
        const string source = "create web url=https://www.virtualparadise.org";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(512.0));
            Assert.That(command.FrameHeight, Is.EqualTo(512.0));
            Assert.That(command.Keys, Is.False);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateWeb_ShouldDeserialize_GivenUrlAndFrameHeight()
    {
        const string source = "create web url=https://www.virtualparadise.org sh=1024";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(512.0));
            Assert.That(command.FrameHeight, Is.EqualTo(1024.0));
            Assert.That(command.Keys, Is.False);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateWeb_ShouldDeserialize_GivenUrlAndFrameWidth()
    {
        const string source = "create web url=https://www.virtualparadise.org sw=1024";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(1024.0));
            Assert.That(command.FrameHeight, Is.EqualTo(512.0));
            Assert.That(command.Keys, Is.False);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateWeb_ShouldDeserialize_GivenUrlAndFrameWidthAndHeight()
    {
        const string source = "create web url=https://www.virtualparadise.org sw=1024 sh=1024";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(1024.0));
            Assert.That(command.FrameHeight, Is.EqualTo(1024.0));
            Assert.That(command.Keys, Is.False);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateWeb_ShouldDeserializeAsKeysFalse_GivenUrlAndKeysOn()
    {
        const string source = "create web url=https://www.virtualparadise.org keys=on";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(512.0));
            Assert.That(command.FrameHeight, Is.EqualTo(512.0));
            Assert.That(command.Keys, Is.False);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateWeb_ShouldDeserializeAsKeysFalse_GivenUrlAndKeysYes()
    {
        const string source = "create web url=https://www.virtualparadise.org keys=yes";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<WebCommand>());

            var command = (WebCommand)action.Create.Commands[0];
            Assert.That(command.FrameWidth, Is.EqualTo(512.0));
            Assert.That(command.FrameHeight, Is.EqualTo(512.0));
            Assert.That(command.Keys, Is.True);
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

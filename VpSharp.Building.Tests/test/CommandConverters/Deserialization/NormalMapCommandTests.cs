using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class NormalMapCommandTests
{
    [Test]
    public void CreateTexture_ShouldDeserialize_WithTextureStone1()
    {
        const string source = "create normalmap stone1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NormalMapCommand>());

            var command = (NormalMapCommand)action.Create.Commands[0];
            Assert.That(command.Texture, Is.EqualTo("stone1"));
            Assert.That(command.Mask, Is.Null);
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateTexture_ShouldDeserialize_WithTextureStone1AndMaskAwb8m()
    {
        const string source = "create normalmap stone1 mask=awb8m";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NormalMapCommand>());

            var command = (NormalMapCommand)action.Create.Commands[0];
            Assert.That(command.Texture, Is.EqualTo("stone1"));
            Assert.That(command.Mask, Is.EqualTo("awb8m"));
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateTexture_ShouldDeserialize_WithTextureStone1AndTag1()
    {
        const string source = "create normalmap stone1 tag=1";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NormalMapCommand>());

            var command = (NormalMapCommand)action.Create.Commands[0];
            Assert.That(command.Texture, Is.EqualTo("stone1"));
            Assert.That(command.Mask, Is.Null);
            Assert.That(command.Tag, Is.EqualTo("1"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateTexture_ShouldDeserialize_WithTextureStone1AndMaskAwb8mAndTag1()
    {
        const string source = "create normalmap stone1 tag=1 mask=awb8m";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<NormalMapCommand>());

            var command = (NormalMapCommand)action.Create.Commands[0];
            Assert.That(command.Texture, Is.EqualTo("stone1"));
            Assert.That(command.Mask, Is.EqualTo("awb8m"));
            Assert.That(command.Tag, Is.EqualTo("1"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}
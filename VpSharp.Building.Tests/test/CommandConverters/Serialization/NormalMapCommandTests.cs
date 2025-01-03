using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class NormalMapCommandTests
{
    [Test]
    public void CreateNormalMap_ShouldSerialize_WithTextureStone1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NormalMapCommand { Texture = "stone1" }
                    ]
                }
            ]
        };

        const string expected = "create normalmap stone1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateNormalMap_ShouldSerialize_WithTextureStone1AndMaskAwb8m()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NormalMapCommand { Texture = "stone1", Mask = Option.Some("awb8m") }
                    ]
                }
            ]
        };

        const string expected = "create normalmap stone1 mask=awb8m";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateNormalMap_ShouldSerialize_WithTextureStone1AndTag1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NormalMapCommand { Texture = "stone1", Tag = Option.Some("1") }
                    ]
                }
            ]
        };

        const string expected = "create normalmap stone1 tag=1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateNormalMap_ShouldSerialize_WithTextureStone1AndMaskAwb8mAndTag1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NormalMapCommand { Texture = "stone1", Tag = Option.Some("1"), Mask = Option.Some("awb8m") }
                    ]
                }
            ]
        };

        const string expected = "create normalmap stone1 mask=awb8m tag=1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

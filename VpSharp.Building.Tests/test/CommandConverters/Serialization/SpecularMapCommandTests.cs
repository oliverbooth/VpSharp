using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SpecularMapCommandTests
{
    [Test]
    public void CreateSpecularMap_ShouldSerialize_WithTextureStone1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularMapCommand { Texture = "stone1" }
                    ]
                }
            ]
        };

        const string expected = "create specularmap stone1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecularMap_ShouldSerialize_WithTextureStone1AndMaskAwb8m()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularMapCommand { Texture = "stone1", Mask = Option.Some("awb8m") }
                    ]
                }
            ]
        };

        const string expected = "create specularmap stone1 mask=awb8m";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecularMap_ShouldSerialize_WithTextureStone1AndTag1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularMapCommand { Texture = "stone1", Tag = Option.Some("1") }
                    ]
                }
            ]
        };

        const string expected = "create specularmap stone1 tag=1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSpecularMap_ShouldSerialize_WithTextureStone1AndMaskAwb8mAndTag1()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SpecularMapCommand { Texture = "stone1", Mask = Option.Some("awb8m"), Tag = Option.Some("1") }
                    ]
                }
            ]
        };

        const string expected = "create specularmap stone1 mask=awb8m tag=1";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

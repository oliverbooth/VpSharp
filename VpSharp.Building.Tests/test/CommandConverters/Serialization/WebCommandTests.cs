using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class WebCommandTests
{
    [Test]
    public void CreateWeb_ShouldSerialize_GivenUrl()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new WebCommand { Url = "https://www.virtualparadise.org" }
                    ]
                }
            ]
        };

        const string expected = "create web url=https://www.virtualparadise.org";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateWeb_ShouldSerialize_GivenUrlAndFrameHeight()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new WebCommand { Url = "https://www.virtualparadise.org", FrameHeight = 1024 }
                    ]
                }
            ]
        };

        const string expected = "create web url=https://www.virtualparadise.org sh=1024";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateWeb_ShouldSerialize_GivenUrlAndFrameWidth()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new WebCommand { Url = "https://www.virtualparadise.org", FrameWidth = 1024 }
                    ]
                }
            ]
        };

        const string expected = "create web url=https://www.virtualparadise.org sw=1024";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateWeb_ShouldSerialize_GivenUrlAndFrameWidthAndHeight()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new WebCommand { Url = "https://www.virtualparadise.org", FrameHeight = 1024, FrameWidth = 1024 }
                    ]
                }
            ]
        };

        const string expected = "create web url=https://www.virtualparadise.org sw=1024 sh=1024";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateWeb_ShouldSerializeAsKeysYes_GivenUrlAndKeysTrue()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new WebCommand { Url = "https://www.virtualparadise.org", Keys = true }
                    ]
                }
            ]
        };

        const string expected = "create web url=https://www.virtualparadise.org keys=yes";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

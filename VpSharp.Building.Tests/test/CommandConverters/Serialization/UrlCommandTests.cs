using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class UrlCommandTests
{
    [Test]
    public void ActivateUrl_ShouldSerialize_GivenUrl()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new UrlCommand { Url = "https://www.virtualparadise.org" }
                    ]
                }
            ]
        };

        const string expected = "activate url https://www.virtualparadise.org";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

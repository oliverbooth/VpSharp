using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class AstartCommandTests
{
    [Test]
    public void ActivateAstart_ShouldSerializeName_GivenName()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new AstartCommand { Name = "foo", IsLooping = false }
                    ]
                }
            ]
        };

        const string expected = "activate astart foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateAstart_ShouldSerializeNameAndLooping_GivenNameAndLooping()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new AstartCommand { Name = "foo", IsLooping = true }
                    ]
                }
            ]
        };

        const string expected = "activate astart foo looping";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

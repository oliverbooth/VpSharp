using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class AstopCommandTests
{
    [Test]
    public void ActivateAstop_ShouldSerializeName_GivenName()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new AstopCommand { Name = "foo" }
                    ]
                }
            ]
        };

        const string expected = "activate astop foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

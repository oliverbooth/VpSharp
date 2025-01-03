using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class PathCommandTests
{
    [Test]
    public void ActivatePath_ShouldSerialize_GivenPathNameAndGlobal()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new PathCommand { Name = "MyPath1", IsGlobal = true }
                    ]
                }
            ]
        };

        const string expected = "activate path MyPath1 global";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivatePath_ShouldSerialize_GivenPathNameAndResetAfter()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new PathCommand { Name = "MyPath1", ResetAfter = Option.Some(TimeSpan.FromSeconds(5)) }
                    ]
                }
            ]
        };

        const string expected = "activate path MyPath1 resetafter=5";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

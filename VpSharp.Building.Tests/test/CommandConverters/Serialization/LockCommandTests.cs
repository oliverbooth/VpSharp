using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class LockCommandTests
{
    [Test]
    public void ActivateLock_ShouldSerializeWithNoOwners()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new LockCommand()
                    ]
                }
            ]
        };

        const string expected = "activate lock";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateLockWithOwner_ShouldSerializeWithOneOwner()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new LockCommand { Owners = [11] }
                    ]
                }
            ]
        };

        const string expected = "activate lock owners=11";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateLockWithOwners_ShouldSerializeWithMultipleOwners()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new LockCommand { Owners = [11, 122] }
                    ]
                }
            ]
        };

        const string expected = "activate lock owners=11:122";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

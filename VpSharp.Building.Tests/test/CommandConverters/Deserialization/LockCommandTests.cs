using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class LockCommandTests
{
    [Test]
    public void ActivateLock_ShouldLock_WithEmptyOwnersList()
    {
        const string source = "activate lock";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<LockCommand>());

            var command = (LockCommand)action.Activate.Commands[0];
            Assert.That(command.Owners, Is.Empty);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void ActivateLockWithOwner_ShouldLock_WithOneParsedIntegerOwnerList()
    {
        const string source = "activate lock owners=11";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<LockCommand>());

            var command = (LockCommand)action.Activate.Commands[0];
            Assert.That(command.Owners, Has.Count.EqualTo(1));
            Assert.That(command.Owners, Contains.Item(11));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void ActivateLockWithOwners_ShouldLock_WithParsedIntegersOwnerList()
    {
        const string source = "activate lock owners=11:122";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<LockCommand>());

            var command = (LockCommand)action.Activate.Commands[0];
            Assert.That(command.Owners, Has.Count.EqualTo(2));
            Assert.That(command.Owners, Contains.Item(11).And.Contains(122));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

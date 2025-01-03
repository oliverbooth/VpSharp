using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests;

internal sealed class VirtualParadiseActionBuilderTests
{
    [Test]
    public void AddTrigger_ShouldAddTrigger_GivenTrigger()
    {
        var builder = new VirtualParadiseActionBuilder();
        var trigger = new ActivateTrigger();

        builder.AddTrigger(trigger);

        VirtualParadiseAction action = builder.Build();

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers, Has.Member(trigger));
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(0));

            string result = ActionSerializer.Serialize(action);
            Assert.That(result, Is.EqualTo("activate"));
        });
    }

    [Test]
    public void AddTrigger_ShouldAddTrigger_GivenBuilder()
    {
        var builder = new VirtualParadiseActionBuilder();

        builder.AddTrigger<CreateTrigger>(trigger =>
        {
            trigger
                .AddCommand(new SolidCommand { IsSolid = false })
                .AddCommand(new VisibleCommand { IsVisible = false });
        });

        VirtualParadiseAction action = builder.Build();

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(2));

            string result = ActionSerializer.Serialize(action);
            Assert.That(result, Is.EqualTo("create solid off, visible off"));
        });
    }
}

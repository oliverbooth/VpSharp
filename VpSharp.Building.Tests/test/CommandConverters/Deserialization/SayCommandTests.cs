using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class SayCommandTests
{
    [Test]
    public void CreateSay_ShouldDeserialize_GivenNoArguments()
    {
        const string source = "activate say";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<SayCommand>());

            var command = (SayCommand)action.Activate.Commands[0];
            Assert.That(command.Message, Is.Empty);
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }

    [Test]
    public void CreateSay_ShouldDeserialize_GivenMessage()
    {
        const string source = "activate say \"Hello World\"";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<SayCommand>());

            var command = (SayCommand)action.Activate.Commands[0];
            Assert.That(command.Message, Is.EqualTo("Hello World"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
        });
    }
}

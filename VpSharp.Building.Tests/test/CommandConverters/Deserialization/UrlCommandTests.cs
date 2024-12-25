using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class UrlCommandTests
{
    [Test]
    public void ActivateUrl_ShouldDeserialize_GivenUrl()
    {
        const string source = "activate url https://www.virtualparadise.org";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<ActivateTrigger>());
            Assert.That(action.Activate.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Activate.Commands[0], Is.InstanceOf<UrlCommand>());

            var command = (UrlCommand)action.Activate.Commands[0];
            Assert.That(command.Url, Is.EqualTo("https://www.virtualparadise.org"));
            Assert.That(command.ExecuteAs, Is.EqualTo(Option.None<string>()));
            Assert.That(command.IsGlobal, Is.False);
        });
    }
}

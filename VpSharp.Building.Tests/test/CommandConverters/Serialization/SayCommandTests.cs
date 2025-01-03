using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SayCommandTests
{
    [Test]
    public void ActivateSay_ShouldSerialize_GivenNoArguments()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new SayCommand()
                    ]
                }
            ]
        };

        const string expected = "activate say";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ActivateSay_ShouldSerialize_GivenMessage()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new ActivateTrigger
                {
                    Commands =
                    [
                        new SayCommand { Message = "Hello World", }
                    ]
                }
            ]
        };

        const string expected = "activate say \"Hello World\"";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

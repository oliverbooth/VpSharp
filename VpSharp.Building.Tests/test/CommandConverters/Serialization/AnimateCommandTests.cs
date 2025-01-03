using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class AnimateCommandTests
{
    [Test]
    public void AdoneAnimate_ShouldSerialize_GivenAnimateCommand()
    {
        var action = new VirtualParadiseAction
        {
            Triggers = [
                new AdoneTrigger
                {
                    Commands = [
                        new AnimateCommand
                        {
                            Name = "me",
                            Animation = ".",
                            ImageCount = 1,
                            FrameCount = 8,
                            FrameDelay = TimeSpan.FromMilliseconds(100),
                            FrameList = [1, 2, 3, 4, 5, 6, 7, 8],
                            IsMask = false
                        }
                    ]
                }
            ]
        };
        
        const string expected = "adone animate me . 1 8 100 1 2 3 4 5 6 7 8";
        string result = ActionSerializer.Serialize(action);
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AdoneAnimate_ShouldSerialize_GivenAnimateCommandWithMask()
    {
        var action = new VirtualParadiseAction
        {
            Triggers = [
                new AdoneTrigger
                {
                    Commands = [
                        new AnimateCommand
                        {
                            Name = "me",
                            Animation = ".",
                            ImageCount = 1,
                            FrameCount = 8,
                            FrameDelay = TimeSpan.FromMilliseconds(100),
                            FrameList = [1, 2, 3, 4, 5, 6, 7, 8],
                            IsMask = true
                        }
                    ]
                }
            ]
        };
        
        const string expected = "adone animate mask me . 1 8 100 1 2 3 4 5 6 7 8";
        string result = ActionSerializer.Serialize(action);
        
        Assert.That(result, Is.EqualTo(expected));
    }
}

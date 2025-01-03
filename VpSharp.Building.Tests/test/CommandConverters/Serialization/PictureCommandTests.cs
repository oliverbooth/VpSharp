using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class PictureCommandTests
{
    [Test]
    public void CreatePicture_ShouldSerialize_WithPictureUrl()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new PictureCommand { Picture = "vmist.net/ptv/tv.php" }
                    ]
                }
            ]
        };

        const string expected = "create picture vmist.net/ptv/tv.php";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreatePicture_ShouldSerialize_WithPictureUrlAndUpdateProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new PictureCommand { Picture = "vmist.net/ptv/tv.php", Update = Option.Some(TimeSpan.FromSeconds(10)) }
                    ]
                }
            ]
        };

        const string expected = "create picture vmist.net/ptv/tv.php update=10";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreatePicture_ShouldSerialize_WithPictureUrlAndTagPropertyAndUpdateProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new PictureCommand
                        {
                            Picture = "vmist.net/ptv/tv.php",
                            Tag = Option.Some("foo"),
                            Update = Option.Some(TimeSpan.FromSeconds(10))
                        }
                    ]
                }
            ]
        };

        const string expected = "create picture vmist.net/ptv/tv.php update=10 tag=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

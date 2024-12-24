using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class PictureCommandTests
{
    [Test]
    public void CreatePicture_ShouldDeserialize_WithPictureUrl()
    {
        const string source = "create picture vmist.net/ptv/tv.php";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<PictureCommand>());

            var command = (PictureCommand)action.Create.Commands[0];
            Assert.That(command.Picture, Is.EqualTo("vmist.net/ptv/tv.php"));
            Assert.That(command.Update, Is.EqualTo(TimeSpan.Zero));
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreatePicture_ShouldDeserialize_WithPictureUrlAndUpdateProperty()
    {
        const string source = "create picture vmist.net/ptv/tv.php update=10";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<PictureCommand>());

            var command = (PictureCommand)action.Create.Commands[0];
            Assert.That(command.Picture, Is.EqualTo("vmist.net/ptv/tv.php"));
            Assert.That(command.Update, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(command.Tag, Is.Null);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreatePicture_ShouldDeserialize_WithPictureUrlAndTagPropertyAndUpdateProperty()
    {
        const string source = "create picture vmist.net/ptv/tv.php update=10 tag=foo";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<PictureCommand>());

            var command = (PictureCommand)action.Create.Commands[0];
            Assert.That(command.Picture, Is.EqualTo("vmist.net/ptv/tv.php"));
            Assert.That(command.Update, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(command.Tag, Is.EqualTo("foo"));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

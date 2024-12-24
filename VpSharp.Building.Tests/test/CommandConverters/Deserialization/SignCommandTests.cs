using System.Drawing;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Deserialization;

internal sealed class SignCommandTests
{
    [Test]
    public void CreateSign_ShouldDeserialize_GivenNoArguments()
    {
        const string source = "create sign";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.Empty);
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignText()
    {
        const string source = "create sign \"Hello World\"";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndAlignment()
    {
        const string source = "create sign \"Hello World\" align=left";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Left));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndBackColor()
    {
        const string source = "create sign \"Hello World\" bcolor=red";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(Color.Red));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndForeColor()
    {
        const string source = "create sign \"Hello World\" color=red";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.Red));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndHorizontalMargin()
    {
        const string source = "create sign \"Hello World\" hmargin=2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.EqualTo(2));
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndMargin()
    {
        const string source = "create sign \"Hello World\" margin=2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.EqualTo(2));
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndShadowFlag()
    {
        const string source = "create sign \"Hello World\" shadow";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.True);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.Zero);
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }

    [Test]
    public void CreateSign_ShouldDeserialize_GivenSignTextAndVerticalMargin()
    {
        const string source = "create sign \"Hello World\" vmargin=2";
        VirtualParadiseAction action = ActionSerializer.Deserialize(source);

        Assert.Multiple(() =>
        {
            Assert.That(action.Triggers, Has.Count.EqualTo(1));
            Assert.That(action.Triggers[0], Is.InstanceOf<CreateTrigger>());
            Assert.That(action.Create.Commands, Has.Count.EqualTo(1));
            Assert.That(action.Create.Commands[0], Is.InstanceOf<SignCommand>());

            var command = (SignCommand)action.Create.Commands[0];
            Assert.That(command.Alignment, Is.EqualTo(TextAlignment.Center));
            Assert.That(command.BackColor, Is.EqualTo(VpColors.DefaultSignBackColor));
            Assert.That(command.ForeColor, Is.EqualTo(Color.White));
            Assert.That(command.HorizontalMargin, Is.Zero);
            Assert.That(command.Margin, Is.Zero);
            Assert.That(command.Shadow, Is.False);
            Assert.That(command.Text, Is.EqualTo("Hello World"));
            Assert.That(command.VerticalMargin, Is.EqualTo(2));
            Assert.That(command.ExecuteAs, Is.Null);
        });
    }
}

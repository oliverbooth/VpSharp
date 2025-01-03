using System.Drawing;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class SignCommandTests
{
    [Test]
    public void CreateSign_ShouldSerialize_GivenNoArguments()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand()
                    ]
                }
            ]
        };

        const string expected = "create sign";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignText()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World" }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\"";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndAlignment()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", Alignment = TextAlignment.Left }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" align=left";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndBackColor()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", BackColor = Color.Red }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" bcolor=red";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndForeColor()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", ForeColor = Color.Red }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" color=red";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndHorizontalMargin()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", HorizontalMargin = 2 }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" hmargin=2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndMargin()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", Margin = 2 }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" margin=2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndShadowFlag()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", Shadow = true }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" shadow";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSign_ShouldSerialize_GivenSignTextAndVerticalMargin()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new SignCommand { Text = "Hello World", VerticalMargin = 2 }
                    ]
                }
            ]
        };

        const string expected = "create sign \"Hello World\" vmargin=2";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

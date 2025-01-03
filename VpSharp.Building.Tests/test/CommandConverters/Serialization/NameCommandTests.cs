using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Tests.CommandConverters.Serialization;

internal sealed class NameCommandTests
{
    [Test]
    public void CreateName_ShouldSerialize_GivenNameFoo()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NameCommand { Name = "foo" }
                    ]
                }
            ]
        };

        const string expected = "create name foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateName_ShouldSerialize_GivenNameBar()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NameCommand { Name = "bar" }
                    ]
                }
            ]
        };

        const string expected = "create name bar";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateName_ShouldSerialize_GivenNameFooAndNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NameCommand { Name = "foo", ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create name foo name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateName_ShouldSerialize_GivenNameBarAndNameProperty()
    {
        var action = new VirtualParadiseAction
        {
            Triggers =
            [
                new CreateTrigger
                {
                    Commands =
                    [
                        new NameCommand { Name = "bar", ExecuteAs = Option.Some("foo") }
                    ]
                }
            ]
        };

        const string expected = "create name bar name=foo";
        string result = ActionSerializer.Serialize(action);

        Assert.That(result, Is.EqualTo(expected));
    }
}

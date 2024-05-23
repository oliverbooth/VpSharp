using System.Diagnostics;
using NUnit.Framework;

namespace VpSharp.Tests;

internal sealed class CoordinateTests
{
    [Test]
    public void TestAbsolute()
    {
        TestCoordinates("asdf", 0.0, 0.0, 0.0, 0.0, world: "asdf");
        TestCoordinates("1n 1w", 1.0, 0.0, 1.0);
        TestCoordinates("test 10n 5e 1a 123", -5, 1, 10, 123, world: "test");
        TestCoordinates("4s 6w -10a 45", 6, -10, -4, 45);
        TestCoordinates(" 100n 100w 1.5a 180", 100, 1.5, 100, 180);
        TestCoordinates("2355.71S 3429.68E -0.37a 0", -3429.68, -0.37, -2355.71);
    }

    [Test]
    public void TestRelative()
    {
        TestCoordinates("-1.1 +0 -1.2a", 0.0, -1.2, -1.1, 0.0, true);
        TestCoordinates("+0   +0 +5a", 0.0, 5.0, 0.0, 0.0, true);
        TestCoordinates("+1 +1 +1a", 1.0, 1.0, 1.0, 0.0, true);
    }

    [Test]
    public void ToString_ShouldReturnFormattedString_GivenCoordinates()
    {
        Coordinates coordinates = Coordinates.Parse("10n 5e 1a 123");
        string result = coordinates.ToString();
        
        Assert.That(result, Is.EqualTo("10.00n 5.00e 1.00a 123.00"));
    }

    [Test]
    public void ToString_ShouldReturnFormattedString_GivenArguments()
    {
        Coordinates coordinates = Coordinates.Parse("10n 5e 1a 123");
        string result = coordinates.ToString("0");
        
        Assert.That(result, Is.EqualTo("10n 5e 1a 123"));
    }

    private static void TestCoordinates(
        string input,
        double x,
        double y,
        double z,
        double yaw = 0.0,
        bool isRelative = false,
        string? world = null
    )
    {
        Coordinates coordinates = Coordinates.Parse(input);

        Trace.WriteLine("----");
        Trace.WriteLine($"Input: {input}");
        Trace.WriteLine($"Parsed: {coordinates}");
        Trace.WriteLine("----");

        Assert.Multiple(() =>
        {
            Assert.That(coordinates.X, Is.EqualTo(x));
            Assert.That(coordinates.Y, Is.EqualTo(y));
            Assert.That(coordinates.Z, Is.EqualTo(z));
            Assert.That(coordinates.Yaw, Is.EqualTo(yaw));
            Assert.That(coordinates.IsRelative, Is.EqualTo(isRelative));
        });
        
        if (string.IsNullOrWhiteSpace(world))
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(coordinates.World));
        }
        else
        {
            Assert.That(coordinates.World, Is.EqualTo(world));
        }
    }
}

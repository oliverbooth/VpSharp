using System.Diagnostics;

namespace VpSharp.Tests;

[TestClass]
public sealed class CoordinateTests
{
    [TestMethod]
    public void TestAbsolute()
    {
        TestCoordinates("asdf", 0.0, 0.0, 0.0, 0.0, world: "asdf");
        TestCoordinates("1n 1w", 1.0, 0.0, 1.0);
        TestCoordinates("test 10n 5e 1a 123", -5, 1, 10, 123, world: "test");
        TestCoordinates("4s 6w -10a 45", 6, -10, -4, 45);
        TestCoordinates(" 100n 100w 1.5a 180", 100, 1.5, 100, 180);
        TestCoordinates("2355.71S 3429.68E -0.37a 0", -3429.68, -0.37, -2355.71);
    }

    [TestMethod]
    public void TestRelative()
    {
        TestCoordinates("-1.1 +0 -1.2a", 0.0, -1.2, -1.1, 0.0, true);
        TestCoordinates("+0   +0 +5a", 0.0, 5.0, 0.0, 0.0, true);
        TestCoordinates("+1 +1 +1a", 1.0, 1.0, 1.0, 0.0, true);
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

        Assert.AreEqual(x, coordinates.X);
        Assert.AreEqual(y, coordinates.Y);
        Assert.AreEqual(z, coordinates.Z);
        Assert.AreEqual(yaw, coordinates.Yaw);
        Assert.AreEqual(isRelative, coordinates.IsRelative);
        if (string.IsNullOrWhiteSpace(world))
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(coordinates.World));
        }
        else
        {
            Assert.AreEqual(world, coordinates.World);
        }
    }
}

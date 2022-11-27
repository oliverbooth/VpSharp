using System.Drawing;

namespace VpSharp.Tests;

[TestClass]
public class ColorFTests
{
    [TestMethod]
    public void Black_ToVpColorF_ShouldGive0ForProperties()
    {
        ColorF color = Color.Black;
        Assert.AreEqual(color.A, 1.0f, float.Epsilon);
        Assert.AreEqual(color.R, 0.0f, float.Epsilon);
        Assert.AreEqual(color.G, 0.0f, float.Epsilon);
        Assert.AreEqual(color.B, 0.0f, float.Epsilon);
    }

    [TestMethod]
    public void Transparent_ToVpColorF_ShouldGive0ForAlpha()
    {
        ColorF color = Color.Transparent;
        Assert.AreEqual(color.A, 0.0f, float.Epsilon);
    }

    [TestMethod]
    public void White_ToVpColorF_ShouldGive1ForProperties()
    {
        ColorF color = Color.White;
        Assert.AreEqual(color.A, 1.0f, float.Epsilon);
        Assert.AreEqual(color.R, 1.0f, float.Epsilon);
        Assert.AreEqual(color.G, 1.0f, float.Epsilon);
        Assert.AreEqual(color.B, 1.0f, float.Epsilon);
    }
}

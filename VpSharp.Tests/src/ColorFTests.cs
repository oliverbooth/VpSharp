using System.Drawing;
using NUnit.Framework;

namespace VpSharp.Tests;

internal sealed class ColorFTests
{
    [Test]
    public void Black_ToVpColorF_ShouldGive0ForProperties()
    {
        ColorF color = Color.Black;
        Assert.Multiple(() =>
        {
            Assert.That(color.A, Is.EqualTo(1.0f).Within(float.Epsilon));
            Assert.That(color.R, Is.EqualTo(0.0f).Within(float.Epsilon));
            Assert.That(color.G, Is.EqualTo(0.0f).Within(float.Epsilon));
            Assert.That(color.B, Is.EqualTo(0.0f).Within(float.Epsilon));
        });
    }

    [Test]
    public void Transparent_ToVpColorF_ShouldGive0ForAlpha()
    {
        ColorF color = Color.Transparent;
        Assert.That(color.A, Is.EqualTo(0.0f).Within(float.Epsilon));
    }

    [Test]
    public void White_ToVpColorF_ShouldGive1ForProperties()
    {
        ColorF color = Color.White;
        Assert.Multiple(() =>
        {
            Assert.That(color.A, Is.EqualTo(1.0f).Within(float.Epsilon));
            Assert.That(color.R, Is.EqualTo(1.0f).Within(float.Epsilon));
            Assert.That(color.G, Is.EqualTo(1.0f).Within(float.Epsilon));
            Assert.That(color.B, Is.EqualTo(1.0f).Within(float.Epsilon));
        });
    }
}

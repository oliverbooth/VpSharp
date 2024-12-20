using System.Text;

namespace VpSharp.Building.Tests;

internal sealed class Utf8ActionReaderTests
{
    [Test]
    public void Read_ShouldReadCreateSign_GivenSpan()
    {
        const string source = "create sign \"Hello World\" bcolor=red";

        int byteCount = Encoding.UTF8.GetByteCount(source);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.UTF8.GetBytes(source, bytes);
        var reader = new Utf8ActionReader(bytes);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("sign"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.String));
        Assert.That(token.Value, Is.EqualTo("Hello World"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.PropertyName));
        Assert.That(token.Value, Is.EqualTo("bcolor"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("red"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }

    [Test]
    public void Read_ShouldReadCreateSign_GivenStream()
    {
        const string source = "create sign \"Hello World\" bcolor=red";

        byte[] bytes = Encoding.UTF8.GetBytes(source);
        using var stream = new MemoryStream(bytes);
        var reader = new Utf8ActionReader(stream);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("sign"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.String));
        Assert.That(token.Value, Is.EqualTo("Hello World"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.PropertyName));
        Assert.That(token.Value, Is.EqualTo("bcolor"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("red"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }

    [Test]
    public void Read_ShouldReadCreateVisibleOff_GivenSpan()
    {
        const string source = "create visible off";

        int byteCount = Encoding.UTF8.GetByteCount(source);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.UTF8.GetBytes(source, bytes);
        var reader = new Utf8ActionReader(bytes);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("visible"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("off"));
        Assert.That(reader.GetBoolean(), Is.False);

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }

    [Test]
    public void Read_ShouldReadCreateVisibleOff_GivenStream()
    {
        const string source = "create visible off";

        byte[] bytes = Encoding.UTF8.GetBytes(source);
        using var stream = new MemoryStream(bytes);
        var reader = new Utf8ActionReader(stream);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("visible"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("off"));
        Assert.That(reader.GetBoolean(), Is.False);

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }

    [Test]
    public void Read_ShouldReadCreateVisibleOnWithNameProperty_GivenSpan()
    {
        const string source = "create visible on name=foo";

        int byteCount = Encoding.UTF8.GetByteCount(source);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.UTF8.GetBytes(source, bytes);
        var reader = new Utf8ActionReader(bytes);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("visible"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("on"));
        Assert.That(reader.GetBoolean(), Is.True);

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.PropertyName));
        Assert.That(token.Value, Is.EqualTo("name"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("foo"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }

    [Test]
    public void Read_ShouldReadCreateVisibleOnWithNameProperty_GivenStream()
    {
        const string source = "create visible on name=foo";

        byte[] bytes = Encoding.UTF8.GetBytes(source);
        using var stream = new MemoryStream(bytes);
        var reader = new Utf8ActionReader(stream);

        Token token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("create"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("visible"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("on"));
        Assert.That(reader.GetBoolean(), Is.True);

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.PropertyName));
        Assert.That(token.Value, Is.EqualTo("name"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.Text));
        Assert.That(token.Value, Is.EqualTo("foo"));

        token = reader.Read();
        Assert.That(token.Type, Is.EqualTo(TokenType.None));
        Assert.That(token.Value, Is.Null);
    }
}

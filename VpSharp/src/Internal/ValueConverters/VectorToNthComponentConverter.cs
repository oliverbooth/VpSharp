using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

internal sealed class VectorToNthComponentConverter : ValueConverter<float>
{
    private readonly int _componentNumber;

    /// <inheritdoc />
    public VectorToNthComponentConverter(int componentNumber)
    {
        _componentNumber = componentNumber;
    }

    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out float result)
    {
        using var builder = new Utf8ValueStringBuilder(false);
        var spaceCount = 0;

        while (true)
        {
            int readChar = reader.Read();

            if (readChar == -1)
                break;

            var currentChar = (char) readChar;
            if (currentChar == ' ')
                spaceCount++;
            else if (spaceCount == _componentNumber - 1)
                builder.Append(currentChar);
        }

        result = builder.AsSpan().ToSingle();
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, float value)
    {
        writer.Write(value);
    }
}
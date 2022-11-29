namespace VpSharp.Internal.ValueConverters;

internal sealed class UriConverter : ValueConverter<Uri>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out Uri result)
    {
        string url = reader.ReadToEnd();
        result = string.IsNullOrWhiteSpace(url) ? null : new Uri(url);
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, Uri value)
    {
        if (value is not null)
        {
            writer.Write(value.ToString());
        }
    }
}

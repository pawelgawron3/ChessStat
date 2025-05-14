using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChessAPI.Helpers;

public class UnixDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long timestamp = reader.GetInt64();
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        long unixTime;
        try
        {
            unixTime = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
        catch
        {
            unixTime=default;
        }
        writer.WriteNumberValue(unixTime);
    }
}
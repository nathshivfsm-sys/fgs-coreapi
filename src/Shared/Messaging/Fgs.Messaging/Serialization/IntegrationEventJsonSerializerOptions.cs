using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fgs.Messaging.Serialization;

public static class IntegrationEventJsonSerializerOptions
{
    public static JsonSerializerOptions Create() =>
        new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new FlexibleInt64JsonConverter(),
                new FlexibleNullableInt64JsonConverter()
            }
        };
}

/// <summary>
/// Accepts numeric ids and numeric strings.
/// </summary>
public sealed class FlexibleInt64JsonConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.GetInt64();
            case JsonTokenType.String:
                return FlexibleInt64Parser.Parse(reader.GetString());
            default:
                throw new JsonException($"Unexpected token '{reader.TokenType}' when parsing a numeric id.");
        }
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}

public sealed class FlexibleNullableInt64JsonConverter : JsonConverter<long?>
{
    public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.GetInt64();
            case JsonTokenType.String:
                return FlexibleInt64Parser.Parse(reader.GetString());
            default:
                throw new JsonException($"Unexpected token '{reader.TokenType}' when parsing a nullable numeric id.");
        }
    }

    public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

internal static class FlexibleInt64Parser
{
    public static long Parse(string? value)
    {
        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        throw new JsonException($"Value '{value}' is not an integer.");
    }
}

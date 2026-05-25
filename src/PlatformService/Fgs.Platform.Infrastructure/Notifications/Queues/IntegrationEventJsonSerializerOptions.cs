using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fgs.Platform.Infrastructure.Notifications.Queues;

internal static class IntegrationEventJsonSerializerOptions
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
/// Accepts numeric ids and numeric strings. Non-numeric strings (e.g. legacy GUID company ids) map to 0.
/// </summary>
internal sealed class FlexibleInt64JsonConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.GetInt64();
            case JsonTokenType.String:
                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return 0;
                }

                return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
                    ? parsed
                    : 0;
            default:
                throw new JsonException($"Unexpected token '{reader.TokenType}' when parsing a numeric id.");
        }
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}

internal sealed class FlexibleNullableInt64JsonConverter : JsonConverter<long?>
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
                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
                    ? parsed
                    : null;
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

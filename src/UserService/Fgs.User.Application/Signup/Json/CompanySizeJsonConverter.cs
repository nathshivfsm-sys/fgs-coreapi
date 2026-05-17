using System.Text.Json;
using System.Text.Json.Serialization;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Signup.Json;

/// <summary>
/// Accepts company size as enum name, camelCase name, integer, or onboarding UI labels.
/// </summary>
public sealed class CompanySizeJsonConverter : JsonConverter<CompanySize>
{
    private static readonly Dictionary<string, CompanySize> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["1"] = CompanySize.SingleOwner,
        ["SingleOwner"] = CompanySize.SingleOwner,
        ["singleOwner"] = CompanySize.SingleOwner,
        ["Single Owner"] = CompanySize.SingleOwner,
        ["SINGLE_OWNER"] = CompanySize.SingleOwner,

        ["2"] = CompanySize.TwoToFive,
        ["TwoToFive"] = CompanySize.TwoToFive,
        ["twoToFive"] = CompanySize.TwoToFive,
        ["2-5"] = CompanySize.TwoToFive,
        ["2-5 employees"] = CompanySize.TwoToFive,
        ["2 to 5"] = CompanySize.TwoToFive,
        ["2 to 5 employees"] = CompanySize.TwoToFive,

        ["3"] = CompanySize.SixToTen,
        ["SixToTen"] = CompanySize.SixToTen,
        ["sixToTen"] = CompanySize.SixToTen,
        ["6-10"] = CompanySize.SixToTen,
        ["6-10 employees"] = CompanySize.SixToTen,
        ["6 to 10"] = CompanySize.SixToTen,
        ["6 to 10 employees"] = CompanySize.SixToTen,

        ["4"] = CompanySize.ElevenPlus,
        ["ElevenPlus"] = CompanySize.ElevenPlus,
        ["elevenPlus"] = CompanySize.ElevenPlus,
        ["11+"] = CompanySize.ElevenPlus,
        ["11+ employees"] = CompanySize.ElevenPlus,
        ["11 or more"] = CompanySize.ElevenPlus,
        ["11 or more employees"] = CompanySize.ElevenPlus
    };

    public override CompanySize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt32(out var numeric)
                => Enum.IsDefined(typeof(CompanySize), numeric)
                    ? (CompanySize)numeric
                    : throw new JsonException($"Invalid company size value '{numeric}'."),
            JsonTokenType.String => ParseString(reader.GetString()),
            _ => throw new JsonException("Company size must be a string or number.")
        };
    }

    public override void Write(Utf8JsonWriter writer, CompanySize value, JsonSerializerOptions options) =>
        writer.WriteStringValue(JsonNamingPolicy.CamelCase.ConvertName(value.ToString()));

    private static CompanySize ParseString(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new JsonException("Company size is required.");
        }

        var trimmed = raw.Trim();
        if (Aliases.TryGetValue(trimmed, out var size))
        {
            return size;
        }

        if (Enum.TryParse<CompanySize>(trimmed, ignoreCase: true, out var parsed)
            && Enum.IsDefined(parsed))
        {
            return parsed;
        }

        throw new JsonException(
            $"Invalid company size '{raw}'. Use SingleOwner, TwoToFive, SixToTen, ElevenPlus, or UI labels such as '2-5 employees'.");
    }
}

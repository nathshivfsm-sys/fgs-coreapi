using System.Globalization;
using System.Text.Json;
using Fgs.Bff.Application.Features.Lookups.Dtos;

namespace Fgs.Bff.Application.Features.Lookups;

/// <summary>
/// Maps heterogeneous owning-service lookup JSON objects into the BFF <see cref="LookupItemDto"/> shape.
/// </summary>
public static class LookupItemMapper
{
    private static readonly string[] IdNames =
    [
        "id", "Id", "eventTypeId", "EventTypeId"
    ];

    private static readonly string[] CodeNames =
    [
        "code", "Code",
        "countryCode", "CountryCode",
        "languageCode", "LanguageCode",
        "timeZoneCode", "TimeZoneCode",
        "unitCode", "UnitCode",
        "stateProvinceCode", "StateProvinceCode",
        "maintenanceTypeCode", "MaintenanceTypeCode"
    ];

    private static readonly string[] NameNames =
    [
        "name", "Name",
        "countryName", "CountryName",
        "languageName", "LanguageName",
        "stateProvinceName", "StateProvinceName",
        "city", "City"
    ];

    private static readonly string[] DisplayOrderNames =
    [
        "displayOrder", "DisplayOrder"
    ];

    private static readonly HashSet<string> ClaimedNames = new(StringComparer.OrdinalIgnoreCase);

    static LookupItemMapper()
    {
        foreach (var name in IdNames.Concat(CodeNames).Concat(NameNames).Concat(DisplayOrderNames))
        {
            ClaimedNames.Add(name);
        }
    }

    public static LookupItemDto Map(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return new LookupItemDto(null, null, element.ToString(), null, null);
        }

        var id = FirstString(element, IdNames);
        var code = FirstString(element, CodeNames);
        var name = FirstString(element, NameNames);
        var displayOrder = FirstInt(element, DisplayOrderNames);

        // Country/language style rows often use code as the primary key.
        if (id is null && code is not null)
        {
            id = code;
        }

        Dictionary<string, object?>? extra = null;
        foreach (var property in element.EnumerateObject())
        {
            if (ClaimedNames.Contains(property.Name))
            {
                continue;
            }

            extra ??= new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            extra[property.Name] = ToObject(property.Value);
        }

        return new LookupItemDto(id, code, name, displayOrder, extra);
    }

    private static object? ToObject(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            // Keep nested JSON as text so HotChocolate AnyType can serialize Extra safely.
            JsonValueKind.Array or JsonValueKind.Object => element.GetRawText(),
            _ => element.GetRawText()
        };

    public static IReadOnlyList<LookupItemDto> MapArray(JsonElement data)
    {
        if (data.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var items = new List<LookupItemDto>(data.GetArrayLength());
        foreach (var element in data.EnumerateArray())
        {
            items.Add(Map(element));
        }

        return items;
    }

    private static string? FirstString(JsonElement element, IEnumerable<string> names)
    {
        foreach (var name in names)
        {
            if (!element.TryGetProperty(name, out var property)
                || property.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                continue;
            }

            return property.ValueKind switch
            {
                JsonValueKind.String => property.GetString(),
                JsonValueKind.Number => property.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => property.ToString()
            };
        }

        return null;
    }

    private static int? FirstInt(JsonElement element, IEnumerable<string> names)
    {
        foreach (var name in names)
        {
            if (!element.TryGetProperty(name, out var property)
                || property.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                continue;
            }

            if (property.ValueKind == JsonValueKind.Number && property.TryGetInt32(out var number))
            {
                return number;
            }

            if (property.ValueKind == JsonValueKind.String
                && int.TryParse(property.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
        }

        return null;
    }
}

using System.Globalization;
using Fgs.Bff.Application.Features.Lookups.Dtos;

namespace Fgs.Bff.Application.Features.Lookups;

public static class LookupQueryStringBuilder
{
    public static string Build(LookupRequestDto request, LookupDefinition definition)
    {
        var pairs = new List<string>();

        // activeOnly when the route documents it (required or optional catalog entry).
        if (definition.OptionalFilters.Contains(LookupFilterNames.ActiveOnly, StringComparer.Ordinal)
            || definition.RequiredFilters.Contains(LookupFilterNames.ActiveOnly, StringComparer.Ordinal))
        {
            Add(pairs, LookupFilterNames.ActiveOnly, request.ActiveOnly ? "true" : "false");
        }

        AddIf(pairs, LookupFilterNames.CountryCode, request.CountryCode);
        AddIf(pairs, LookupFilterNames.UnitType, request.UnitType);
        AddIf(pairs, LookupFilterNames.SourceTypeId, request.SourceTypeId);
        AddIf(pairs, LookupFilterNames.JobTypeId, request.JobTypeId);
        AddIf(pairs, LookupFilterNames.PriceBookId, request.PriceBookId);
        AddIf(pairs, LookupFilterNames.PricingMatrixId, request.PricingMatrixId);
        AddIf(pairs, LookupFilterNames.PricingMatrixLaborId, request.PricingMatrixLaborId);
        AddIf(pairs, LookupFilterNames.UniversalPricingServiceId, request.UniversalPricingServiceId);
        AddIf(pairs, LookupFilterNames.InventoryItemId, request.InventoryItemId);
        AddIf(pairs, LookupFilterNames.FgsRoleId, request.FgsRoleId);
        AddIf(pairs, LookupFilterNames.RoleId, request.RoleId);
        AddIf(pairs, LookupFilterNames.UserId, request.UserId?.ToString("D"));
        AddIf(pairs, LookupFilterNames.ShowToFieldTech, FormatBool(request.ShowToFieldTech));
        AddIf(pairs, LookupFilterNames.AllowToPick, FormatBool(request.AllowToPick));
        AddIf(pairs, LookupFilterNames.IsMobileVisible, FormatBool(request.IsMobileVisible));
        AddIf(pairs, LookupFilterNames.IsCustomerPortalVisible, FormatBool(request.IsCustomerPortalVisible));
        AddIf(pairs, LookupFilterNames.StateProvinceCode, request.StateProvinceCode);

        return pairs.Count == 0 ? string.Empty : "?" + string.Join("&", pairs);
    }

    private static string? FormatBool(bool? value) =>
        value.HasValue ? (value.Value ? "true" : "false") : null;

    private static void AddIf(List<string> pairs, string name, long? value)
    {
        if (value.HasValue)
        {
            Add(pairs, name, value.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddIf(List<string> pairs, string name, int? value)
    {
        if (value.HasValue)
        {
            Add(pairs, name, value.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddIf(List<string> pairs, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Add(pairs, name, value);
        }
    }

    private static void Add(List<string> pairs, string name, string value) =>
        pairs.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value)}");
}

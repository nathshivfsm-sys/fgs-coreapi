namespace Fgs.Bff.Application.Features.Lookups;

/// <summary>Canonical query-string filter names forwarded to owning services.</summary>
public static class LookupFilterNames
{
    public const string ActiveOnly = "activeOnly";
    public const string CountryCode = "countryCode";
    public const string UnitType = "unitType";
    public const string SourceTypeId = "sourceTypeId";
    public const string JobTypeId = "jobTypeId";
    public const string PriceBookId = "priceBookId";
    public const string PricingMatrixId = "pricingMatrixId";
    public const string PricingMatrixLaborId = "pricingMatrixLaborId";
    public const string UniversalPricingServiceId = "universalPricingServiceId";
    public const string InventoryItemId = "inventoryItemId";
    public const string FgsRoleId = "fgsRoleId";
    public const string RoleId = "roleId";
    public const string UserId = "userId";
    public const string ShowToFieldTech = "showToFieldTech";
    public const string AllowToPick = "allowToPick";
    public const string IsMobileVisible = "isMobileVisible";
    public const string IsCustomerPortalVisible = "isCustomerPortalVisible";
    public const string StateProvinceCode = "stateProvinceCode";
}

/// <summary>Named HttpClient / catalog service identifiers.</summary>
public static class LookupServiceNames
{
    public const string Setup = "Setup";
    public const string User = "User";
    public const string Asset = "Asset";
    public const string Inventory = "Inventory";
    public const string Billing = "Billing";
    public const string Crm = "Crm";
}

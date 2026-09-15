namespace Fgs.Bff.Application.Features.Lookups;

/// <summary>
/// Catalog of every owning-service lookup route orchestrated by BFF batch GraphQL.
/// </summary>
public static class LookupCatalog
{
    private static readonly IReadOnlyList<string> NoFilters = [];
    private static readonly IReadOnlyList<string> ActiveOnlyOnly = [LookupFilterNames.ActiveOnly];

    private static readonly LookupDefinition[] Definitions =
    [
        // Glo
        Def(LookupKey.GloCountry, LookupServiceNames.Setup, "api/v1/glo/country/lookup", false, NoFilters, ActiveOnlyOnly, "Global countries"),
        Def(LookupKey.GloStateProvince, LookupServiceNames.Setup, "api/v1/glo/stateprovince/lookup", false, [LookupFilterNames.CountryCode], ActiveOnlyOnly, "States/provinces for a country"),
        Def(LookupKey.GloTimeZone, LookupServiceNames.Setup, "api/v1/glo/timezone/lookup", false, NoFilters, ActiveOnlyOnly, "Global time zones"),
        Def(LookupKey.GloLanguage, LookupServiceNames.Setup, "api/v1/glo/language/lookup", false, NoFilters, ActiveOnlyOnly, "Global languages"),
        Def(LookupKey.GloLocationType, LookupServiceNames.Setup, "api/v1/glo/locationtype/lookup", false, NoFilters, ActiveOnlyOnly, "Global location types"),
        Def(LookupKey.GloBusinessType, LookupServiceNames.Setup, "api/v1/glo/businesstype/lookup", false, NoFilters, ActiveOnlyOnly, "Global business types"),
        Def(LookupKey.GloSetupDescriptionType, LookupServiceNames.Setup, "api/v1/glo/setupdescriptiontype/lookup", false, NoFilters, ActiveOnlyOnly, "Global setup description types"),
        Def(LookupKey.GloMasterEntityType, LookupServiceNames.Setup, "api/v1/glo/masterentitytype/lookup", false, NoFilters, ActiveOnlyOnly, "Global master entity types"),
        Def(LookupKey.GloUnitOfMeasure, LookupServiceNames.Setup, "api/v1/glo/unitofmeasure/lookup", false, NoFilters, [LookupFilterNames.UnitType, LookupFilterNames.ActiveOnly], "Global units of measure"),
        Def(LookupKey.GloInventoryTransactionSource, LookupServiceNames.Setup, "api/v1/glo/inventorytransactionsource/lookup", false, NoFilters, ActiveOnlyOnly, "Inventory transaction source types"),
        Def(LookupKey.GloInventoryTransactionType, LookupServiceNames.Setup, "api/v1/glo/inventorytransactiontype/lookup", false, NoFilters, [LookupFilterNames.SourceTypeId, LookupFilterNames.ActiveOnly], "Inventory transaction types"),
        Def(LookupKey.GloAccountingIntegrationType, LookupServiceNames.Setup, "api/v1/glo/accountingintegrationtype/lookup", false, NoFilters, ActiveOnlyOnly, "Accounting integration types"),
        Def(LookupKey.GloAppointmentAssignmentEventType, LookupServiceNames.Setup, "api/v1/glo/appointmentassignmenteventtype/lookup", false, NoFilters, ActiveOnlyOnly, "Appointment assignment event types"),
        Def(LookupKey.GloVehicleMaintenanceType, LookupServiceNames.Setup, "api/v1/glo/vehiclemaintenancetype/lookup", false, NoFilters, ActiveOnlyOnly, "Vehicle maintenance types"),
        Def(LookupKey.GloSetupTenantStatus, LookupServiceNames.Setup, "api/v1/glo/setuptenantstatus/lookup", false, NoFilters, ActiveOnlyOnly, "Setup tenant statuses"),

        // Setup tenant
        Setup("techtrade", LookupKey.TechTrade, "Tech trades"),
        Setup("jobcategory", LookupKey.JobCategory, "Job categories"),
        Setup("jobtype", LookupKey.JobType, "Job types"),
        Def(LookupKey.JobTypeCategory, LookupServiceNames.Setup, "api/v1/jobtypecategory/lookup", true, NoFilters, [LookupFilterNames.JobTypeId, LookupFilterNames.ActiveOnly], "Job type categories"),
        Setup("jobtypetask", LookupKey.JobTypeTask, "Job type tasks"),
        Def(LookupKey.BillingCategory, LookupServiceNames.Setup, "api/v1/billingcategory/lookup", true, NoFilters, [LookupFilterNames.ShowToFieldTech, LookupFilterNames.AllowToPick, LookupFilterNames.ActiveOnly], "Billing categories"),
        Setup("pricebook", LookupKey.PriceBook, "Price books"),
        Def(LookupKey.PriceBookItem, LookupServiceNames.Setup, "api/v1/pricebookitem/lookup", true, NoFilters, [LookupFilterNames.PriceBookId, LookupFilterNames.ActiveOnly], "Price book items"),
        Setup("pricingmatrix", LookupKey.PricingMatrix, "Pricing matrices"),
        Def(LookupKey.PricingMatrixLabor, LookupServiceNames.Setup, "api/v1/pricingmatrixlabor/lookup", true, NoFilters, [LookupFilterNames.PricingMatrixId, LookupFilterNames.ActiveOnly], "Pricing matrix labor rows"),
        Def(LookupKey.PricingMatrixLaborTier, LookupServiceNames.Setup, "api/v1/pricingmatrixlabortier/lookup", true, NoFilters, [LookupFilterNames.PricingMatrixLaborId, LookupFilterNames.ActiveOnly], "Pricing matrix labor tiers"),
        Def(LookupKey.PricingMatrixMaterialTier, LookupServiceNames.Setup, "api/v1/pricingmatrixmaterialtier/lookup", true, NoFilters, [LookupFilterNames.PricingMatrixId, LookupFilterNames.ActiveOnly], "Pricing matrix material tiers"),
        Def(LookupKey.PricingMatrixOther, LookupServiceNames.Setup, "api/v1/pricingmatrixother/lookup", true, NoFilters, [LookupFilterNames.PricingMatrixId, LookupFilterNames.ActiveOnly], "Pricing matrix other rows"),
        Setup("universalpricingservice", LookupKey.UniversalPricingService, "Universal pricing services"),
        Def(LookupKey.UniversalMatrixItem, LookupServiceNames.Setup, "api/v1/universalmatrixitem/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix items"),
        Def(LookupKey.UniversalMatrixTier, LookupServiceNames.Setup, "api/v1/universalmatrixtier/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix tiers"),
        Def(LookupKey.UniversalMatrixAddOn, LookupServiceNames.Setup, "api/v1/universalmatrixaddon/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix add-ons"),
        Def(LookupKey.UniversalMatrixOneTimeFee, LookupServiceNames.Setup, "api/v1/universalmatrixonetimefee/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix one-time fees"),
        Def(LookupKey.UniversalMatrixSizeTier, LookupServiceNames.Setup, "api/v1/universalmatrixsizetier/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix size tiers"),
        Def(LookupKey.UniversalMatrixFrequencyDiscount, LookupServiceNames.Setup, "api/v1/universalmatrixfrequencydiscount/lookup", true, NoFilters, [LookupFilterNames.UniversalPricingServiceId, LookupFilterNames.ActiveOnly], "Universal matrix frequency discounts"),
        Def(LookupKey.PaymentMethod, LookupServiceNames.Setup, "api/v1/paymentmethod/lookup", true, NoFilters, [LookupFilterNames.IsMobileVisible, LookupFilterNames.IsCustomerPortalVisible, LookupFilterNames.ActiveOnly], "Payment methods"),
        Setup("paymentterm", LookupKey.PaymentTerm, "Payment terms"),
        Setup("tax", LookupKey.Tax, "Taxes"),
        Setup("taxauthority", LookupKey.TaxAuthority, "Tax authorities"),
        Setup("laborratetype", LookupKey.LaborRateType, "Labor rate types"),
        Setup("techskilllevel", LookupKey.TechSkillLevel, "Tech skill levels"),
        Def(LookupKey.ResolutionCode, LookupServiceNames.Setup, "api/v1/resolutioncode/lookup", true, NoFilters, [LookupFilterNames.IsMobileVisible, LookupFilterNames.ActiveOnly], "Resolution codes"),
        Def(LookupKey.Timeslot, LookupServiceNames.Setup, "api/v1/timeslot/lookup", true, NoFilters, [LookupFilterNames.IsMobileVisible, LookupFilterNames.IsCustomerPortalVisible, LookupFilterNames.ActiveOnly], "Time slots"),
        Setup("nonworkingdate", LookupKey.NonWorkingDate, "Non-working dates"),
        Setup("zone", LookupKey.Zone, "Zones"),
        Setup("tag", LookupKey.Tag, "Tags"),
        Setup("titleofcourtesy", LookupKey.TitleOfCourtesy, "Titles of courtesy"),
        Setup("setupdescription", LookupKey.SetupDescription, "Setup descriptions"),
        Setup("businesstype", LookupKey.BusinessType, "Tenant business types"),
        Setup("termscondition", LookupKey.TermsCondition, "Terms and conditions"),
        Setup("entitydefaulttermscondition", LookupKey.EntityDefaultTermsCondition, "Entity default terms"),
        Setup("communicationtemplate", LookupKey.CommunicationTemplate, "Communication templates"),
        Setup("employee", LookupKey.Employee, "Employees"),
        Setup("vehicle", LookupKey.Vehicle, "Vehicles"),
        Setup("vehiclemaintenance", LookupKey.VehicleMaintenance, "Vehicle maintenances"),
        Setup("postalcode", LookupKey.PostalCode, "Postal codes"),
        Def(LookupKey.PostalCodeCity, LookupServiceNames.Setup, "api/v1/postalcode/cities", true, NoFilters, [LookupFilterNames.CountryCode, LookupFilterNames.StateProvinceCode, LookupFilterNames.ActiveOnly], "Distinct cities from postal codes"),
        Setup("leadsource", LookupKey.LeadSource, "Lead sources"),
        Setup("leadstatus", LookupKey.LeadStatus, "Lead statuses"),
        Setup("leaddisqualificationreason", LookupKey.LeadDisqualificationReason, "Lead disqualification reasons"),
        Setup("salesactivitytype", LookupKey.SalesActivityType, "Sales activity types"),
        Setup("salesactivityoutcome", LookupKey.SalesActivityOutcome, "Sales activity outcomes"),
        Setup("salespipelinestatus", LookupKey.SalesPipelineStatus, "Sales pipeline statuses"),
        Setup("salesdispositionreason", LookupKey.SalesDispositionReason, "Sales disposition reasons"),
        Setup("glbreak", LookupKey.GlBreak, "GL breaks"),

        // User
        User("role", LookupKey.Role, "Roles"),
        User("permission", LookupKey.Permission, "Permissions"),
        User("dataaccess", LookupKey.DataAccess, "Data access scopes"),
        Def(LookupKey.RolePermission, LookupServiceNames.User, "api/v1/rolepermission/lookup", true, [LookupFilterNames.FgsRoleId], NoFilters, "Permissions assigned to a role"),
        Def(LookupKey.RoleDataAccess, LookupServiceNames.User, "api/v1/roledataaccess/lookup", true, [LookupFilterNames.FgsRoleId], NoFilters, "Data access assigned to a role"),
        Def(LookupKey.RoleMenu, LookupServiceNames.User, "api/v1/rolemenu/lookup", true, [LookupFilterNames.RoleId], ActiveOnlyOnly, "Menus assigned to a role"),
        Def(LookupKey.UserRole, LookupServiceNames.User, "api/v1/userrole/lookup", true, [LookupFilterNames.UserId], NoFilters, "Roles assigned to a user"),
        User("tenantmenu", LookupKey.TenantMenu, "Tenant menus"),
        User("apiclient", LookupKey.ApiClient, "API clients"),
        User("apiwebhook", LookupKey.ApiWebhook, "API webhooks"),
        User("apievent", LookupKey.ApiEvent, "API events"),
        User("publicendpoint", LookupKey.PublicEndpoint, "Public endpoints"),

        // Asset
        Asset("asset", LookupKey.Asset, "Assets"),
        Asset("assettype", LookupKey.AssetType, "Asset types"),
        Asset("assetstatus", LookupKey.AssetStatus, "Asset statuses"),
        Asset("assetmanufacturer", LookupKey.AssetManufacturer, "Asset manufacturers"),
        Asset("assetmodel", LookupKey.AssetModel, "Asset models"),
        Asset("assetattribute", LookupKey.AssetAttribute, "Asset attributes"),
        Asset("assetattributeoption", LookupKey.AssetAttributeOption, "Asset attribute options"),
        Asset("assetattributevalue", LookupKey.AssetAttributeValue, "Asset attribute values"),
        Def(LookupKey.AssetWarranty, LookupServiceNames.Asset, "api/v1/assetwarranty/lookup", true, NoFilters, NoFilters, "Asset warranties"),

        // Inventory
        Inventory("inventoryitem", LookupKey.InventoryItem, "Inventory items"),
        Inventory("inventoryitemtype", LookupKey.InventoryItemType, "Inventory item types"),
        Inventory("inventorycategory", LookupKey.InventoryCategory, "Inventory categories"),
        Inventory("inventorysubcategory", LookupKey.InventorySubCategory, "Inventory sub-categories"),
        Inventory("inventorylocation", LookupKey.InventoryLocation, "Inventory locations"),
        Def(LookupKey.InventorySerial, LookupServiceNames.Inventory, "api/v1/inventoryserial/lookup", true, NoFilters, [LookupFilterNames.InventoryItemId], "Inventory serials"),
        Inventory("vendor", LookupKey.Vendor, "Vendors"),
        Inventory("vendorinventoryitem", LookupKey.VendorInventoryItem, "Vendor inventory items"),
        Inventory("truckstocktemplate", LookupKey.TruckStockTemplate, "Truck stock templates"),

        // Billing / CRM
        Def(LookupKey.Invoice, LookupServiceNames.Billing, "api/v1/invoice/lookup", true, NoFilters, NoFilters, "Invoices"),
        Def(LookupKey.Customer, LookupServiceNames.Crm, "api/v1/customer/lookup", true, NoFilters, ActiveOnlyOnly, "Customers")
    ];

    private static readonly IReadOnlyDictionary<LookupKey, LookupDefinition> ByKey =
        Definitions.ToDictionary(d => d.Key);

    public static IReadOnlyList<LookupDefinition> All { get; } = Definitions;

    public static LookupDefinition Get(LookupKey key) => ByKey[key];

    public static bool TryGet(LookupKey key, out LookupDefinition definition) =>
        ByKey.TryGetValue(key, out definition!);

    private static LookupDefinition Setup(string segment, LookupKey key, string description) =>
        Def(key, LookupServiceNames.Setup, $"api/v1/{segment}/lookup", true, NoFilters, ActiveOnlyOnly, description);

    private static LookupDefinition User(string segment, LookupKey key, string description) =>
        Def(key, LookupServiceNames.User, $"api/v1/{segment}/lookup", true, NoFilters, ActiveOnlyOnly, description);

    private static LookupDefinition Asset(string segment, LookupKey key, string description) =>
        Def(key, LookupServiceNames.Asset, $"api/v1/{segment}/lookup", true, NoFilters, ActiveOnlyOnly, description);

    private static LookupDefinition Inventory(string segment, LookupKey key, string description) =>
        Def(key, LookupServiceNames.Inventory, $"api/v1/{segment}/lookup", true, NoFilters, ActiveOnlyOnly, description);

    private static LookupDefinition Def(
        LookupKey key,
        string service,
        string path,
        bool requiresTenant,
        IReadOnlyList<string> required,
        IReadOnlyList<string> optional,
        string description) =>
        new(key, service, path, requiresTenant, required, optional, description);
}

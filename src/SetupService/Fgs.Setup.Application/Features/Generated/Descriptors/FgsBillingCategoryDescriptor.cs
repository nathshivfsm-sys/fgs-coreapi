using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsBillingCategoryDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.BillingCategory,
        EntityName: "FgsBillingCategory",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsBillingCategory),
        SummaryDtoType: typeof(FgsBillingCategorySummaryDto),
        DetailDtoType: typeof(FgsBillingCategoryDetailDto),
        CreateDtoType: typeof(FgsBillingCategoryCreateDto),
        UpdateDtoType: typeof(FgsBillingCategoryUpdateDto),
        PatchDtoType: typeof(FgsBillingCategoryPatchDto),
        TableName: "FgsBillingCategory",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "billingcategories",
        SwaggerTag: "Setup - Billing",
        TableComment: "Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Primary key identity of the billing category record."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier owning this billing category."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier within the tenant owning this billing category."),
            new CatalogEntityColumnDescriptor(
                "BillingCategoryType", "BillingCategoryType", typeof(string), false, 0, false, true, true, "Short billing category code such as IN, PM, SR, or other tenant-defined values."),
            new CatalogEntityColumnDescriptor(
                "BillingCategoryName", "BillingCategoryName", typeof(string), false, 100, false, true, true, "Display name of the billing category shown throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 100, false, true, true, "Optional internal description or notes for the billing category."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls sorting/display order of billing categories in dropdowns and setup screens."),
            new CatalogEntityColumnDescriptor(
                "IsSystemDefined", "IsSystemDefined", typeof(bool), true, null, false, false, true, "Indicates whether the billing category was system seeded or manually created by the tenant/company."),
            new CatalogEntityColumnDescriptor(
                "ShowToFieldTech", "ShowToFieldTech", typeof(bool), true, null, false, false, true, "Date and time the billing category record was created."),
            new CatalogEntityColumnDescriptor(
                "AllowToPick", "AllowToPick", typeof(bool), true, null, false, false, true, "AllowToPick"),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User identifier that created the billing category record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the billing category record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User identifier that last updated the billing category record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the billing category is active and available for use."),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsBillingCategory", ["TenantId", "CompanyId", "BillingCategoryType", "BillingCategoryName"]),
        ],
        SearchableColumns: ["BillingCategoryType", "BillingCategoryName", "Description"],
        SortableColumns: ["Id", "BillingCategoryType", "BillingCategoryName", "Description", "DisplayOrder", "IsSystemDefined", "ShowToFieldTech", "AllowToPick", "IsActive"]);
}

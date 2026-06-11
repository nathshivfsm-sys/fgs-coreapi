using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPricingMatrixDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPricingMatrix,
        EntityName: "FgsSetupPricingMatrix",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPricingMatrix),
        SummaryDtoType: typeof(FgsSetupPricingMatrixSummaryDto),
        DetailDtoType: typeof(FgsSetupPricingMatrixDetailDto),
        CreateDtoType: typeof(FgsSetupPricingMatrixCreateDto),
        UpdateDtoType: typeof(FgsSetupPricingMatrixUpdateDto),
        PatchDtoType: typeof(FgsSetupPricingMatrixPatchDto),
        TableName: "FgsSetupPricingMatrix",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Guid,
        Variant: CatalogEntityVariant.StandardGuid,
        RoutePlural: "pricingmatrixs",
        SwaggerTag: "Setup - Pricing",
        TableComment: "FgsSetupPricingMatrix",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(Guid), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "Code", "Code", typeof(string), false, 0, false, true, true, "Code"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "IsLaborTierStructure", "IsLaborTierStructure", typeof(bool), true, null, false, false, true, "IsLaborTierStructure"),
            new CatalogEntityColumnDescriptor(
                "IsLaborRateBySkillLevel", "IsLaborRateBySkillLevel", typeof(bool), true, null, false, false, true, "IsLaborRateBySkillLevel"),
            new CatalogEntityColumnDescriptor(
                "IsMobileVisible", "IsMobileVisible", typeof(bool), true, null, false, false, true, "IsMobileVisible"),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "IsActive"),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupPricingMatrix", ["TenantId", "CompanyId", "Code"]),
        ],
        SearchableColumns: ["Code", "Name"],
        SortableColumns: ["Id", "Code", "Name", "IsLaborTierStructure", "IsLaborRateBySkillLevel", "IsMobileVisible", "IsActive"]);
}

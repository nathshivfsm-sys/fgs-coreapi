using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupDescriptionDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupDescription,
        EntityName: "FgsSetupDescription",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupDescription),
        SummaryDtoType: typeof(FgsSetupDescriptionSummaryDto),
        DetailDtoType: typeof(FgsSetupDescriptionDetailDto),
        CreateDtoType: typeof(FgsSetupDescriptionCreateDto),
        UpdateDtoType: typeof(FgsSetupDescriptionUpdateDto),
        PatchDtoType: typeof(FgsSetupDescriptionPatchDto),
        TableName: "FgsSetupDescription",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "descriptions",
        SwaggerTag: "Setup - Technician",
        TableComment: "FgsSetupDescription",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "DescriptionTypeCode", "DescriptionTypeCode", typeof(string), false, 0, false, true, true, "DescriptionTypeCode"),
            new CatalogEntityColumnDescriptor(
                "ShortNote", "ShortNote", typeof(string), false, 0, false, true, true, "ShortNote"),
            new CatalogEntityColumnDescriptor(
                "Body", "Body", typeof(string), false, 0, false, true, true, "Body"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupTechTradeId", "FgsSetupTechTradeId", typeof(long?), false, null, false, false, true, "FgsSetupTechTradeId"),
            new CatalogEntityColumnDescriptor(
                "SortOrder", "SortOrder", typeof(int), true, null, false, false, true, "SortOrder"),
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
        ],
        SearchableColumns: ["DescriptionTypeCode", "ShortNote", "Body"],
        SortableColumns: ["Id", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive"]);
}

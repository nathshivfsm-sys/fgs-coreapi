using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupTitleOfCourtesyDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupTitleOfCourtesy,
        EntityName: "FgsSetupTitleOfCourtesy",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupTitleOfCourtesy),
        SummaryDtoType: typeof(FgsSetupTitleOfCourtesySummaryDto),
        DetailDtoType: typeof(FgsSetupTitleOfCourtesyDetailDto),
        CreateDtoType: typeof(FgsSetupTitleOfCourtesyCreateDto),
        UpdateDtoType: typeof(FgsSetupTitleOfCourtesyUpdateDto),
        PatchDtoType: typeof(FgsSetupTitleOfCourtesyPatchDto),
        TableName: "FgsSetupTitleOfCourtesy",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "titleofcourtesies",
        SwaggerTag: "Setup - Technician",
        TableComment: "FgsSetupTitleOfCourtesy",
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
                "Code", "Code", typeof(string), false, 0, false, true, true, "Code"),
            new CatalogEntityColumnDescriptor(
                "DisplayName", "DisplayName", typeof(string), false, 0, false, true, true, "DisplayName"),
            new CatalogEntityColumnDescriptor(
                "SortOrder", "SortOrder", typeof(int?), false, null, false, false, true, "SortOrder"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupTitleOfCourtesy", ["TenantId", "CompanyId", "Code"]),
        ],
        SearchableColumns: ["Code", "DisplayName"],
        SortableColumns: ["Id", "Code", "DisplayName", "SortOrder", "IsActive"]);
}

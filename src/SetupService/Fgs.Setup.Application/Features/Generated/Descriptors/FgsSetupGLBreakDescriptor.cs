using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupGLBreakDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupGLBreak,
        EntityName: "FgsSetupGLBreak",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupGLBreak),
        SummaryDtoType: typeof(FgsSetupGLBreakSummaryDto),
        DetailDtoType: typeof(FgsSetupGLBreakDetailDto),
        CreateDtoType: typeof(FgsSetupGLBreakCreateDto),
        UpdateDtoType: typeof(FgsSetupGLBreakUpdateDto),
        PatchDtoType: typeof(FgsSetupGLBreakPatchDto),
        TableName: "FgsSetupGLBreak",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "glbreaks",
        SwaggerTag: "Setup - GL",
        TableComment: "FgsSetupGLBreak",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Surrogate primary key."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "Code", "Code", typeof(string), false, 0, false, true, true, "Unique GL break code within tenant, company, and break level scope."),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Display name of the GL break."),
            new CatalogEntityColumnDescriptor(
                "BreakLabel", "BreakLabel", typeof(string), false, 0, false, true, true, "Optional label displayed in UI and financial documents."),
            new CatalogEntityColumnDescriptor(
                "BreakLevel", "BreakLevel", typeof(short), true, null, false, false, true, "Break hierarchy level. Allowed values: 1 or 2."),
            new CatalogEntityColumnDescriptor(
                "LogoFileId", "LogoFileId", typeof(long?), false, null, false, false, true, "Optional reference to uploaded logo file in FgsFile."),
            new CatalogEntityColumnDescriptor(
                "AddressId", "AddressId", typeof(Guid?), false, null, false, false, true, "Optional reference to branch or break address in FgsLocation."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "UTC timestamp when the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User or process that created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UTC timestamp when the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 100, true, false, false, "User or process that last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the GL break is active."),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupGLBreak", ["TenantId", "CompanyId", "Code", "BreakLevel"]),
        ],
        SearchableColumns: ["Code", "Name", "BreakLabel"],
        SortableColumns: ["Id", "Code", "Name", "BreakLabel", "BreakLevel", "LogoFileId", "AddressId", "IsActive"]);
}

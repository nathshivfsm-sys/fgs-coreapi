using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupTechTradeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupTechTrade,
        EntityName: "FgsSetupTechTrade",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupTechTrade),
        SummaryDtoType: typeof(FgsSetupTechTradeSummaryDto),
        DetailDtoType: typeof(FgsSetupTechTradeDetailDto),
        CreateDtoType: typeof(FgsSetupTechTradeCreateDto),
        UpdateDtoType: typeof(FgsSetupTechTradeUpdateDto),
        PatchDtoType: typeof(FgsSetupTechTradePatchDto),
        TableName: "FgsSetupTechTrade",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "techtrades",
        SwaggerTag: "Setup - Technician",
        TableComment: "FgsSetupTechTrade",
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
                "TradeCode", "TradeCode", typeof(string), false, 0, false, true, true, "TradeCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupTechTrade", ["TenantId", "CompanyId", "TradeCode"]),
        ],
        SearchableColumns: ["TradeCode", "Name", "Description"],
        SortableColumns: ["Id", "TradeCode", "Name", "Description", "SortOrder", "IsActive"]);
}

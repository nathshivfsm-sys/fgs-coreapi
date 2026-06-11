using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPaymentTermDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPaymentTerm,
        EntityName: "FgsSetupPaymentTerm",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPaymentTerm),
        SummaryDtoType: typeof(FgsSetupPaymentTermSummaryDto),
        DetailDtoType: typeof(FgsSetupPaymentTermDetailDto),
        CreateDtoType: typeof(FgsSetupPaymentTermCreateDto),
        UpdateDtoType: typeof(FgsSetupPaymentTermUpdateDto),
        PatchDtoType: typeof(FgsSetupPaymentTermPatchDto),
        TableName: "FgsSetupPaymentTerm",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "paymentterms",
        SwaggerTag: "Setup - Billing",
        TableComment: "FgsSetupPaymentTerm",
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
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "DueDateMethod", "DueDateMethod", typeof(string), false, 0, false, true, true, "DueDateMethod"),
            new CatalogEntityColumnDescriptor(
                "NumberOfDays", "NumberOfDays", typeof(int?), false, null, false, false, true, "NumberOfDays"),
            new CatalogEntityColumnDescriptor(
                "IsAccountsReceivable", "IsAccountsReceivable", typeof(bool), true, null, false, false, true, "IsAccountsReceivable"),
            new CatalogEntityColumnDescriptor(
                "IsAccountsPayable", "IsAccountsPayable", typeof(bool), true, null, false, false, true, "IsAccountsPayable"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupPaymentTerm", ["TenantId", "CompanyId", "Name"]),
        ],
        SearchableColumns: ["Name", "DueDateMethod"],
        SortableColumns: ["Id", "Name", "DueDateMethod", "NumberOfDays", "IsAccountsReceivable", "IsAccountsPayable", "IsMobileVisible", "IsActive"]);
}

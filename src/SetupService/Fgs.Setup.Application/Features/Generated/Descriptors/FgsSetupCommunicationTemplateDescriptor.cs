using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupCommunicationTemplateDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupCommunicationTemplate,
        EntityName: "FgsSetupCommunicationTemplate",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupCommunicationTemplate),
        SummaryDtoType: typeof(FgsSetupCommunicationTemplateSummaryDto),
        DetailDtoType: typeof(FgsSetupCommunicationTemplateDetailDto),
        CreateDtoType: typeof(FgsSetupCommunicationTemplateCreateDto),
        UpdateDtoType: typeof(FgsSetupCommunicationTemplateUpdateDto),
        PatchDtoType: typeof(FgsSetupCommunicationTemplatePatchDto),
        TableName: "FgsSetupCommunicationTemplate",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.NullableTenantScope,
        RoutePlural: "communicationtemplates",
        SwaggerTag: "Setup - Communication",
        TableComment: "FgsSetupCommunicationTemplate",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long?), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long?), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "CommunicationChannel", "CommunicationChannel", typeof(string), false, 0, false, true, true, "CommunicationChannel"),
            new CatalogEntityColumnDescriptor(
                "TemplateType", "TemplateType", typeof(string), false, 100, false, true, true, "TemplateType"),
            new CatalogEntityColumnDescriptor(
                "Code", "Code", typeof(string), false, 0, false, true, true, "Code"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Subject", "Subject", typeof(string), false, 0, false, true, true, "Subject"),
            new CatalogEntityColumnDescriptor(
                "Body", "Body", typeof(string), false, 0, false, true, true, "Body"),
            new CatalogEntityColumnDescriptor(
                "IsMobileVisible", "IsMobileVisible", typeof(bool), true, null, false, false, true, "IsMobileVisible"),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 100, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "IsActive"),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body"],
        SortableColumns: ["Id", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive"]);
}

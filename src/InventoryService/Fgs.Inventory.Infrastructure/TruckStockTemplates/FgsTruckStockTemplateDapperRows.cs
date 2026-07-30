using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplates;

internal sealed class FgsTruckStockTemplateSummaryRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsTruckStockTemplateSummaryDto ToDto() =>
        new(Id, TemplateCode, Name, Description, IsActive);
}

internal sealed class FgsTruckStockTemplateDetailRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsTruckStockTemplateDetailDto ToDto() =>
        new(Id, TemplateCode, Name, Description, IsActive);
}

internal sealed class FgsTruckStockTemplateLookupRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsTruckStockTemplateLookupDto ToDto() => new(Id, TemplateCode, Name);
}

using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;

namespace Fgs.Inventory.Application.Abstractions.TruckStockTemplates;

public interface IFgsTruckStockTemplateWriteService
{
    Task<FgsTruckStockTemplateDetailDto> CreateAsync(FgsTruckStockTemplateCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTruckStockTemplateDetailDto> UpdateAsync(long id, FgsTruckStockTemplateUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTruckStockTemplateDetailDto> PatchAsync(long id, FgsTruckStockTemplatePatchDto dto, CancellationToken cancellationToken = default);
}

using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;

public interface IFgsSalesPipelineStatusWriteService
{
    Task<FgsSalesPipelineStatusDetailDto> CreateAsync(FgsSalesPipelineStatusCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesPipelineStatusDetailDto> UpdateAsync(long id, FgsSalesPipelineStatusUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesPipelineStatusDetailDto> PatchAsync(long id, FgsSalesPipelineStatusPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesPipelineStatusDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

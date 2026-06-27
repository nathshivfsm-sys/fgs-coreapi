using Fgs.Setup.Application.Features.LeadStatuses.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadStatuses;

public interface ILeadStatusWriteService
{
    Task<LeadStatusDetailDto> CreateAsync(LeadStatusCreateDto dto, CancellationToken cancellationToken = default);

    Task<LeadStatusDetailDto> UpdateAsync(long id, LeadStatusUpdateDto dto, CancellationToken cancellationToken = default);

    Task<LeadStatusDetailDto> PatchAsync(long id, LeadStatusPatchDto dto, CancellationToken cancellationToken = default);

    Task<LeadStatusDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

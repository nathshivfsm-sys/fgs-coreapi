using Fgs.Setup.Application.Features.LeadSources.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadSources;

public interface ILeadSourceWriteService
{
    Task<LeadSourceDetailDto> CreateAsync(LeadSourceCreateDto dto, CancellationToken cancellationToken = default);

    Task<LeadSourceDetailDto> UpdateAsync(long id, LeadSourceUpdateDto dto, CancellationToken cancellationToken = default);

    Task<LeadSourceDetailDto> PatchAsync(long id, LeadSourcePatchDto dto, CancellationToken cancellationToken = default);

    Task<LeadSourceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

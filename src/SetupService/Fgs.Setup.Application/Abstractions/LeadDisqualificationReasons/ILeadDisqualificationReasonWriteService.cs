using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;

public interface ILeadDisqualificationReasonWriteService
{
    Task<LeadDisqualificationReasonDetailDto> CreateAsync(LeadDisqualificationReasonCreateDto dto, CancellationToken cancellationToken = default);

    Task<LeadDisqualificationReasonDetailDto> UpdateAsync(long id, LeadDisqualificationReasonUpdateDto dto, CancellationToken cancellationToken = default);

    Task<LeadDisqualificationReasonDetailDto> PatchAsync(long id, LeadDisqualificationReasonPatchDto dto, CancellationToken cancellationToken = default);

    Task<LeadDisqualificationReasonDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

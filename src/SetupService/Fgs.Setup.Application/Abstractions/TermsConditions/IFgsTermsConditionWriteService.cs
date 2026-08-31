using Fgs.Setup.Application.Features.TermsConditions.Dtos;

namespace Fgs.Setup.Application.Abstractions.TermsConditions;

public interface IFgsTermsConditionWriteService
{
    Task<FgsTermsConditionDetailDto> CreateAsync(FgsTermsConditionCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTermsConditionDetailDto> UpdateAsync(long id, FgsTermsConditionUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTermsConditionDetailDto> PatchAsync(long id, FgsTermsConditionPatchDto dto, CancellationToken cancellationToken = default);
}

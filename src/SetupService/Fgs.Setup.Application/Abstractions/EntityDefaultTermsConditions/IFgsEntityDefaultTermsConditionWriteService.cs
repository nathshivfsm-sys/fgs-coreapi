using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;

namespace Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;

public interface IFgsEntityDefaultTermsConditionWriteService
{
    Task<FgsEntityDefaultTermsConditionDetailDto> CreateAsync(
        FgsEntityDefaultTermsConditionCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsEntityDefaultTermsConditionDetailDto> UpdateAsync(
        long id,
        FgsEntityDefaultTermsConditionUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsEntityDefaultTermsConditionDetailDto> PatchAsync(
        long id,
        FgsEntityDefaultTermsConditionPatchDto dto,
        CancellationToken cancellationToken = default);
}

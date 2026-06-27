using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;

public interface IFgsSalesActivityOutcomeWriteService
{
    Task<FgsSalesActivityOutcomeDetailDto> CreateAsync(FgsSalesActivityOutcomeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityOutcomeDetailDto> UpdateAsync(long id, FgsSalesActivityOutcomeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityOutcomeDetailDto> PatchAsync(long id, FgsSalesActivityOutcomePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityOutcomeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

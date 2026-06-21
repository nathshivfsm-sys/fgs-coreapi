using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesActivityTypes;

public interface IFgsSalesActivityTypeWriteService
{
    Task<FgsSalesActivityTypeDetailDto> CreateAsync(FgsSalesActivityTypeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityTypeDetailDto> UpdateAsync(long id, FgsSalesActivityTypeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityTypeDetailDto> PatchAsync(long id, FgsSalesActivityTypePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesActivityTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

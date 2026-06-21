using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesDispositionReasons;

public interface IFgsSalesDispositionReasonWriteService
{
    Task<FgsSalesDispositionReasonDetailDto> CreateAsync(FgsSalesDispositionReasonCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesDispositionReasonDetailDto> UpdateAsync(long id, FgsSalesDispositionReasonUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesDispositionReasonDetailDto> PatchAsync(long id, FgsSalesDispositionReasonPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSalesDispositionReasonDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

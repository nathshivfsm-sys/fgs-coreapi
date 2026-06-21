using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPaymentTerms;

public interface IFgsSetupPaymentTermWriteService
{
    Task<FgsSetupPaymentTermDetailDto> CreateAsync(FgsSetupPaymentTermCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentTermDetailDto> UpdateAsync(long id, FgsSetupPaymentTermUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentTermDetailDto> PatchAsync(long id, FgsSetupPaymentTermPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentTermDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

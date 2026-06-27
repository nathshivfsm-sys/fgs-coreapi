using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPaymentMethods;

public interface IFgsSetupPaymentMethodWriteService
{
    Task<FgsSetupPaymentMethodDetailDto> CreateAsync(FgsSetupPaymentMethodCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentMethodDetailDto> UpdateAsync(long id, FgsSetupPaymentMethodUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentMethodDetailDto> PatchAsync(long id, FgsSetupPaymentMethodPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupPaymentMethodDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

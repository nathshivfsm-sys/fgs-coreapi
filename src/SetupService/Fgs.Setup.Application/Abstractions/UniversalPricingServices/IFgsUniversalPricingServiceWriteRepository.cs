using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalPricingServices;

public interface IFgsUniversalPricingServiceWriteRepository
{
    Task<FgsUniversalPricingServiceDetailDto> CreateAsync(FgsUniversalPricingServiceCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalPricingServiceDetailDto> UpdateAsync(long id, FgsUniversalPricingServiceUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalPricingServiceDetailDto> PatchAsync(long id, FgsUniversalPricingServicePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUniversalPricingServiceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

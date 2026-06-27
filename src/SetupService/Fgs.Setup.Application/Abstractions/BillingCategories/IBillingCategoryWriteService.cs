using Fgs.Setup.Application.Features.BillingCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.BillingCategories;

public interface IBillingCategoryWriteService
{
    Task<BillingCategoryDetailDto> CreateAsync(BillingCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<BillingCategoryDetailDto> UpdateAsync(long id, BillingCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<BillingCategoryDetailDto> PatchAsync(long id, BillingCategoryPatchDto dto, CancellationToken cancellationToken = default);

    Task<BillingCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

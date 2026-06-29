using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.BillingCategories;

public interface IBillingCategoryReadRepository
{
    Task<BillingCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<BillingCategorySummaryDto>> ListAsync(
        SetupListQuery query,
        BillingCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BillingCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        bool? showToFieldTech = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
        string billingCategoryType, string billingCategoryName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

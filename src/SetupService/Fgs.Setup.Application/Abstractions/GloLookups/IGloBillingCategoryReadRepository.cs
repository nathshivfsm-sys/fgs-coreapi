using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloBillingCategoryReadRepository
{
    Task<IReadOnlyList<GloBillingCategoryTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByBillingCategoryTypeAsync(
        string billingCategoryType,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;

namespace Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;

public interface IFgsEntityDefaultTermsConditionReadRepository
{
    Task<FgsEntityDefaultTermsConditionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsEntityDefaultTermsConditionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEntityTypeAsync(
        string entityType,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;

namespace Fgs.Setup.Application.Abstractions.TermsConditions;

public interface IFgsTermsConditionReadRepository
{
    Task<FgsTermsConditionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTermsConditionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsTermsConditionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTermsConditionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAndVersionAsync(
        string code,
        int versionNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
}

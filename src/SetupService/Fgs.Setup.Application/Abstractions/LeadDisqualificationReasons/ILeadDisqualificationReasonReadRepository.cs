using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;

public interface ILeadDisqualificationReasonReadRepository
{
    Task<LeadDisqualificationReasonDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<LeadDisqualificationReasonSummaryDto>> ListAsync(
        SetupListQuery query,
        LeadDisqualificationReasonListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LeadDisqualificationReasonLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByReasonCodeAsync(
        string reasonCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByReasonNameAsync(
        string reasonName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesDispositionReasons;

public interface IFgsSalesDispositionReasonReadRepository
{
    Task<FgsSalesDispositionReasonDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSalesDispositionReasonSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSalesDispositionReasonListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSalesDispositionReasonLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDispositionReasonCodeAsync(
        string dispositionReasonCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByDispositionReasonNameAsync(
        string dispositionReasonName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

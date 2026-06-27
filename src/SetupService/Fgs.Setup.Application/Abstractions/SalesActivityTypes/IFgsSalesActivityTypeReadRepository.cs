using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesActivityTypes;

public interface IFgsSalesActivityTypeReadRepository
{
    Task<FgsSalesActivityTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSalesActivityTypeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSalesActivityTypeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSalesActivityTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByActivityTypeCodeAsync(
        string activityTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByActivityTypeNameAsync(
        string activityTypeName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.FgsBusinessTypes;

public interface IFgsBusinessTypeReadRepository
{
    Task<FgsBusinessTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsBusinessTypeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsBusinessTypeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsBusinessTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

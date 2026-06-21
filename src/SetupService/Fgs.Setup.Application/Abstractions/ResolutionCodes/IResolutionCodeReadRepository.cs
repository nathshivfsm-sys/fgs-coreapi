using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

namespace Fgs.Setup.Application.Abstractions.ResolutionCodes;

public interface IResolutionCodeReadRepository
{
    Task<ResolutionCodeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<ResolutionCodeSummaryDto>> ListAsync(
        SetupListQuery query,
        ResolutionCodeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ResolutionCodeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByResolutionCodeAsync(
        string resolutionCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsGloResolutionTypeIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}

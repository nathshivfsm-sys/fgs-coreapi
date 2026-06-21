using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupZones;

public interface IFgsSetupZoneReadRepository
{
    Task<FgsSetupZoneDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupZoneSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupZoneListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupZoneLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

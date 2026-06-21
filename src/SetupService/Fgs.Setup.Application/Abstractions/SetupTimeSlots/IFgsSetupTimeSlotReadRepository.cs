using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTimeSlots;

public interface IFgsSetupTimeSlotReadRepository
{
    Task<FgsSetupTimeSlotDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupTimeSlotSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupTimeSlotListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupTimeSlotLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsZoneIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}

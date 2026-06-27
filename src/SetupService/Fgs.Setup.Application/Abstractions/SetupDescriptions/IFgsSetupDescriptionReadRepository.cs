using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupDescriptions;

public interface IFgsSetupDescriptionReadRepository
{
    Task<FgsSetupDescriptionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupDescriptionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupDescriptionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupDescriptionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDescriptionTypeCodeAsync(
        string descriptionTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsTechTradeIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}

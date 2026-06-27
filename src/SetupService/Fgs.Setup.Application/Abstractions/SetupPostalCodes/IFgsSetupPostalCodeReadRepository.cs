using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPostalCodes;

public interface IFgsSetupPostalCodeReadRepository
{
    Task<FgsSetupPostalCodeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupPostalCodeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPostalCodeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupPostalCodeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByPostalCodeAsync(
        string postalCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsZoneIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsTaxIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}

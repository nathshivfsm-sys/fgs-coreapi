using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPaymentMethods;

public interface IFgsSetupPaymentMethodReadRepository
{
    Task<FgsSetupPaymentMethodDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupPaymentMethodSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPaymentMethodListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupPaymentMethodLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDisplayNameAsync(
        string displayName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

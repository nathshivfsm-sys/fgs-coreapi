using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPaymentTerms;

public interface IFgsSetupPaymentTermReadRepository
{
    Task<FgsSetupPaymentTermDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupPaymentTermSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPaymentTermListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupPaymentTermLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxDetails;

public interface IFgsSetupTaxDetailReadRepository
{
    Task<FgsSetupTaxDetailDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupTaxDetailSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupTaxDetailListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupTaxDetailLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsTaxIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsTaxAuthorityIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}

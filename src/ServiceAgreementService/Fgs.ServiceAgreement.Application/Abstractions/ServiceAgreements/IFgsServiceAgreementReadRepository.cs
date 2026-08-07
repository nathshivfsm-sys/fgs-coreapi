using Fgs.Foundation.Paging;
using Fgs.ServiceAgreement.Application.Common.ServiceAgreementCrud;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;

namespace Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;

public interface IFgsServiceAgreementReadRepository
{
    Task<FgsServiceAgreementDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsServiceAgreementSummaryDto>> ListAsync(
        ServiceAgreementListQuery query,
        FgsServiceAgreementListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByAgreementNumberAsync(
        string agreementNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

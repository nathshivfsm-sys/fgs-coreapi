using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.ListFgsServiceAgreements;

public sealed class ListFgsServiceAgreementsQueryHandler(IFgsServiceAgreementReadRepository readRepository)
    : IRequestHandler<ListFgsServiceAgreementsQuery, ApiResponse<PagedResult<FgsServiceAgreementSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsServiceAgreementSummaryDto>>> Handle(
        ListFgsServiceAgreementsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsServiceAgreementSummaryDto>>.Ok(result);
    }
}

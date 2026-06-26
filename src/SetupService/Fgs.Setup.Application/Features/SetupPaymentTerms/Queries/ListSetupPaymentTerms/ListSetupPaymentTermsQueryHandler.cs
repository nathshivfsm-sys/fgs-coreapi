using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.ListSetupPaymentTerms;

public sealed class ListSetupPaymentTermsQueryHandler(IFgsSetupPaymentTermReadRepository readRepository)
    : IRequestHandler<ListSetupPaymentTermsQuery, ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>> Handle(
        ListSetupPaymentTermsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>.Ok(result);
    }
}

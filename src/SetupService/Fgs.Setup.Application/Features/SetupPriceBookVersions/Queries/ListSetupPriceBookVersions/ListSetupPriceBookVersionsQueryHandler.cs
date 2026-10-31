using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.ListSetupPriceBookVersions;

public sealed class ListSetupPriceBookVersionsQueryHandler(
    IFgsSetupPriceBookVersionReadRepository readRepository)
    : IRequestHandler<ListSetupPriceBookVersionsQuery, ApiResponse<PagedResult<FgsSetupPriceBookVersionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPriceBookVersionSummaryDto>>> Handle(
        ListSetupPriceBookVersionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupPriceBookVersionSummaryDto>>.Ok(result);
    }
}

using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListResolutionCodes;

public sealed class ListResolutionCodesQueryHandler(IResolutionCodeReadRepository readRepository)
    : IRequestHandler<ListResolutionCodesQuery, ApiResponse<PagedResult<ResolutionCodeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<ResolutionCodeSummaryDto>>> Handle(
        ListResolutionCodesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<ResolutionCodeSummaryDto>>.Ok(result);
    }
}

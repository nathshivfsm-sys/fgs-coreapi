using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.ListSetupDescriptions;

public sealed class ListSetupDescriptionsQueryHandler(IFgsSetupDescriptionReadRepository readRepository)
    : IRequestHandler<ListSetupDescriptionsQuery, ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>> Handle(
        ListSetupDescriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>.Ok(result);
    }
}

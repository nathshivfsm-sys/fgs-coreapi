using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListSetupLaborRateTypes;

public sealed class ListSetupLaborRateTypesQueryHandler(IFgsSetupLaborRateTypeReadRepository readRepository)
    : IRequestHandler<ListSetupLaborRateTypesQuery, ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>> Handle(
        ListSetupLaborRateTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>.Ok(result);
    }
}

using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListActiveSetupLaborRateTypes;

public sealed class ListActiveSetupLaborRateTypesQueryHandler(IFgsSetupLaborRateTypeReadRepository readRepository)
    : IRequestHandler<ListActiveSetupLaborRateTypesQuery, ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>> Handle(
        ListActiveSetupLaborRateTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupLaborRateTypeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupLaborRateTypeSummaryDto>>(ex);
        }
    }
}

using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListActiveFgsBusinessTypes;

public sealed class ListActiveFgsBusinessTypesQueryHandler(IFgsBusinessTypeReadRepository readRepository)
    : IRequestHandler<ListActiveFgsBusinessTypesQuery, ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>> Handle(
        ListActiveFgsBusinessTypesQuery request,
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
                request.Filters ?? new FgsBusinessTypeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsBusinessTypeSummaryDto>>(ex);
        }
    }
}

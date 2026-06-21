using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.ListActiveJobTypes;

public sealed class ListActiveJobTypesQueryHandler(IJobTypeReadRepository readRepository)
    : IRequestHandler<ListActiveJobTypesQuery, ApiResponse<PagedResult<JobTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeSummaryDto>>> Handle(
        ListActiveJobTypesQuery request,
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
                request.Filters ?? new JobTypeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<JobTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<JobTypeSummaryDto>>(ex);
        }
    }
}

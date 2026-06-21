using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListActiveResolutionCodes;

public sealed class ListActiveResolutionCodesQueryHandler(IResolutionCodeReadRepository readRepository)
    : IRequestHandler<ListActiveResolutionCodesQuery, ApiResponse<PagedResult<ResolutionCodeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<ResolutionCodeSummaryDto>>> Handle(
        ListActiveResolutionCodesQuery request,
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
                request.Filters ?? new ResolutionCodeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<ResolutionCodeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<ResolutionCodeSummaryDto>>(ex);
        }
    }
}

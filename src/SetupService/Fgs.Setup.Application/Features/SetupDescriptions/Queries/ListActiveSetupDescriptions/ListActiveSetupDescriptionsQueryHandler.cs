using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.ListActiveSetupDescriptions;

public sealed class ListActiveSetupDescriptionsQueryHandler(IFgsSetupDescriptionReadRepository readRepository)
    : IRequestHandler<ListActiveSetupDescriptionsQuery, ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>> Handle(
        ListActiveSetupDescriptionsQuery request,
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
                request.Filters ?? new FgsSetupDescriptionListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupDescriptionSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupDescriptionSummaryDto>>(ex);
        }
    }
}

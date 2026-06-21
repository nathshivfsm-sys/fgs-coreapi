using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Queries.ListActiveVendors;

public sealed class ListActiveVendorsQueryHandler(IFgsVendorReadRepository readRepository)
    : IRequestHandler<ListActiveVendorsQuery, ApiResponse<PagedResult<FgsVendorSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVendorSummaryDto>>> Handle(
        ListActiveVendorsQuery request,
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
                request.Filters ?? new FgsVendorListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsVendorSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsVendorSummaryDto>>(ex);
        }
    }
}

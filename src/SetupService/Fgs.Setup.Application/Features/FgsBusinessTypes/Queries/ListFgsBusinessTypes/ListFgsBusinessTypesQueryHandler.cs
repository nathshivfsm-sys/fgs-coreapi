using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListFgsBusinessTypes;

public sealed class ListFgsBusinessTypesQueryHandler(IFgsBusinessTypeReadRepository readRepository)
    : IRequestHandler<ListFgsBusinessTypesQuery, ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>> Handle(
        ListFgsBusinessTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsBusinessTypeSummaryDto>>(ex);
        }
    }
}

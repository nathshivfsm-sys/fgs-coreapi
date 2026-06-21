using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.LookupWarehouses;

public sealed class LookupWarehousesQueryHandler(IFgsWarehouseReadRepository readRepository)
    : IRequestHandler<LookupWarehousesQuery, ApiResponse<IReadOnlyList<FgsWarehouseLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsWarehouseLookupDto>>> Handle(
        LookupWarehousesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsWarehouseLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsWarehouseLookupDto>>(ex);
        }
    }
}

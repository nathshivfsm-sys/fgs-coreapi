using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.GetFgsWarehouseById;

public sealed class GetFgsWarehouseByIdQueryHandler(IFgsWarehouseReadRepository readRepository)
    : IRequestHandler<GetFgsWarehouseByIdQuery, ApiResponse<FgsWarehouseDetailDto>>
{
    public async Task<ApiResponse<FgsWarehouseDetailDto>> Handle(
        GetFgsWarehouseByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsWarehouseDetailDto>.Fail(
                    [$"Warehouse '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsWarehouseDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsWarehouseDetailDto>(ex);
        }
    }
}

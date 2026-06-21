using Fgs.Setup.Application.Features.Warehouses.Dtos;

namespace Fgs.Setup.Application.Abstractions.Warehouses;

public interface IFgsWarehouseWriteService
{
    Task<FgsWarehouseDetailDto> CreateAsync(FgsWarehouseCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsWarehouseDetailDto> UpdateAsync(long id, FgsWarehouseUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsWarehouseDetailDto> PatchAsync(long id, FgsWarehousePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsWarehouseDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

using Fgs.Setup.Application.Features.Employees.Dtos;

namespace Fgs.Setup.Application.Abstractions.Employees;

public interface IFgsEmployeeWriteService
{
    Task<FgsEmployeeDetailDto> CreateAsync(FgsEmployeeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsEmployeeDetailDto> UpdateAsync(long id, FgsEmployeeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsEmployeeDetailDto> PatchAsync(long id, FgsEmployeePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsEmployeeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

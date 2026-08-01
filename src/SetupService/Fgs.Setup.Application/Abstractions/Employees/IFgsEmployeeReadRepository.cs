using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;

namespace Fgs.Setup.Application.Abstractions.Employees;

public interface IFgsEmployeeReadRepository
{
    Task<FgsEmployeeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsEmployeeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsEmployeeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsEmployeeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmployeeNumberAsync(
        string employeeNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUserIdAsync(
        long userId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}

using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;

namespace Fgs.Setup.Application.Abstractions.Employees;

public interface IFgsEmployeeReadRepository
{
    Task<FgsEmployeeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <param name="includeSummary">
    /// When true, runs company-scoped summary COUNTs (ignores list filters). When false, returns zeroed summary.
    /// </param>
    Task<FgsEmployeeListResultDto> ListAsync(
        SetupListQuery query,
        FgsEmployeeListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Company-scoped card counts (ignores list filters).
    /// </summary>
    Task<FgsEmployeeListSummaryDto> GetListSummaryAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsEmployeeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmployeeNumberAsync(
        string employeeNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByOfficeEmailAsync(
        string officeEmail,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUserIdAsync(
        Guid userId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTechCodeAsync(
        string techCode,
        long? excludeEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsTechnicianProfileByEmployeeIdAsync(
        long employeeId,
        CancellationToken cancellationToken = default);
}

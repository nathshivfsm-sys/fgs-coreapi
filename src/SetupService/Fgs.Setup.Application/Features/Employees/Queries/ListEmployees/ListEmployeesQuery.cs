using Fgs.Contracts.Api;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;

/// <param name="IncludeSummary">
/// When true (default), loads company-scoped card counts. When false, skips COUNT queries and returns zeros.
/// </param>
public sealed record ListEmployeesQuery(
    SetupListQuery Query,
    FgsEmployeeListFilters Filters,
    bool IncludeSummary = true)
    : IRequest<ApiResponse<FgsEmployeeListResultDto>>;

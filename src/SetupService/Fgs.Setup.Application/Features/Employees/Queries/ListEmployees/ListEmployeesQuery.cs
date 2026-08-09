using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;

public sealed record ListEmployeesQuery(
    SetupListQuery Query,
    FgsEmployeeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsEmployeeSummaryDto>>>;

using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.LookupEmployees;

public sealed record LookupEmployeesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsEmployeeLookupDto>>>;

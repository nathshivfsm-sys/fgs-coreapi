using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.GetFgsEmployeeById;

public sealed record GetFgsEmployeeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsEmployeeDetailDto>>;

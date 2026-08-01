using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Commands.DeleteFgsEmployee;

public sealed record DeleteFgsEmployeeCommand(long Id)
    : IRequest<ApiResponse<FgsEmployeeDetailDto>>;

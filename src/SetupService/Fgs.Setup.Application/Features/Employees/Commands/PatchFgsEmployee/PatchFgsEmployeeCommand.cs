using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;

public sealed record PatchFgsEmployeeCommand(long Id, FgsEmployeePatchDto Dto)
    : IRequest<ApiResponse<FgsEmployeeDetailDto>>;

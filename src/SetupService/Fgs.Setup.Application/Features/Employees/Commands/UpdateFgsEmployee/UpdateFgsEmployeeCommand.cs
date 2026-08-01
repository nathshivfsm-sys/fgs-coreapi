using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;

public sealed record UpdateFgsEmployeeCommand(long Id, FgsEmployeeUpdateDto Dto)
    : IRequest<ApiResponse<FgsEmployeeDetailDto>>;

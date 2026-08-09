using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;

public sealed record CreateFgsEmployeeCommand(FgsEmployeeCreateDto Dto)
    : IRequest<ApiResponse<FgsEmployeeDetailDto>>;

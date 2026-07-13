using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Commands.CreateFgsRole;

public sealed record CreateFgsRoleCommand(FgsRoleCreateDto Dto) : IRequest<ApiResponse<FgsRoleDetailDto>>;

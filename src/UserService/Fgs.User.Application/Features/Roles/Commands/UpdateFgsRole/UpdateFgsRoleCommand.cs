using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole;

public sealed record UpdateFgsRoleCommand(long Id, FgsRoleUpdateDto Dto) : IRequest<ApiResponse<FgsRoleDetailDto>>;

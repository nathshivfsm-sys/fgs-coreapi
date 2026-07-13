using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Commands.PatchFgsRole;

public sealed record PatchFgsRoleCommand(long Id, FgsRolePatchDto Dto) : IRequest<ApiResponse<FgsRoleDetailDto>>;

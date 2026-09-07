using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Commands.CloneFgsRole;

public sealed record CloneFgsRoleCommand(long SourceRoleId, FgsRoleCloneDto Dto)
    : IRequest<ApiResponse<FgsRoleDetailDto>>;

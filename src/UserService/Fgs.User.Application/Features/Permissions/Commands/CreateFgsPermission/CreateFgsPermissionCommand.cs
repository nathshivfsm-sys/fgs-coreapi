using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Commands.CreateFgsPermission;

public sealed record CreateFgsPermissionCommand(FgsPermissionCreateDto Dto)
    : IRequest<ApiResponse<FgsPermissionDetailDto>>;

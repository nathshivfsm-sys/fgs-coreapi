using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Commands.PatchFgsPermission;

public sealed record PatchFgsPermissionCommand(long Id, FgsPermissionPatchDto Dto)
    : IRequest<ApiResponse<FgsPermissionDetailDto>>;

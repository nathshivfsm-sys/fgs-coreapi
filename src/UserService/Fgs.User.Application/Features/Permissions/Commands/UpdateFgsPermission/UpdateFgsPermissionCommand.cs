using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Commands.UpdateFgsPermission;

public sealed record UpdateFgsPermissionCommand(long Id, FgsPermissionUpdateDto Dto)
    : IRequest<ApiResponse<FgsPermissionDetailDto>>;

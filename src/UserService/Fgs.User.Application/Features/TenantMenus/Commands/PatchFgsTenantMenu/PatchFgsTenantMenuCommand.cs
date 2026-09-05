using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Commands.PatchFgsTenantMenu;

public sealed record PatchFgsTenantMenuCommand(long Id, FgsTenantMenuPatchDto Dto)
    : IRequest<ApiResponse<FgsTenantMenuDetailDto>>;

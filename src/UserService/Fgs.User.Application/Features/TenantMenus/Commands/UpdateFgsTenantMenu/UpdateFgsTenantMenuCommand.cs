using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Commands.UpdateFgsTenantMenu;

public sealed record UpdateFgsTenantMenuCommand(long Id, FgsTenantMenuUpdateDto Dto)
    : IRequest<ApiResponse<FgsTenantMenuDetailDto>>;

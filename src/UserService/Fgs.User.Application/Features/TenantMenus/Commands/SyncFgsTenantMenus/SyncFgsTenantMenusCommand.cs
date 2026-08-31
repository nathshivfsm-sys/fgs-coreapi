using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;

public sealed record SyncFgsTenantMenusCommand(FgsTenantMenuSyncDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>>;

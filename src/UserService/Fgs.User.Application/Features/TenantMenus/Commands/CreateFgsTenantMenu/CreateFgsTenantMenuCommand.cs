using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Commands.CreateFgsTenantMenu;

public sealed record CreateFgsTenantMenuCommand(FgsTenantMenuCreateDto Dto)
    : IRequest<ApiResponse<FgsTenantMenuDetailDto>>;

using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.ListFgsTenantMenus;

public sealed record ListFgsTenantMenusQuery()
    : IRequest<ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>>;

using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.LookupFgsTenantMenus;

public sealed record LookupFgsTenantMenusQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTenantMenuLookupDto>>>;

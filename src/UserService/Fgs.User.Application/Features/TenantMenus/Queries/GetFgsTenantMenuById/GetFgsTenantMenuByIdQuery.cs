using Fgs.Contracts.Api;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.GetFgsTenantMenuById;

public sealed record GetFgsTenantMenuByIdQuery(long Id) : IRequest<ApiResponse<FgsTenantMenuDetailDto>>;

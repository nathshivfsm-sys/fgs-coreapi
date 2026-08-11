using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenants;

public sealed record ListTenantsQuery(IdentityListQuery Query)
    : IRequest<ApiResponse<PagedResult<TenantSummaryDto>>>;

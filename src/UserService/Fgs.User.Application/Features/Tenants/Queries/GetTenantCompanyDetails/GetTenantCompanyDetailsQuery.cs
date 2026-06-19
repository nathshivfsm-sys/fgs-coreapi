using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;

public sealed record GetTenantCompanyDetailsQuery(long TenantId, long CompanyId)
    : IRequest<ApiResponse<TenantCompanyDetailDto>>;

using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantCompanyDetails;

public sealed record UpdateTenantCompanyDetailsCommand(
    long TenantId,
    long CompanyId,
    UpdateTenantCompanyDetailsRequest Request)
    : IRequest<ApiResponse<TenantCompanyDetailDto>>;

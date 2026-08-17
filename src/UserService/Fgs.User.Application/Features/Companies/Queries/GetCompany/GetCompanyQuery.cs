using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Queries.GetCompany;

public sealed record GetCompanyQuery(long TenantId, long CompanyId)
    : IRequest<ApiResponse<CompanyDetailDto>>;

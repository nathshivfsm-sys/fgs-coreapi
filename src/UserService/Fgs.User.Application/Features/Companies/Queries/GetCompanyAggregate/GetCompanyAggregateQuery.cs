using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Queries.GetCompanyAggregate;

public sealed record GetCompanyAggregateQuery(long TenantId, long CompanyId)
    : IRequest<ApiResponse<CompanyAggregateDto>>;

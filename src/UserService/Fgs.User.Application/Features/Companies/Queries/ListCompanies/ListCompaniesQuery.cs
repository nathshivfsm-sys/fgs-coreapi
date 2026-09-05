using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Queries.ListCompanies;

public sealed record ListCompaniesQuery(long TenantId)
    : IRequest<ApiResponse<IReadOnlyList<TenantCompanyDto>>>;

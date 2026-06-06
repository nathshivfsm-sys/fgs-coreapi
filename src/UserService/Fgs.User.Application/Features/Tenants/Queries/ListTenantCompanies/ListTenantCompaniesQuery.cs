using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;

public sealed record ListTenantCompaniesQuery(long TenantId) : IRequest<ApiResponse<IReadOnlyList<TenantCompanyDto>>>;

using Fgs.User.Application.Features.Tenants.Dtos;

namespace Fgs.User.Application.Abstractions.Persistence;

public interface ITenantCompanyDetailsReadQuery
{
    Task<TenantCompanyDetailDto?> GetAsync(
        long tenantId,
        long companyNumber,
        CancellationToken cancellationToken = default);
}

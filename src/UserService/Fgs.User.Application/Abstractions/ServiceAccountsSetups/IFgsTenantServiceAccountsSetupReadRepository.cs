using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceAccountsSetups;

public interface IFgsTenantServiceAccountsSetupReadRepository
{
    Task<FgsTenantServiceAccountsSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default);

    Task<FgsTenantServiceAccountsSetupDetailDto?> GetByTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);
}

using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceAccountsSetups;

public interface IFgsTenantServiceAccountsSetupReadRepository
{
    Task<FgsTenantServiceAccountsSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default);

    Task<FgsTenantServiceAccountsSetupDetailDto?> GetByTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>> ListAsync(
        IdentityListQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);
}

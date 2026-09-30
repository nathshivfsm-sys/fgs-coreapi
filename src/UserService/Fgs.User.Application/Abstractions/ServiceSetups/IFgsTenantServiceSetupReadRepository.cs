using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceSetups;

public interface IFgsTenantServiceSetupReadRepository
{
    Task<FgsTenantServiceSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default);

    Task<FgsTenantServiceSetupDetailDto?> GetByTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTenantServiceSetupSummaryDto>> ListAsync(
        IdentityListQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTenantServiceSetupLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);
}

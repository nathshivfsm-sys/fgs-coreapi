using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Common.Security;

public sealed class DbFgsTenantScopeValidator(IUnitOfWork unitOfWork) : IFgsTenantScopeValidator
{
    public async Task<bool> IsValidScopeAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default)
    {
        var tenant = await unitOfWork.Repository<FgsTenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantId && t.IsActive, cancellationToken);

        if (tenant is null)
        {
            return false;
        }

        return await unitOfWork.Repository<FgsTenantCompany>()
            .AnyAsync(
                c => c.TenantId == tenantId && c.CompanyNumber == companyId && c.IsActive,
                cancellationToken);
    }
}

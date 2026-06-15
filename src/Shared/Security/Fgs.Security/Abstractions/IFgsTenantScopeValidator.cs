namespace Fgs.Security.Abstractions;

public interface IFgsTenantScopeValidator
{
    Task<bool> IsValidScopeAsync(long tenantId, long companyId, CancellationToken cancellationToken = default);
}

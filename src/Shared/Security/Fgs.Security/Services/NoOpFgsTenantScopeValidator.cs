using Fgs.Security.Abstractions;

namespace Fgs.Security.Services;

public sealed class NoOpFgsTenantScopeValidator : IFgsTenantScopeValidator
{
    public Task<bool> IsValidScopeAsync(long tenantId, long companyId, CancellationToken cancellationToken = default) =>
        Task.FromResult(true);
}

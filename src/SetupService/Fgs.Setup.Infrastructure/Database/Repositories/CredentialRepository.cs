using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Database.Repositories;

public sealed class CredentialRepository : ICredentialRepository
{
    private readonly FgsSetupDbContext _setupContext;

    public CredentialRepository(FgsSetupDbContext setupContext) => _setupContext = setupContext;

    public Task<GloCredential?> GetGlobalByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _setupContext.GloCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<FgsCredential?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _setupContext.FgsCredentials
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<GloCredential?> GetGlobalByProviderTypeIdAsync(
        int providerTypeId,
        CancellationToken cancellationToken = default) =>
        _setupContext.GloCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.CredentialProviderTypeId == providerTypeId, cancellationToken);

    public Task<FgsCredential?> GetTenantByProviderTypeAsync(
        long tenantId,
        long companyId,
        int providerTypeId,
        CancellationToken cancellationToken = default) =>
        _setupContext.FgsCredentials
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(
                x => x.TenantId == tenantId && x.CompanyId == companyId && x.CredentialProviderTypeId == providerTypeId,
                cancellationToken);

    public Task<GloCredentialProviderType?> GetProviderTypeByCodeAsync(
        string providerCode,
        CancellationToken cancellationToken = default) =>
        _setupContext.GloCredentialProviderTypes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.ProviderCode == providerCode, cancellationToken);

    public async Task<IReadOnlyList<GloCredential>> ListGlobalAsync(
        bool activeOnly,
        CancellationToken cancellationToken = default)
    {
        var query = _setupContext.GloCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .AsQueryable();

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query.OrderBy(x => x.CredentialName).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<FgsCredential>> ListTenantAsync(
        long? tenantId,
        long? companyId,
        bool activeOnly,
        CancellationToken cancellationToken = default)
    {
        var query = _setupContext.FgsCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .AsQueryable();

        if (tenantId.HasValue)
        {
            query = query.Where(x => x.TenantId == tenantId.Value);
        }

        if (companyId.HasValue)
        {
            query = query.Where(x => x.CompanyId == companyId.Value);
        }

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.TenantId)
            .ThenBy(x => x.CompanyId)
            .ThenBy(x => x.CredentialName)
            .ToListAsync(cancellationToken);
    }

    public Task AddGlobalAsync(GloCredential credential, CancellationToken cancellationToken = default) =>
        _setupContext.GloCredentials.AddAsync(credential, cancellationToken).AsTask();

    public Task AddTenantAsync(FgsCredential credential, CancellationToken cancellationToken = default) =>
        _setupContext.FgsCredentials.AddAsync(credential, cancellationToken).AsTask();

    public void UpdateGlobal(GloCredential credential) => _setupContext.GloCredentials.Update(credential);

    public void UpdateTenant(FgsCredential credential) => _setupContext.FgsCredentials.Update(credential);

    public void RemoveGlobal(GloCredential credential) => _setupContext.GloCredentials.Remove(credential);

    public void RemoveTenant(FgsCredential credential) => _setupContext.FgsCredentials.Remove(credential);
}

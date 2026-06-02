using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence.Database.Repositories;

public sealed class CredentialRepository : ICredentialRepository
{
    private readonly FgsUserDbContext _context;

    public CredentialRepository(FgsUserDbContext context) => _context = context;

    public Task<GloCredential?> GetGlobalByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _context.GloCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<FgsCredential?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.FgsCredentials
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<GloCredential?> GetGlobalByProviderTypeIdAsync(
        int providerTypeId,
        CancellationToken cancellationToken = default) =>
        _context.GloCredentials
            .IgnoreQueryFilters()
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(x => x.CredentialProviderTypeId == providerTypeId, cancellationToken);

    public Task<FgsCredential?> GetTenantByProviderTypeAsync(
        long tenantId,
        long companyId,
        int providerTypeId,
        CancellationToken cancellationToken = default) =>
        _context.FgsCredentials
            .Include(x => x.ProviderType)
            .FirstOrDefaultAsync(
                x => x.TenantId == tenantId && x.CompanyId == companyId && x.CredentialProviderTypeId == providerTypeId,
                cancellationToken);

    public Task<GloCredentialProviderType?> GetProviderTypeByCodeAsync(
        string providerCode,
        CancellationToken cancellationToken = default) =>
        _context.GloCredentialProviderTypes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.ProviderCode == providerCode, cancellationToken);

    public async Task<IReadOnlyList<GloCredential>> ListGlobalAsync(
        bool activeOnly,
        CancellationToken cancellationToken = default)
    {
        var query = _context.GloCredentials
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
        var query = _context.FgsCredentials
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
        _context.GloCredentials.AddAsync(credential, cancellationToken).AsTask();

    public Task AddTenantAsync(FgsCredential credential, CancellationToken cancellationToken = default) =>
        _context.FgsCredentials.AddAsync(credential, cancellationToken).AsTask();

    public void UpdateGlobal(GloCredential credential) => _context.GloCredentials.Update(credential);

    public void UpdateTenant(FgsCredential credential) => _context.FgsCredentials.Update(credential);

    public void RemoveGlobal(GloCredential credential) => _context.GloCredentials.Remove(credential);

    public void RemoveTenant(FgsCredential credential) => _context.FgsCredentials.Remove(credential);

    public Task AddAuditAsync(FgsCredentialAudit audit, CancellationToken cancellationToken = default) =>
        _context.FgsCredentialAudits.AddAsync(audit, cancellationToken).AsTask();
}

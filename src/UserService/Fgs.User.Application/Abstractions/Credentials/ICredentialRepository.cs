using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialRepository
{
    Task<GloCredential?> GetGlobalByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<FgsCredential?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<GloCredential?> GetGlobalByProviderTypeIdAsync(
        int providerTypeId,
        CancellationToken cancellationToken = default);

    Task<FgsCredential?> GetTenantByProviderTypeAsync(
        long tenantId,
        long companyId,
        int providerTypeId,
        CancellationToken cancellationToken = default);

    Task<GloCredentialProviderType?> GetProviderTypeByCodeAsync(
        string providerCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GloCredential>> ListGlobalAsync(
        bool activeOnly,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsCredential>> ListTenantAsync(
        long? tenantId,
        long? companyId,
        bool activeOnly,
        CancellationToken cancellationToken = default);

    Task AddGlobalAsync(GloCredential credential, CancellationToken cancellationToken = default);

    Task AddTenantAsync(FgsCredential credential, CancellationToken cancellationToken = default);

    void UpdateGlobal(GloCredential credential);

    void UpdateTenant(FgsCredential credential);

    void RemoveGlobal(GloCredential credential);

    void RemoveTenant(FgsCredential credential);

    Task AddAuditAsync(FgsCredentialAudit audit, CancellationToken cancellationToken = default);
}

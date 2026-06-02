using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Constants;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.Persistence.Abstractions;

namespace Fgs.User.Application.Features.Credentials.Services;

public sealed class CredentialMutationService
{
    private readonly ICredentialRepository _repository;
    private readonly ICredentialEncryptionService _encryptionService;
    private readonly ICredentialActorResolver _actorResolver;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICredentialConfigurationProvider _configurationProvider;

    public CredentialMutationService(
        ICredentialRepository repository,
        ICredentialEncryptionService encryptionService,
        ICredentialActorResolver actorResolver,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        ICredentialConfigurationProvider configurationProvider)
    {
        _repository = repository;
        _encryptionService = encryptionService;
        _actorResolver = actorResolver;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _configurationProvider = configurationProvider;
    }

    public async Task<(GloCredential Credential, string ProviderCode)> CreateGlobalAsync(
        string providerCode,
        string credentialName,
        string? description,
        byte[] payload,
        CancellationToken cancellationToken)
    {
        var providerType = await RequireActiveProviderAsync(providerCode, cancellationToken);
        if (await _repository.GetGlobalByProviderTypeIdAsync(providerType.Id, cancellationToken) is not null)
        {
            throw new InvalidOperationException(CredentialErrorMessages.GlobalCredentialExists);
        }

        var envelope = await _encryptionService.EncryptAsync(payload, cancellationToken);
        var actor = _actorResolver.ResolveActorId();
        var now = _dateTimeProvider.UtcNow;

        var credential = new GloCredential
        {
            CredentialProviderTypeId = providerType.Id,
            CredentialName = credentialName.Trim(),
            Description = description?.Trim(),
            CredentialData = envelope.CredentialData,
            EncryptedDataKey = envelope.EncryptedDataKey,
            KeyIdentifier = envelope.KeyIdentifier,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await _repository.AddGlobalAsync(credential, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        credential.ProviderType = providerType;
        await _configurationProvider.ReloadAsync(cancellationToken);
        return (credential, providerType.ProviderCode);
    }

    public async Task<(FgsCredential Credential, string ProviderCode)> CreateTenantAsync(
        long tenantId,
        long companyId,
        string providerCode,
        string credentialName,
        string? description,
        byte[] payload,
        CancellationToken cancellationToken)
    {
        var providerType = await RequireActiveProviderAsync(providerCode, cancellationToken);
        if (await _repository.GetTenantByProviderTypeAsync(tenantId, companyId, providerType.Id, cancellationToken) is not null)
        {
            throw new InvalidOperationException(CredentialErrorMessages.TenantCredentialExists);
        }

        var envelope = await _encryptionService.EncryptAsync(payload, cancellationToken);
        var actor = _actorResolver.ResolveActorId();
        var now = _dateTimeProvider.UtcNow;

        var credential = new FgsCredential
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CompanyId = companyId,
            CredentialProviderTypeId = providerType.Id,
            CredentialName = credentialName.Trim(),
            Description = description?.Trim(),
            CredentialData = envelope.CredentialData,
            EncryptedDataKey = envelope.EncryptedDataKey,
            KeyIdentifier = envelope.KeyIdentifier,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await _repository.AddTenantAsync(credential, cancellationToken);
        await WriteAuditAsync(credential, CredentialAuditActions.Created, "Credential created.", null, null, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        credential.ProviderType = providerType;
        await _configurationProvider.ReloadAsync(cancellationToken);
        return (credential, providerType.ProviderCode);
    }

    public async Task<GloCredential> UpdateGlobalAsync(
        int id,
        string credentialName,
        string? description,
        byte[]? payload,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var credential = await _repository.GetGlobalByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.GlobalCredentialNotFound);

        credential.CredentialName = credentialName.Trim();
        credential.Description = description?.Trim();
        if (isActive.HasValue)
        {
            credential.IsActive = isActive.Value;
        }

        if (payload is not null)
        {
            var envelope = await _encryptionService.EncryptAsync(payload, cancellationToken);
            credential.CredentialData = envelope.CredentialData;
            credential.EncryptedDataKey = envelope.EncryptedDataKey;
            credential.KeyIdentifier = envelope.KeyIdentifier;
        }

        credential.UpdatedOn = _dateTimeProvider.UtcNow;
        credential.UpdatedBy = _actorResolver.ResolveActorId();
        _repository.UpdateGlobal(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
        return credential;
    }

    public async Task<FgsCredential> UpdateTenantAsync(
        Guid id,
        string credentialName,
        string? description,
        byte[]? payload,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var credential = await _repository.GetTenantByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.TenantCredentialNotFound);

        credential.CredentialName = credentialName.Trim();
        credential.Description = description?.Trim();
        if (isActive.HasValue)
        {
            credential.IsActive = isActive.Value;
        }

        if (payload is not null)
        {
            var envelope = await _encryptionService.EncryptAsync(payload, cancellationToken);
            credential.CredentialData = envelope.CredentialData;
            credential.EncryptedDataKey = envelope.EncryptedDataKey;
            credential.KeyIdentifier = envelope.KeyIdentifier;
        }

        credential.UpdatedOn = _dateTimeProvider.UtcNow;
        credential.UpdatedBy = _actorResolver.ResolveActorId();
        _repository.UpdateTenant(credential);
        await WriteAuditAsync(credential, CredentialAuditActions.Updated, "Credential updated.", null, null, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
        return credential;
    }

    public async Task DeleteGlobalAsync(int id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetGlobalByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.GlobalCredentialNotFound);

        _repository.RemoveGlobal(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
    }

    public async Task DeleteTenantAsync(Guid id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetTenantByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.TenantCredentialNotFound);

        await WriteAuditAsync(credential, CredentialAuditActions.Revoked, "Credential deleted.", null, null, cancellationToken);
        _repository.RemoveTenant(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
    }

    public async Task<GloCredential> RotateGlobalAsync(
        int id,
        CredentialRotationMode rotationMode,
        CancellationToken cancellationToken)
    {
        var credential = await _repository.GetGlobalByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.GlobalCredentialNotFound);

        await ApplyRotationAsync(
            credential.CredentialData,
            credential.EncryptedDataKey,
            credential.KeyIdentifier,
            rotationMode,
            (data, key, keyId) =>
            {
                credential.CredentialData = data;
                credential.EncryptedDataKey = key;
                credential.KeyIdentifier = keyId;
                credential.UpdatedOn = _dateTimeProvider.UtcNow;
                credential.UpdatedBy = _actorResolver.ResolveActorId();
            },
            cancellationToken);

        _repository.UpdateGlobal(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
        return credential;
    }

    public async Task<FgsCredential> RotateTenantAsync(
        Guid id,
        CredentialRotationMode rotationMode,
        CancellationToken cancellationToken)
    {
        var credential = await _repository.GetTenantByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(CredentialErrorMessages.TenantCredentialNotFound);

        await ApplyRotationAsync(
            credential.CredentialData,
            credential.EncryptedDataKey,
            credential.KeyIdentifier,
            rotationMode,
            (data, key, keyId) =>
            {
                credential.CredentialData = data;
                credential.EncryptedDataKey = key;
                credential.KeyIdentifier = keyId;
                credential.UpdatedOn = _dateTimeProvider.UtcNow;
                credential.UpdatedBy = _actorResolver.ResolveActorId();
            },
            cancellationToken);

        _repository.UpdateTenant(credential);
        await WriteAuditAsync(credential, CredentialAuditActions.Rotated, $"Credential rotated ({rotationMode}).", null, null, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _configurationProvider.ReloadAsync(cancellationToken);
        return credential;
    }

    public Task<byte[]> DecryptGlobalAsync(GloCredential credential, CancellationToken cancellationToken) =>
        _encryptionService.DecryptAsync(credential.CredentialData, credential.EncryptedDataKey, cancellationToken);

    public Task<byte[]> DecryptTenantAsync(FgsCredential credential, CancellationToken cancellationToken) =>
        _encryptionService.DecryptAsync(credential.CredentialData, credential.EncryptedDataKey, cancellationToken);

    private async Task ApplyRotationAsync(
        byte[] credentialData,
        byte[] encryptedDataKey,
        string? keyIdentifier,
        CredentialRotationMode rotationMode,
        Action<byte[], byte[], string> assign,
        CancellationToken cancellationToken)
    {
        switch (rotationMode)
        {
            case CredentialRotationMode.Full:
            {
                var envelope = await _encryptionService.ReEncryptPayloadAsync(
                    credentialData,
                    encryptedDataKey,
                    cancellationToken);
                assign(envelope.CredentialData, envelope.EncryptedDataKey, envelope.KeyIdentifier);
                break;
            }
            case CredentialRotationMode.KmsReEncrypt:
            {
                var reEncrypted = await _encryptionService.ReEncryptDataKeyOnlyAsync(
                    encryptedDataKey,
                    keyIdentifier,
                    cancellationToken);
                assign(credentialData, reEncrypted.EncryptedDataKey, reEncrypted.KeyIdentifier);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(rotationMode), rotationMode, null);
        }
    }

    private async Task<GloCredentialProviderType> RequireActiveProviderAsync(
        string providerCode,
        CancellationToken cancellationToken)
    {
        var providerType = await _repository.GetProviderTypeByCodeAsync(providerCode.Trim(), cancellationToken);
        if (providerType is null)
        {
            throw new InvalidOperationException(CredentialErrorMessages.ProviderNotFound);
        }

        if (!providerType.IsActive)
        {
            throw new InvalidOperationException(CredentialErrorMessages.ProviderInactive);
        }

        return providerType;
    }

    private Task WriteAuditAsync(
        FgsCredential credential,
        string actionType,
        string? remarks,
        int? oldVersion,
        int? newVersion,
        CancellationToken cancellationToken)
    {
        var audit = new FgsCredentialAudit
        {
            Id = Guid.NewGuid(),
            TenantId = credential.TenantId,
            CompanyId = credential.CompanyId,
            CredentialId = credential.Id,
            ActionType = actionType,
            Remarks = remarks,
            OldVersionNo = oldVersion,
            NewVersionNo = newVersion,
            CreatedOn = _dateTimeProvider.UtcNow,
            CreatedBy = _actorResolver.ResolveActorId()
        };

        return _repository.AddAuditAsync(audit, cancellationToken);
    }
}

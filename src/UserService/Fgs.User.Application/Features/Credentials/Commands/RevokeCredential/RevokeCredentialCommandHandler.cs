using Fgs.Foundation.Correlation;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Time;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Constants;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RevokeCredential;

public sealed class RevokeCredentialCommandHandler(
    IUnitOfWork unitOfWork,
    ISecretsManagerService secretsManager,
    ISecretCache secretCache,
    ICredentialAuditWriter auditWriter,
    ICorrelationContext correlationContext,
    IDateTimeProvider dateTime) : IRequestHandler<RevokeCredentialCommand, ApiResponse<CredentialSecretMetadataDto>>
{
    public async Task<ApiResponse<CredentialSecretMetadataDto>> Handle(
        RevokeCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var secretRepo = unitOfWork.Repository<FgsCredentialSecret>();
        var secret = await secretRepo.GetByIdAsync(request.SecretId, cancellationToken);

        if (secret is null
            || secret.TenantId != request.TenantId
            || secret.CompanyId != request.CompanyId)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.SecretNotFound],
                ApiStatusCodes.NotFound);
        }

        if (secret.IsRevoked)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.SecretAlreadyRevoked],
                ApiStatusCodes.Conflict);
        }

        var provider = await unitOfWork.Repository<FgsCredentialProvider>()
            .GetByIdAsync(secret.CredentialProviderId, cancellationToken);

        if (provider is null)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.ProviderNotFound],
                ApiStatusCodes.NotFound);
        }

        var providerType = await unitOfWork.Repository<GloCredentialProviderType>()
            .FirstOrDefaultAsync(p => p.Id == provider.CredentialProviderTypeId, cancellationToken);

        var arn = CredentialSecretStorageMapping.GetAwsSecretArn(secret);
        await secretsManager.DeleteSecretAsync(arn, forceDelete: false, cancellationToken);

        secret.IsRevoked = true;
        secret.IsActive = false;
        secret.UpdatedOn = dateTime.UtcNow;
        secret.UpdatedBy = request.RevokedBy;
        secretRepo.Update(secret);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        secretCache.Invalidate(request.TenantId, request.CompanyId, secret.Id);

        await auditWriter.WriteAsync(
            request.TenantId,
            request.CompanyId,
            secret.Id,
            CredentialAuditActions.Revoked,
            secret.VersionNo,
            secret.VersionNo,
            CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), "Secret revoked."),
            request.RevokedBy,
            cancellationToken: cancellationToken);

        return ApiResponse<CredentialSecretMetadataDto>.Ok(
            CredentialMetadataMapper.ToSecretMetadata(secret, provider, providerType?.Code));
    }
}

using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Credentials;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Constants;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RotateCredential;

public sealed class RotateCredentialCommandHandler(
    IUnitOfWork unitOfWork,
    ISecretsManagerService secretsManager,
    ISecretCache secretCache,
    ICredentialAuditWriter auditWriter,
    ICorrelationContext correlationContext,
    IDateTimeProvider dateTime) : IRequestHandler<RotateCredentialCommand, ApiResponse<CredentialSecretMetadataDto>>
{
    public async Task<ApiResponse<CredentialSecretMetadataDto>> Handle(
        RotateCredentialCommand request,
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

        var oldVersion = secret.VersionNo;
        var arn = CredentialSecretStorageMapping.GetAwsSecretArn(secret);

        await secretsManager.RotateSecretAsync(arn, request.RotationLambdaArn, cancellationToken);

        secret.VersionNo++;
        secret.LastRotatedOn = dateTime.UtcNow;
        secret.UpdatedOn = dateTime.UtcNow;
        secret.UpdatedBy = request.RotatedBy;
        secretRepo.Update(secret);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        secretCache.Invalidate(request.TenantId, request.CompanyId, secret.Id);

        await auditWriter.WriteAsync(
            request.TenantId,
            request.CompanyId,
            secret.Id,
            CredentialAuditActions.Rotated,
            oldVersion,
            secret.VersionNo,
            CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), "Rotation initiated."),
            request.RotatedBy,
            cancellationToken: cancellationToken);

        return ApiResponse<CredentialSecretMetadataDto>.Ok(
            CredentialMetadataMapper.ToSecretMetadata(secret, provider, providerType?.Code));
    }
}

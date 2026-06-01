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

namespace Fgs.User.Application.Features.Credentials.Commands.UpdateCredential;

public sealed class UpdateCredentialCommandHandler(
    IUnitOfWork unitOfWork,
    ISecretsManagerService secretsManager,
    ISecretCache secretCache,
    ICredentialAuditWriter auditWriter,
    ICorrelationContext correlationContext,
    IDateTimeProvider dateTime) : IRequestHandler<UpdateCredentialCommand, ApiResponse<CredentialSecretMetadataDto>>
{
    public async Task<ApiResponse<CredentialSecretMetadataDto>> Handle(
        UpdateCredentialCommand request,
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

        if (request.SecretPayload is not { } payload
            || payload.ValueKind is System.Text.Json.JsonValueKind.Undefined
            || payload.ValueKind is System.Text.Json.JsonValueKind.Null)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.SecretPayloadRequiredForUpdate],
                ApiStatusCodes.BadRequest);
        }

        var oldVersion = secret.VersionNo;

        return await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var arn = CredentialSecretStorageMapping.GetAwsSecretArn(secret);
            await secretsManager.PutSecretValueAsync(arn, payload.GetRawText(), ct);
            secret.VersionNo++;

            secret.UpdatedOn = dateTime.UtcNow;
            secret.UpdatedBy = request.UpdatedBy;
            secretRepo.Update(secret);

            secretCache.Invalidate(request.TenantId, request.CompanyId, secret.Id);

            await auditWriter.WriteAsync(
                request.TenantId,
                request.CompanyId,
                secret.Id,
                CredentialAuditActions.Updated,
                oldVersion,
                secret.VersionNo,
                CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), "Secret updated."),
                request.UpdatedBy,
                saveImmediately: false,
                ct);

            return ApiResponse<CredentialSecretMetadataDto>.Ok(
                CredentialMetadataMapper.ToSecretMetadata(secret, provider, providerType?.Code));
        }, cancellationToken);
    }
}

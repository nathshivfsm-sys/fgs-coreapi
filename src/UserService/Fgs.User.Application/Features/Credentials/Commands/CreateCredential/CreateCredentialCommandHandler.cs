using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Credentials;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Constants;
using Fgs.User.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Credentials.Commands.CreateCredential;

public sealed class CreateCredentialCommandHandler(
    IUnitOfWork unitOfWork,
    ISecretsManagerService secretsManager,
    ICredentialSecretNameBuilder secretNameBuilder,
    ICredentialAuditWriter auditWriter,
    ICorrelationContext correlationContext,
    IDateTimeProvider dateTime,
    IConfiguration configuration) : IRequestHandler<CreateCredentialCommand, ApiResponse<CredentialSecretMetadataDto>>
{
    public async Task<ApiResponse<CredentialSecretMetadataDto>> Handle(
        CreateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var kmsKeyArn = configuration["AwsCredentials:KmsKeyArn"];
        if (string.IsNullOrWhiteSpace(kmsKeyArn))
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.KmsKeyArnNotConfigured],
                ApiStatusCodes.BadRequest);
        }

        var region = configuration["AwsCredentials:Region"] ?? "us-east-1";
        var providerType = await unitOfWork.Repository<GloCredentialProviderType>()
            .FirstOrDefaultAsync(p => p.Id == request.CredentialProviderTypeId, cancellationToken);

        if (providerType is null)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.ProviderTypeNotFound],
                ApiStatusCodes.BadRequest);
        }

        var tenant = await unitOfWork.Repository<FgsTenant>()
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.TenantNotFound],
                ApiStatusCodes.NotFound);
        }

        try
        {
            return await unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
            var providerRepo = unitOfWork.Repository<FgsCredentialProvider>();
            var provider = await providerRepo.FirstOrDefaultAsync(
                p => p.TenantId == request.TenantId
                    && p.CompanyId == request.CompanyId
                    && p.Code == request.ProviderCode,
                ct);

            if (provider is null)
            {
                provider = new FgsCredentialProvider
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    CompanyId = request.CompanyId,
                    CredentialProviderTypeId = request.CredentialProviderTypeId,
                    Code = request.ProviderCode,
                    Name = request.ProviderName,
                    Environment = request.Environment,
                    Description = request.Description,
                    IsActive = true,
                    CreatedOn = dateTime.UtcNow,
                    CreatedBy = request.CreatedBy
                };
                await providerRepo.AddAsync(provider, ct);
            }

            var smName = secretNameBuilder.BuildSecretName(
                provider.Environment,
                tenant.TenantCode,
                provider.Code);
            var logicalSecretName = smName[(smName.LastIndexOf('/') + 1)..];

            var secretRepo = unitOfWork.Repository<FgsCredentialSecret>();
            var duplicateSecret = await secretRepo.AnyAsync(
                s => s.TenantId == request.TenantId
                    && s.CompanyId == request.CompanyId
                    && s.CredentialProviderId == provider.Id
                    && s.SecretName == logicalSecretName
                    && s.IsActive
                    && !s.IsRevoked,
                ct);

            if (duplicateSecret)
            {
                throw new InvalidOperationException(CredentialErrorMessages.SecretAlreadyExists);
            }

            if (request.Configurations is not null)
            {
                var configRepo = unitOfWork.Repository<FgsCredentialProviderConfiguration>();
                foreach (var (key, value) in request.Configurations)
                {
                    var exists = await configRepo.AnyAsync(
                        c => c.TenantId == request.TenantId
                            && c.CompanyId == request.CompanyId
                            && c.CredentialProviderId == provider.Id
                            && c.ConfigurationKey == key
                            && c.Environment == request.Environment,
                        ct);

                    if (exists)
                    {
                        continue;
                    }

                    await configRepo.AddAsync(new FgsCredentialProviderConfiguration
                    {
                        Id = Guid.NewGuid(),
                        TenantId = request.TenantId,
                        CompanyId = request.CompanyId,
                        CredentialProviderId = provider.Id,
                        ConfigurationKey = key,
                        ConfigurationValue = value,
                        Environment = request.Environment,
                        IsActive = true,
                        CreatedOn = dateTime.UtcNow,
                        CreatedBy = request.CreatedBy
                    }, ct);
                }
            }

            var secretJson = request.SecretPayload.GetRawText();
            var tags = new Dictionary<string, string>
            {
                ["TenantId"] = request.TenantId.ToString(),
                ["CompanyId"] = request.CompanyId.ToString(),
                ["ProviderCode"] = provider.Code
            };

            var createResult = await secretsManager.CreateSecretAsync(
                smName,
                secretJson,
                kmsKeyArn,
                tags,
                ct);

            var secret = new FgsCredentialSecret
            {
                Id = Guid.NewGuid(),
                TenantId = request.TenantId,
                CompanyId = request.CompanyId,
                CredentialProviderId = provider.Id,
                SecretName = logicalSecretName,
                VersionNo = 1,
                IsActive = true,
                IsRevoked = false,
                CreatedOn = dateTime.UtcNow,
                CreatedBy = request.CreatedBy
            };

            CredentialSecretStorageMapping.SetAwsSecretArn(secret, createResult.SecretArn);
            CredentialSecretStorageMapping.SetRegionName(secret, region);
            CredentialSecretStorageMapping.SetKmsKeyArn(secret, kmsKeyArn);

            await secretRepo.AddAsync(secret, ct);

            await auditWriter.WriteAsync(
                request.TenantId,
                request.CompanyId,
                secret.Id,
                CredentialAuditActions.Created,
                null,
                secret.VersionNo,
                CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), "Secret created in AWS Secrets Manager."),
                request.CreatedBy,
                saveImmediately: false,
                ct);

            return ApiResponse<CredentialSecretMetadataDto>.Ok(
                CredentialMetadataMapper.ToSecretMetadata(secret, provider, providerType.Code),
                ApiStatusCodes.Created);
            }, cancellationToken);
        }
        catch (InvalidOperationException ex) when (ex.Message == CredentialErrorMessages.SecretAlreadyExists)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.SecretAlreadyExists],
                ApiStatusCodes.Conflict);
        }
        catch (CredentialSecretsException ex) when (ex.IsAccessDenied)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [ex.Message],
                ApiStatusCodes.Forbidden);
        }
        catch (CredentialSecretsException ex)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [ex.Message],
                ApiStatusCodes.InternalServerError);
        }
    }
}

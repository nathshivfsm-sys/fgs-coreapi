using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Application.Common.Options;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Files.Commands.CreateUploadUrl;

public sealed class CreateUploadUrlCommandHandler(
    IUserTenantClient userTenantClient,
    ITenantContextAccessor tenantContextAccessor,
    IS3ObjectKeyBuilder objectKeyBuilder,
    IS3ObjectStorageService objectStorageService,
    IUnitOfWork unitOfWork,
    IOptions<FileServiceOptions> fileOptions)
    : IRequestHandler<CreateUploadUrlCommand, ApiResponse<CreateFileUploadUrlResponse>>
{
    public async Task<ApiResponse<CreateFileUploadUrlResponse>> Handle(
        CreateUploadUrlCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is not { IsResolved: true })
        {
            return ApiResponse<CreateFileUploadUrlResponse>.Fail(
                ["Tenant context is required."],
                ApiStatusCodes.BadRequest);
        }

        if (!FileEntityTypes.TryParse(request.EntityType, out var entityType))
        {
            return ApiResponse<CreateFileUploadUrlResponse>.Fail(
                ["Unsupported entity type."],
                ApiStatusCodes.BadRequest);
        }

        var entityTypeValue = FileEntityTypes.ToStorageValue(entityType);

        if (FileEntityTypes.RequiresMatchingCompanyContext(entityType)
            && request.EntityId != tenantContext.CompanyId)
        {
            return ApiResponse<CreateFileUploadUrlResponse>.Fail(
                ["EntityId must match the current company context."],
                ApiStatusCodes.BadRequest);
        }

        var options = fileOptions.Value;
        if (request.FileSizeBytes > options.MaxUploadSizeBytes)
        {
            return ApiResponse<CreateFileUploadUrlResponse>.Fail(
                [$"File size exceeds the maximum allowed size of {options.MaxUploadSizeBytes} bytes."],
                ApiStatusCodes.BadRequest);
        }

        var tenantResponse = await userTenantClient.GetTenantAsync(tenantContext.TenantId, cancellationToken);
        if (!tenantResponse.Success || tenantResponse.Data?.StorageBucketName is not { Length: > 0 } bucketName)
        {
            return ApiResponse<CreateFileUploadUrlResponse>.Fail(
                ["Tenant storage bucket is not provisioned."],
                ApiStatusCodes.BadRequest);
        }

        var variant = request.RequestedVariant.ToLowerInvariant();
        var storedFileName = BuildStoredFileName(request.FileName);
        var sourceObjectKey = objectKeyBuilder.BuildCompanyAssetKey(
            tenantContext.CompanyId,
            entityTypeValue,
            request.EntityId,
            storedFileName);

        var baseTags = request.Tags?.Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];

        var now = DateTimeOffset.UtcNow;
        var fileExtension = Path.GetExtension(request.FileName).ToLowerInvariant();
        long pendingFileId = 0;

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await RemoveStalePendingFileAsync(
                tenantContext.TenantId,
                tenantContext.CompanyId,
                entityTypeValue,
                request.EntityId,
                variant,
                ct);

            var tags = baseTags
                .Concat(FileLogoVariants.BuildVariantTags(variant))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            var file = new FgsFile
            {
                TenantId = tenantContext.TenantId,
                CompanyId = tenantContext.CompanyId,
                EntityType = entityTypeValue,
                EntityId = request.EntityId,
                BucketName = bucketName,
                ObjectKey = sourceObjectKey,
                OriginalFileName = request.FileName.Trim(),
                StoredFileName = storedFileName,
                ContentType = request.ContentType.Trim(),
                FileExtension = fileExtension,
                FileSizeBytes = 0,
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                Tags = tags,
                UploadedByName = "TenantAdmin",
                UploadedByType = "User",
                CreatedOn = now,
                CreatedBy = "TenantAdmin"
            };

            await unitOfWork.Repository<FgsFile>().AddAsync(file, ct);
            await unitOfWork.SaveChangesAsync(ct);
            pendingFileId = file.Id;
        }, cancellationToken);

        var expiry = TimeSpan.FromMinutes(options.UploadUrlExpiryMinutes);
        var expiresAt = DateTimeOffset.UtcNow.Add(expiry);
        var presignedUpload = await objectStorageService.CreateUploadUrlAsync(
            bucketName,
            sourceObjectKey,
            request.ContentType,
            expiry,
            cancellationToken);

        return ApiResponse<CreateFileUploadUrlResponse>.Ok(new CreateFileUploadUrlResponse(
            pendingFileId,
            presignedUpload.Url,
            "PUT",
            presignedUpload.RequiredHeaders,
            sourceObjectKey,
            expiresAt));
    }

    private async Task RemoveStalePendingFileAsync(
        long tenantId,
        long companyId,
        string entityType,
        long entityId,
        string variant,
        CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<FgsFile>();
        var staleFiles = await repo.ListAsync(
            file => file.TenantId == tenantId
                    && file.CompanyId == companyId
                    && file.EntityType == entityType
                    && file.EntityId == entityId
                    && file.FileSizeBytes == 0
                    && file.Tags != null
                    && file.Tags.Contains(FileLogoVariants.LogoTag)
                    && file.Tags.Contains(variant),
            cancellationToken);

        foreach (var stale in staleFiles)
        {
            repo.Remove(stale);
        }
    }

    private static string BuildStoredFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var sanitizedBase = string.Concat(baseName.Where(ch => char.IsLetterOrDigit(ch) || ch is '-' or '_'));
        if (string.IsNullOrWhiteSpace(sanitizedBase))
        {
            sanitizedBase = "upload";
        }

        return $"{sanitizedBase}-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
    }
}

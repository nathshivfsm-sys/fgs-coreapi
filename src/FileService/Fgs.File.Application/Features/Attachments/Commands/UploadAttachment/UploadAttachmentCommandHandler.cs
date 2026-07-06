using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;

public sealed class UploadAttachmentCommandHandler(
    IUserTenantClient userTenantClient,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext,
    IS3ObjectKeyBuilder objectKeyBuilder,
    IFileStorageService fileStorageService,
    IThumbnailGenerator thumbnailGenerator,
    IImageVariantGenerator imageVariantGenerator,
    IAttachmentUrlBuilder urlBuilder,
    IUnitOfWork unitOfWork,
    IOptions<FileServiceOptions> fileOptions) : IRequestHandler<UploadAttachmentCommand, ApiResponse<AttachmentMetadataDto>>
{
    public async Task<ApiResponse<AttachmentMetadataDto>> Handle(
        UploadAttachmentCommand command,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is null)
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(["Tenant context is required."], ApiStatusCodes.BadRequest);
        }

        if (!FileEntityTypes.TryParse(command.EntityType, out var entityType))
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(["Unsupported entity type."], ApiStatusCodes.BadRequest);
        }

        var entityTypeValue = FileEntityTypes.ToStorageValue(entityType);
        if (FileEntityTypes.RequiresMatchingCompanyContext(entityType)
            && command.EntityId != tenantContext.CompanyId)
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(
                ["EntityId must match the current company context."],
                ApiStatusCodes.BadRequest);
        }

        if (command.FileSizeBytes > fileOptions.Value.MaxUploadSizeBytes)
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(
                [$"File size exceeds the maximum allowed size of {fileOptions.Value.MaxUploadSizeBytes} bytes."],
                ApiStatusCodes.BadRequest);
        }

        var tenantResponse = await userTenantClient.GetTenantAsync(tenantContext.TenantId, cancellationToken);
        if (!tenantResponse.Success || tenantResponse.Data?.StorageBucketName is not { Length: > 0 } bucketName)
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(
                ["Tenant storage bucket is not provisioned."],
                ApiStatusCodes.BadRequest);
        }

        var originalFileName = command.OriginalFileName.Trim();
        var contentType = command.ContentType.Trim();
        var storedFileName = AttachmentFileValidator.BuildStoredFileName(originalFileName);
        var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();

        await using var sourceBuffer = new MemoryStream();
        await command.FileContent.CopyToAsync(sourceBuffer, cancellationToken);
        var fileBytes = sourceBuffer.ToArray();

        var isLogo = command.Category.Equals("logo", StringComparison.OrdinalIgnoreCase);
        byte[] mainContent = fileBytes;
        string mainContentType = contentType;
        string mainStoredFileName = storedFileName;
        string? mainExtension = fileExtension;

        if (isLogo && !string.IsNullOrWhiteSpace(command.LogoVariant))
        {
            var variant = command.LogoVariant.ToLowerInvariant();
            await using var variantInput = new MemoryStream(fileBytes);
            var generated = await imageVariantGenerator.GenerateVariantAsync(
                variantInput,
                contentType,
                variant,
                cancellationToken);

            if (generated is null)
            {
                return ApiResponse<AttachmentMetadataDto>.Fail(
                    ["Logo variant was not generated."],
                    ApiStatusCodes.BadRequest);
            }

            mainContent = generated.Content;
            mainContentType = generated.ContentType;
            mainExtension = generated.FileExtension;
            mainStoredFileName = FileLogoVariants.BuildVariantFileName(storedFileName, variant, mainExtension);
        }

        var objectKey = objectKeyBuilder.BuildCompanyAssetKey(
            tenantContext.CompanyId,
            entityTypeValue,
            command.EntityId,
            mainStoredFileName);

        var tags = BuildTags(command);
        var auditActor = userContext.ResolveAuditActor();
        var now = DateTimeOffset.UtcNow;
        FgsFile? savedFile = null;

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            if (isLogo && !string.IsNullOrWhiteSpace(command.LogoVariant))
            {
                await SupersedeExistingLogoAsync(
                    tenantContext.TenantId,
                    tenantContext.CompanyId,
                    entityTypeValue,
                    command.EntityId,
                    command.LogoVariant!.ToLowerInvariant(),
                    ct);
            }

            await using (var uploadStream = new MemoryStream(mainContent))
            {
                await fileStorageService.UploadAsync(
                    new StorageObjectRef(bucketName, objectKey),
                    uploadStream,
                    new StorageUploadOptions(mainContentType, mainContent.Length),
                    ct);
            }

            await using var thumbInput = new MemoryStream(mainContent);
            var thumbnail = await thumbnailGenerator.GenerateAsync(
                thumbInput,
                mainContentType,
                originalFileName,
                ct);

            string thumbnailObjectKey;
            if (thumbnail is not null)
            {
                var thumbDir = objectKey[..(objectKey.LastIndexOf('/') + 1)];
                thumbnailObjectKey = await ResolveUniqueThumbnailKeyAsync(
                    bucketName,
                    $"{thumbDir}{thumbnail.ThumbnailFileName}",
                    ct);

                await using var thumbStream = new MemoryStream(thumbnail.Content);
                await fileStorageService.UploadAsync(
                    new StorageObjectRef(bucketName, thumbnailObjectKey),
                    thumbStream,
                    new StorageUploadOptions(thumbnail.ContentType, thumbnail.Content.Length),
                    ct);
            }
            else
            {
                thumbnailObjectKey = objectKeyBuilder.BuildThumbnailKey(objectKey, originalFileName);
            }

            var file = new FgsFile
            {
                TenantId = tenantContext.TenantId,
                CompanyId = tenantContext.CompanyId,
                EntityType = entityTypeValue,
                EntityId = command.EntityId,
                BucketName = bucketName,
                ObjectKey = objectKey,
                ThumbnailObjectKey = thumbnailObjectKey,
                OriginalFileName = originalFileName,
                StoredFileName = mainStoredFileName,
                ContentType = mainContentType,
                FileExtension = mainExtension,
                FileSizeBytes = mainContent.Length,
                Description = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim(),
                Tags = tags,
                IsVisibleToCustomer = command.IsVisibleToCustomer,
                IsVisibleToFieldTechnician = command.IsVisibleToFieldTechnician,
                UploadedByUserId = userContext.UserId.HasValue ? null : null,
                UploadedByName = auditActor,
                UploadedByType = userContext.IsAuthenticated ? "User" : "System",
                CreatedOn = now,
                CreatedBy = auditActor
            };

            await unitOfWork.Repository<FgsFile>().AddAsync(file, ct);
            await unitOfWork.SaveChangesAsync(ct);
            savedFile = file;
        }, cancellationToken);

        return ApiResponse<AttachmentMetadataDto>.Ok(
            AttachmentMetadataMapper.ToDto(savedFile!, urlBuilder),
            ApiStatusCodes.Created);
    }

    private static string[] BuildTags(UploadAttachmentCommand command)
    {
        var tags = AttachmentCategoryTags.MergeTags(command.Category, command.Tags).ToList();
        if (command.Category.Equals("logo", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(command.LogoVariant))
        {
            tags.Add(FileLogoVariants.LogoTag);
            tags.Add(command.LogoVariant.ToLowerInvariant());
        }

        return tags.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private async Task SupersedeExistingLogoAsync(
        long tenantId,
        long companyId,
        string entityType,
        long entityId,
        string variant,
        CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<FgsFile>();
        var existingFiles = await repo.ListAsync(
            f => f.TenantId == tenantId
                 && f.CompanyId == companyId
                 && f.EntityType == entityType
                 && f.EntityId == entityId
                 && (f.Tags == null || !f.Tags.Contains(AttachmentDeletionTags.DeletedTag))
                 && f.FileSizeBytes > 0
                 && f.Tags != null
                 && f.Tags.Contains(FileLogoVariants.LogoTag)
                 && f.Tags.Contains(variant),
            cancellationToken);

        foreach (var existing in existingFiles)
        {
            await fileStorageService.DeleteAsync(new StorageObjectRef(existing.BucketName, existing.ObjectKey), cancellationToken);
            if (!string.IsNullOrWhiteSpace(existing.ThumbnailObjectKey))
            {
                await fileStorageService.DeleteAsync(
                    new StorageObjectRef(existing.BucketName, existing.ThumbnailObjectKey),
                    cancellationToken);
            }

            existing.Tags = AttachmentDeletionTags.MarkDeleted(existing.Tags);
            existing.UpdatedOn = DateTimeOffset.UtcNow;
            repo.Update(existing);
        }
    }

    private async Task<string> ResolveUniqueThumbnailKeyAsync(
        string bucketName,
        string thumbnailKey,
        CancellationToken cancellationToken)
    {
        if (!await fileStorageService.ExistsAsync(new StorageObjectRef(bucketName, thumbnailKey), cancellationToken))
        {
            return thumbnailKey;
        }

        var dir = thumbnailKey[..(thumbnailKey.LastIndexOf('/') + 1)];
        var fileName = Path.GetFileNameWithoutExtension(thumbnailKey);
        var ext = Path.GetExtension(thumbnailKey);
        return $"{dir}{fileName}-{Guid.NewGuid():N}{ext}";
    }
}

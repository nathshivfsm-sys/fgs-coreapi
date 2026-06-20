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

namespace Fgs.File.Application.Features.Files.Commands.CompleteUpload;

public sealed class CompleteUploadCommandHandler(
    ITenantContextAccessor tenantContextAccessor,
    IS3ObjectStorageService objectStorageService,
    IS3ObjectKeyBuilder objectKeyBuilder,
    IImageVariantGenerator imageVariantGenerator,
    IFileContentUrlBuilder contentUrlBuilder,
    IUnitOfWork unitOfWork,
    IOptions<FileServiceOptions> fileOptions)
    : IRequestHandler<CompleteUploadCommand, ApiResponse<FileVariantSetDto>>
{
    public async Task<ApiResponse<FileVariantSetDto>> Handle(
        CompleteUploadCommand command,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is not { IsResolved: true })
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Tenant context is required."], ApiStatusCodes.BadRequest);
        }

        var file = await unitOfWork.Repository<FgsFile>().GetByIdAsync(command.FileId, cancellationToken);
        if (file is null)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["File not found."], ApiStatusCodes.NotFound);
        }

        if (file.TenantId != tenantContext.TenantId || file.CompanyId != tenantContext.CompanyId)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["File does not match tenant context."], ApiStatusCodes.Forbidden);
        }

        if (!FileUploadState.IsPending(file))
        {
            return ApiResponse<FileVariantSetDto>.Fail(["File upload is not pending."], ApiStatusCodes.BadRequest);
        }

        var expiry = TimeSpan.FromMinutes(fileOptions.Value.UploadUrlExpiryMinutes);
        if (file.CreatedOn.Add(expiry) <= DateTimeOffset.UtcNow)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Upload session has expired."], ApiStatusCodes.BadRequest);
        }

        if (!FileLogoVariants.TryGetVariantTag(file.Tags, out var variant))
        {
            return ApiResponse<FileVariantSetDto>.Fail(["File variant tag was not found."], ApiStatusCodes.BadRequest);
        }

        var stagingObjectKey = file.ObjectKey;
        if (!await objectStorageService.ObjectExistsAsync(file.BucketName, stagingObjectKey, cancellationToken))
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Uploaded object was not found in storage."], ApiStatusCodes.NotFound);
        }

        await using var sourcePayload = await objectStorageService.GetObjectAsync(
            file.BucketName,
            stagingObjectKey,
            cancellationToken);

        var generated = await imageVariantGenerator.GenerateVariantAsync(
            sourcePayload.Content,
            file.ContentType ?? "application/octet-stream",
            variant,
            cancellationToken);

        if (generated is null)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Logo variant was not generated."], ApiStatusCodes.BadRequest);
        }

        var fileExtension = file.FileExtension ?? Path.GetExtension(file.StoredFileName);
        var variantFileName = FileLogoVariants.BuildVariantFileName(file.StoredFileName, variant, fileExtension);
        var variantObjectKey = objectKeyBuilder.BuildCompanyAssetKey(
            file.CompanyId,
            file.EntityType,
            file.EntityId,
            variantFileName);

        var downloadExpiry = TimeSpan.FromMinutes(fileOptions.Value.DownloadUrlExpiryMinutes);
        var now = DateTimeOffset.UtcNow;
        FileVariantInfoDto? variantResult = null;

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await SupersedeExistingCompletedVariantAsync(file, variant, ct);

            await using var uploadStream = new MemoryStream(generated.Content);
            await objectStorageService.PutObjectAsync(
                file.BucketName,
                variantObjectKey,
                uploadStream,
                generated.ContentType,
                ct);

            file.ObjectKey = variantObjectKey;
            file.StoredFileName = variantFileName;
            file.ThumbnailObjectKey = objectKeyBuilder.BuildThumbnailKey(variantObjectKey);
            file.ContentType = generated.ContentType;
            file.FileExtension = generated.FileExtension;
            file.FileSizeBytes = generated.Content.Length;
            file.UpdatedOn = now;
            file.UpdatedBy = "TenantAdmin";
            unitOfWork.Repository<FgsFile>().Update(file);
            await unitOfWork.SaveChangesAsync(ct);

            var downloadUrl = await objectStorageService.CreateDownloadUrlAsync(
                file.BucketName,
                variantObjectKey,
                downloadExpiry,
                ct);

            variantResult = new FileVariantInfoDto(
                file.Id,
                contentUrlBuilder.BuildContentUrl(file.Id),
                downloadUrl,
                generated.ContentType,
                generated.Content.Length);

            await objectStorageService.DeleteObjectAsync(file.BucketName, stagingObjectKey, ct);
        }, cancellationToken);

        return ApiResponse<FileVariantSetDto>.Ok(new FileVariantSetDto(
            variantResult!.FileId,
            new Dictionary<string, FileVariantInfoDto>(StringComparer.OrdinalIgnoreCase)
            {
                [variant] = variantResult
            }));
    }

    private async Task SupersedeExistingCompletedVariantAsync(
        FgsFile file,
        string variant,
        CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<FgsFile>();
        var existingFiles = await repo.ListAsync(
            f => f.Id != file.Id
                 && f.EntityType == file.EntityType
                 && f.EntityId == file.EntityId
                 && f.FileSizeBytes > 0
                 && f.Tags != null
                 && f.Tags.Contains(FileLogoVariants.LogoTag)
                 && f.Tags.Contains(variant),
            cancellationToken);

        foreach (var existing in existingFiles)
        {
            await objectStorageService.DeleteObjectAsync(existing.BucketName, existing.ObjectKey, cancellationToken);
            if (!string.IsNullOrWhiteSpace(existing.ThumbnailObjectKey))
            {
                await objectStorageService.DeleteObjectAsync(existing.BucketName, existing.ThumbnailObjectKey, cancellationToken);
            }

            repo.Remove(existing);
        }
    }
}

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
    IFileUploadSessionStore uploadSessionStore,
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

        var session = uploadSessionStore.Get(command.UploadId);
        if (session is null || session.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Upload session not found or expired."], ApiStatusCodes.NotFound);
        }

        if (session.TenantId != tenantContext.TenantId || session.CompanyId != tenantContext.CompanyId)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Upload session does not match tenant context."], ApiStatusCodes.Forbidden);
        }

        if (!await objectStorageService.ObjectExistsAsync(session.BucketName, session.ObjectKey, cancellationToken))
        {
            return ApiResponse<FileVariantSetDto>.Fail(["Uploaded object was not found in storage."], ApiStatusCodes.NotFound);
        }

        await using var sourcePayload = await objectStorageService.GetObjectAsync(
            session.BucketName,
            session.ObjectKey,
            cancellationToken);

        var generatedVariants = await imageVariantGenerator.GenerateVariantsAsync(
            sourcePayload.Content,
            session.ContentType,
            session.RequestedVariants,
            cancellationToken);

        if (generatedVariants.Count == 0)
        {
            return ApiResponse<FileVariantSetDto>.Fail(["No logo variants were generated."], ApiStatusCodes.BadRequest);
        }

        var downloadExpiry = TimeSpan.FromMinutes(fileOptions.Value.DownloadUrlExpiryMinutes);
        var now = DateTimeOffset.UtcNow;
        var variantResults = new Dictionary<string, FileVariantInfoDto>(StringComparer.OrdinalIgnoreCase);
        long sourceFileId = 0;

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            foreach (var (variant, generated) in generatedVariants)
            {
                await SupersedeExistingVariantAsync(session, variant, ct);

                var variantFileName = BuildVariantFileName(session.StoredFileName, variant, generated.FileExtension);
                var variantObjectKey = objectKeyBuilder.BuildCompanyAssetKey(
                    session.CompanyId,
                    session.EntityType,
                    session.EntityId,
                    variantFileName);

                await using var uploadStream = new MemoryStream(generated.Content);
                await objectStorageService.PutObjectAsync(
                    session.BucketName,
                    variantObjectKey,
                    uploadStream,
                    generated.ContentType,
                    ct);

                var tags = session.BaseTags
                    .Concat(FileLogoVariants.BuildVariantTags(variant))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                var file = new FgsFile
                {
                    TenantId = session.TenantId,
                    CompanyId = session.CompanyId,
                    EntityType = session.EntityType,
                    EntityId = session.EntityId,
                    BucketName = session.BucketName,
                    ObjectKey = variantObjectKey,
                    ThumbnailObjectKey = objectKeyBuilder.BuildThumbnailKey(variantObjectKey),
                    OriginalFileName = session.OriginalFileName,
                    StoredFileName = variantFileName,
                    ContentType = generated.ContentType,
                    FileExtension = generated.FileExtension,
                    FileSizeBytes = generated.Content.Length,
                    Description = session.Description,
                    Tags = tags,
                    UploadedByName = "TenantAdmin",
                    UploadedByType = "User",
                    CreatedOn = now,
                    CreatedBy = "TenantAdmin"
                };

                await unitOfWork.Repository<FgsFile>().AddAsync(file, ct);
                await unitOfWork.SaveChangesAsync(ct);

                if (sourceFileId == 0)
                {
                    sourceFileId = file.Id;
                }

                var downloadUrl = await objectStorageService.CreateDownloadUrlAsync(
                    session.BucketName,
                    variantObjectKey,
                    downloadExpiry,
                    ct);

                variantResults[variant] = new FileVariantInfoDto(
                    file.Id,
                    contentUrlBuilder.BuildContentUrl(file.Id),
                    downloadUrl,
                    generated.ContentType,
                    generated.Content.Length);
            }

            await objectStorageService.DeleteObjectAsync(session.BucketName, session.ObjectKey, ct);
        }, cancellationToken);

        uploadSessionStore.Remove(command.UploadId);

        return ApiResponse<FileVariantSetDto>.Ok(new FileVariantSetDto(
            sourceFileId,
            variantResults));
    }

    private async Task SupersedeExistingVariantAsync(
        FileUploadSession session,
        string variant,
        CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<FgsFile>();
        var existingFiles = await repo.ListAsync(
            file => file.EntityType == session.EntityType
                    && file.EntityId == session.EntityId
                    && file.Tags != null
                    && file.Tags.Contains(FileLogoVariants.LogoTag)
                    && file.Tags.Contains(variant),
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

    private static string BuildVariantFileName(string storedFileName, string variant, string extension)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(storedFileName);
        return $"{nameWithoutExtension}-{variant}{extension}";
    }
}

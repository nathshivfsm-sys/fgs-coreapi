using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Files.Commands.CreateUploadUrl;

public sealed class CreateUploadUrlCommandHandler(
    IUserTenantClient userTenantClient,
    ITenantContextAccessor tenantContextAccessor,
    IS3ObjectKeyBuilder objectKeyBuilder,
    IS3ObjectStorageService objectStorageService,
    IFileUploadSessionStore uploadSessionStore,
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

        if (request.EntityType.Equals("Company", StringComparison.OrdinalIgnoreCase)
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

        var storedFileName = BuildStoredFileName(request.FileName);
        var objectKey = objectKeyBuilder.BuildCompanyAssetKey(
            tenantContext.CompanyId,
            request.EntityType,
            request.EntityId,
            storedFileName);

        var expiry = TimeSpan.FromMinutes(options.UploadUrlExpiryMinutes);
        var expiresAt = DateTimeOffset.UtcNow.Add(expiry);
        var uploadId = Guid.NewGuid();
        var uploadUrl = await objectStorageService.CreateUploadUrlAsync(
            bucketName,
            objectKey,
            request.ContentType,
            expiry,
            cancellationToken);

        var baseTags = request.Tags?.Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];

        uploadSessionStore.Save(new FileUploadSession
        {
            UploadId = uploadId,
            TenantId = tenantContext.TenantId,
            CompanyId = tenantContext.CompanyId,
            EntityType = request.EntityType.Trim(),
            EntityId = request.EntityId,
            BucketName = bucketName,
            ObjectKey = objectKey,
            OriginalFileName = request.FileName.Trim(),
            StoredFileName = storedFileName,
            ContentType = request.ContentType.Trim(),
            FileSizeBytes = request.FileSizeBytes,
            BaseTags = baseTags,
            RequestedVariants = request.RequestedVariants
                .Select(variant => variant.ToLowerInvariant())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            ExpiresAt = expiresAt
        });

        return ApiResponse<CreateFileUploadUrlResponse>.Ok(new CreateFileUploadUrlResponse(
            uploadId,
            uploadUrl,
            "PUT",
            new Dictionary<string, string> { ["Content-Type"] = request.ContentType.Trim() },
            objectKey,
            expiresAt));
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

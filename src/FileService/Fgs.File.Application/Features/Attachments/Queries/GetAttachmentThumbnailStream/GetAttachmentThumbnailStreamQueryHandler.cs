using Fgs.Contracts.Api;
using Fgs.File.Application.Common;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Features.Attachments.Models;
using Fgs.File.Application.Features.Attachments.Queries.GetAttachmentThumbnailStream;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.GetAttachmentThumbnailStream;

public sealed class GetAttachmentThumbnailStreamQueryHandler(
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetAttachmentThumbnailStreamQuery, ApiResponse<AttachmentStreamModel>>
{
    public async Task<ApiResponse<AttachmentStreamModel>> Handle(
        GetAttachmentThumbnailStreamQuery request,
        CancellationToken cancellationToken)
    {
        var file = await FindActiveAttachmentAsync(request.AttachmentId, request.EntityType, cancellationToken);
        if (file is null)
        {
            return ApiResponse<AttachmentStreamModel>.Fail(["Attachment not found."], ApiStatusCodes.NotFound);
        }

        if (string.IsNullOrWhiteSpace(file.ThumbnailObjectKey))
        {
            return ApiResponse<AttachmentStreamModel>.Fail(["Thumbnail not found."], ApiStatusCodes.NotFound);
        }

        var result = await fileStorageService.OpenReadAsync(
            new StorageObjectRef(file.BucketName, file.ThumbnailObjectKey),
            range: null,
            cancellationToken);

        var thumbFileName = Path.GetFileName(file.ThumbnailObjectKey);
        return ApiResponse<AttachmentStreamModel>.Ok(new AttachmentStreamModel
        {
            Content = result.Content,
            ContentType = result.ContentType,
            ContentLength = result.ContentLength,
            ETag = result.ETag,
            LastModified = result.LastModified,
            FileDownloadName = thumbFileName
        });
    }

    private async Task<FgsFile?> FindActiveAttachmentAsync(
        long attachmentId,
        string entityType,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is null)
        {
            return null;
        }

        var file = await unitOfWork.Repository<FgsFile>()
            .FirstOrDefaultAsync(
                f => f.Id == attachmentId
                     && (f.Tags == null || !f.Tags.Contains(AttachmentDeletionTags.DeletedTag)),
                cancellationToken);

        if (file is null
            || file.TenantId != tenantContext.TenantId
            || file.CompanyId != tenantContext.CompanyId
            || !FileEntityTypes.RouteValueMatchesStorage(entityType, file.EntityType))
        {
            return null;
        }

        return file;
    }
}

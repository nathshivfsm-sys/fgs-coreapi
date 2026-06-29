using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Domain.Entities;

namespace Fgs.File.Application.Features.Attachments;

public static class AttachmentMetadataMapper
{
    public static AttachmentMetadataDto ToDto(FgsFile file, IAttachmentUrlBuilder urlBuilder)
    {
        AttachmentCategoryTags.TryGetCategory(file.Tags, out var category);

        return new AttachmentMetadataDto(
            file.Id,
            file.TenantId,
            file.CompanyId,
            file.EntityType,
            file.EntityId,
            string.IsNullOrWhiteSpace(category) ? null : category,
            file.OriginalFileName,
            file.StoredFileName,
            file.ContentType ?? "application/octet-stream",
            file.FileExtension,
            file.FileSizeBytes,
            file.Tags,
            file.Description,
            file.IsVisibleToCustomer,
            file.IsVisibleToFieldTechnician,
            file.UploadedByUserId,
            file.UploadedByName,
            file.UploadedByType,
            file.CreatedOn,
            file.CreatedBy,
            file.UpdatedOn,
            file.UpdatedBy,
            file.CreatedOn,
            AttachmentDeletionTags.IsActive(file.Tags),
            urlBuilder.BuildDownloadUrl(file.EntityType, file.Id),
            urlBuilder.BuildThumbnailUrl(file.EntityType, file.Id),
            urlBuilder.BuildMetadataUrl(file.EntityType, file.Id));
    }
}

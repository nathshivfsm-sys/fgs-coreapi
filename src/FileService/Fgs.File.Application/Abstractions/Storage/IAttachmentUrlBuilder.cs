namespace Fgs.File.Application.Abstractions.Storage;

public interface IAttachmentUrlBuilder
{
    string BuildDownloadUrl(string entityType, long attachmentId);

    string BuildThumbnailUrl(string entityType, long attachmentId);

    string BuildMetadataUrl(string entityType, long attachmentId);
}

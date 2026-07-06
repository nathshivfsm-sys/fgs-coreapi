using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Fgs.File.API.Services;

public sealed class AttachmentUrlBuilder(
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor,
    IOptions<FileServiceOptions> fileOptions) : IAttachmentUrlBuilder
{
    public string BuildDownloadUrl(string entityType, long attachmentId) =>
        BuildUrl(AttachmentRouteNames.Download, entityType, attachmentId);

    public string BuildThumbnailUrl(string entityType, long attachmentId) =>
        BuildUrl(AttachmentRouteNames.Thumbnail, entityType, attachmentId);

    public string BuildMetadataUrl(string entityType, long attachmentId) =>
        BuildUrl(AttachmentRouteNames.Metadata, entityType, attachmentId);

    private string BuildUrl(string routeName, string entityType, long attachmentId)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            var uri = linkGenerator.GetUriByName(
                httpContext,
                routeName,
                values: new { entityType, attachmentId, version = "1.0" });
            if (!string.IsNullOrWhiteSpace(uri))
            {
                return uri;
            }
        }

        var baseUrl = fileOptions.Value.PublicBaseUrl.TrimEnd('/');
        var encodedEntityType = Uri.EscapeDataString(entityType);
        return routeName switch
        {
            AttachmentRouteNames.Download => $"{baseUrl}/api/v1/attachment/{encodedEntityType}/{attachmentId}",
            AttachmentRouteNames.Thumbnail => $"{baseUrl}/api/v1/attachment/{encodedEntityType}/{attachmentId}/thumbnail",
            AttachmentRouteNames.Metadata => $"{baseUrl}/api/v1/attachment/{encodedEntityType}/{attachmentId}/metadata",
            _ => $"{baseUrl}/api/v1/attachment/{encodedEntityType}/{attachmentId}"
        };
    }
}

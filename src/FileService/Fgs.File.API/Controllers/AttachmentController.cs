using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Application.Features.Attachments.Commands.BulkSoftDeleteAttachmentsByEntity;
using Fgs.File.Application.Features.Attachments.Commands.SoftDeleteAttachment;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;
using Fgs.File.Application.Features.Attachments.Queries.GetAttachmentMetadata;
using Fgs.File.Application.Features.Attachments.Queries.GetAttachmentStream;
using Fgs.File.Application.Features.Attachments.Queries.GetAttachmentThumbnailStream;
using Fgs.File.Application.Features.Attachments.Queries.ListAttachments;
using Fgs.File.Application.Features.Attachments.Models;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Fgs.File.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("attachment")]
public sealed class AttachmentController(
    IMediator mediator,
    IOptions<FileServiceOptions> fileOptions) : FgsApiControllerBase(mediator)
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<AttachmentMetadataDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(524288000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] string entityType,
        [FromForm] long entityId,
        [FromForm] string category,
        [FromForm] string? description,
        [FromForm] string? tags,
        [FromForm] bool isVisibleToCustomer = true,
        [FromForm] bool isVisibleToFieldTechnician = true,
        [FromForm] string? logoVariant = null,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(ApiResponse<object>.Fail(["File is required."], ApiStatusCodes.BadRequest));
        }

        if (file.Length > fileOptions.Value.MaxUploadSizeBytes)
        {
            return BadRequest(ApiResponse<object>.Fail(
                [$"File size exceeds the maximum allowed size of {fileOptions.Value.MaxUploadSizeBytes} bytes."],
                ApiStatusCodes.BadRequest));
        }

        var parsedTags = string.IsNullOrWhiteSpace(tags)
            ? null
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        await using var stream = file.OpenReadStream();
        var response = await Mediator.Send(new UploadAttachmentCommand(
            stream,
            file.FileName,
            file.ContentType ?? "application/octet-stream",
            file.Length,
            entityType,
            entityId,
            category,
            description,
            parsedTags,
            isVisibleToCustomer,
            isVisibleToFieldTechnician,
            logoVariant), cancellationToken);

        return CreatedFromApiResponse(response);
    }

    [HttpGet("{entityType}/{attachmentId:long}", Name = AttachmentRouteNames.Download)]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> Download(string entityType, long attachmentId, CancellationToken cancellationToken)
    {
        var range = ParseRangeHeader();
        var response = await Mediator.Send(new GetAttachmentStreamQuery(attachmentId, entityType, range), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        return BuildFileStreamResult(response.Data, enableRangeProcessing: true);
    }

    [HttpGet("{entityType}/{attachmentId:long}/thumbnail", Name = AttachmentRouteNames.Thumbnail)]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> Thumbnail(string entityType, long attachmentId, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetAttachmentThumbnailStreamQuery(attachmentId, entityType), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        return BuildFileStreamResult(response.Data, enableRangeProcessing: false);
    }

    [HttpGet("{entityType}/{attachmentId:long}/metadata", Name = AttachmentRouteNames.Metadata)]
    [ProducesResponseType(typeof(ApiResponse<AttachmentMetadataDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetadata(string entityType, long attachmentId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetAttachmentMetadataQuery(attachmentId, entityType), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AttachmentMetadataDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? entityType = null,
        [FromQuery] long? entityId = null,
        [FromQuery] bool? isVisibleToCustomer = null,
        [FromQuery] bool? isVisibleToFieldTechnician = null,
        [FromQuery] string? category = null,
        [FromQuery] string? contentType = null,
        [FromQuery] string? extension = null,
        [FromQuery] string? fileName = null,
        [FromQuery] string? uploadedBy = null,
        [FromQuery] long? uploadedByUserId = null,
        [FromQuery] string? tags = null,
        CancellationToken cancellationToken = default)
    {
        var parsedTags = string.IsNullOrWhiteSpace(tags)
            ? null
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var query = new ListAttachmentsQuery(
            new AttachmentListQuery(page, pageSize, sortBy, sortDirection, search),
            new AttachmentListFilters(
                entityType,
                entityId,
                isVisibleToCustomer,
                isVisibleToFieldTechnician,
                category,
                contentType,
                extension,
                fileName,
                uploadedBy,
                uploadedByUserId,
                parsedTags));

        return FromApiResponse(await Mediator.Send(query, cancellationToken));
    }

    [HttpDelete("{attachmentId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long attachmentId, CancellationToken cancellationToken) =>
        NoContentFromApiResponse(await Mediator.Send(new SoftDeleteAttachmentCommand(attachmentId), cancellationToken));

    [HttpDelete("by-entity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> BulkDeleteByEntity(
        [FromQuery] string entityType,
        [FromQuery] long entityId,
        [FromQuery] string? category = null,
        CancellationToken cancellationToken = default) =>
        NoContentFromApiResponse(await Mediator.Send(
            new BulkSoftDeleteAttachmentsByEntityCommand(entityType, entityId, category),
            cancellationToken));

    private StorageByteRange? ParseRangeHeader()
    {
        var rangeHeader = Request.Headers.Range.ToString();
        if (string.IsNullOrWhiteSpace(rangeHeader) || !rangeHeader.StartsWith("bytes=", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var rangeValue = rangeHeader["bytes=".Length..].Split('-', 2);
        if (!long.TryParse(rangeValue[0], out var start))
        {
            return null;
        }

        long? end = rangeValue.Length > 1 && long.TryParse(rangeValue[1], out var parsedEnd)
            ? parsedEnd
            : null;

        return new StorageByteRange(start, end);
    }

    private IActionResult BuildFileStreamResult(
        AttachmentStreamModel model,
        bool enableRangeProcessing)
    {
        Response.Headers[HeaderNames.CacheControl] = $"private, max-age={fileOptions.Value.StreamCacheControlMaxAgeSeconds}";
        if (!string.IsNullOrWhiteSpace(model.ETag))
        {
            Response.Headers[HeaderNames.ETag] = model.ETag;
        }

        if (model.LastModified.HasValue)
        {
            Response.Headers[HeaderNames.LastModified] = model.LastModified.Value.ToString("R");
        }

        return new FileStreamResult(model.Content, model.ContentType)
        {
            EnableRangeProcessing = enableRangeProcessing,
            FileDownloadName = model.FileDownloadName,
            LastModified = model.LastModified
        };
    }
}

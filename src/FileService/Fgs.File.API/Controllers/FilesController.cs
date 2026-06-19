using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Features.Files.Commands.CompleteUpload;
using Fgs.File.Application.Features.Files.Commands.CreateUploadUrl;
using Fgs.File.Application.Features.Files.Queries.GetFileById;
using Fgs.File.Application.Features.Files.Queries.GetFileContentUrl;
using Fgs.File.Application.Features.Files.Queries.GetFilesByEntity;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.File.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("files")]
public sealed class FilesController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpPost("upload-url")]
    [ProducesResponseType(typeof(ApiResponse<CreateFileUploadUrlResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUploadUrl(
        [FromBody] CreateFileUploadUrlRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new CreateUploadUrlCommand(request), cancellationToken));

    [HttpPost("{uploadId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<FileVariantSetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteUpload(
        Guid uploadId,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new CompleteUploadCommand(uploadId), cancellationToken));

    [HttpGet("by-entity")]
    [ProducesResponseType(typeof(ApiResponse<CompanyLogoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEntity(
        [FromQuery] string entityType,
        [FromQuery] long entityId,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new GetFilesByEntityQuery(entityType, entityId),
            cancellationToken));

    [HttpGet("{fileId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FileMetadataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFile(long fileId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFileByIdQuery(fileId), cancellationToken));

    [HttpGet("{fileId:long}/content")]
    [ProducesResponseType(typeof(ApiResponse<FileContentUrlResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFileContent(long fileId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFileContentUrlQuery(fileId), cancellationToken));
}

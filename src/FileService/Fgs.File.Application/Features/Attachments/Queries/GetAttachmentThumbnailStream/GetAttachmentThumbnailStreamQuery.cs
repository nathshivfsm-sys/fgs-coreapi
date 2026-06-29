using Fgs.Contracts.Api;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Features.Attachments.Models;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.GetAttachmentThumbnailStream;

public sealed record GetAttachmentThumbnailStreamQuery(long AttachmentId, string EntityType)
    : IRequest<ApiResponse<AttachmentStreamModel>>;

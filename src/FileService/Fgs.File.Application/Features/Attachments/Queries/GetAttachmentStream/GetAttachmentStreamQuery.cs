using Fgs.Contracts.Api;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Features.Attachments.Models;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.GetAttachmentStream;

public sealed record GetAttachmentStreamQuery(long AttachmentId, string EntityType, StorageByteRange? Range)
    : IRequest<ApiResponse<AttachmentStreamModel>>;

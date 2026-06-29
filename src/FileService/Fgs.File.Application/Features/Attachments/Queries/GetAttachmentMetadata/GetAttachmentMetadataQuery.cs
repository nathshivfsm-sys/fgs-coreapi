using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.GetAttachmentMetadata;

public sealed record GetAttachmentMetadataQuery(long AttachmentId, string EntityType)
    : IRequest<ApiResponse<AttachmentMetadataDto>>;

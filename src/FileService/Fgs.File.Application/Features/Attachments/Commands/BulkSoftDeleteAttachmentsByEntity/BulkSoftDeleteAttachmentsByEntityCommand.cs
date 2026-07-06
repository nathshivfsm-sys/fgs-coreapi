using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Commands.BulkSoftDeleteAttachmentsByEntity;

public sealed record BulkSoftDeleteAttachmentsByEntityCommand(
    string EntityType,
    long EntityId,
    string? Category) : IRequest<ApiResponse<object>>;

using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Commands.SoftDeleteAttachment;

public sealed record SoftDeleteAttachmentCommand(long AttachmentId)
    : IRequest<ApiResponse<object>>;

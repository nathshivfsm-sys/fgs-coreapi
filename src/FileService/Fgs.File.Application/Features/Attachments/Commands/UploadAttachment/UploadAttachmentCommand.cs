using Fgs.Contracts.Clients;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;

public sealed record UploadAttachmentCommand(
    Stream FileContent,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    string EntityType,
    long EntityId,
    string Category,
    string? Description,
    IReadOnlyList<string>? Tags,
    bool IsVisibleToCustomer,
    bool IsVisibleToFieldTechnician,
    string? LogoVariant) : IRequest<ApiResponse<AttachmentMetadataDto>>;

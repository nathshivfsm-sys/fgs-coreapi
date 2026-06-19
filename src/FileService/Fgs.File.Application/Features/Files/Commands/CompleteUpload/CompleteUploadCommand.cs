using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Commands.CompleteUpload;

public sealed record CompleteUploadCommand(Guid UploadId)
    : IRequest<ApiResponse<FileVariantSetDto>>;

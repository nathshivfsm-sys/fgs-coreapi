using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Commands.CompleteUpload;

public sealed record CompleteUploadCommand(long FileId)
    : IRequest<ApiResponse<FileVariantSetDto>>;

using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Commands.CreateUploadUrl;

public sealed record CreateUploadUrlCommand(CreateFileUploadUrlRequest Request)
    : IRequest<ApiResponse<CreateFileUploadUrlResponse>>;

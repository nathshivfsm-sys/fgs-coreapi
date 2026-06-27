using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Queries.GetFileContentUrl;

public sealed record GetFileContentUrlQuery(long FileId)
    : IRequest<ApiResponse<FileContentUrlResponse>>;

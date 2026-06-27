using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Queries.GetFileById;

public sealed record GetFileByIdQuery(long FileId)
    : IRequest<ApiResponse<FileMetadataDto>>;

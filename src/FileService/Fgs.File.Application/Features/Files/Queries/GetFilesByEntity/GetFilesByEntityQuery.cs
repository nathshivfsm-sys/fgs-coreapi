using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.Files.Queries.GetFilesByEntity;

public sealed record GetFilesByEntityQuery(string EntityType, long EntityId)
    : IRequest<ApiResponse<CompanyLogoDto>>;

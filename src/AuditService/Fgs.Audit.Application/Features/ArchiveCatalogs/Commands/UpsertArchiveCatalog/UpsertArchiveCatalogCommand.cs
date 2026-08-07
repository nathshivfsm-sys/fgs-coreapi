using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Commands.UpsertArchiveCatalog;

public sealed record UpsertArchiveCatalogCommand(UpsertArchiveCatalogRequest Request)
    : IRequest<ApiResponse<ArchiveCatalogDto>>;

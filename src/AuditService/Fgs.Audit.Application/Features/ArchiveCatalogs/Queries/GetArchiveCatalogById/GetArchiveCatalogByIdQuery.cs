using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.GetArchiveCatalogById;

public sealed record GetArchiveCatalogByIdQuery(long Id)
    : IRequest<ApiResponse<ArchiveCatalogDto>>;

using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.ListArchiveCatalogs;

public sealed record ListArchiveCatalogsQuery
    : IRequest<ApiResponse<IReadOnlyList<ArchiveCatalogDto>>>;

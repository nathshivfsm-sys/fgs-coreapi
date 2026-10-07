using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.LookupSources;

public sealed record LookupSourcesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<SourceLookupDto>>>;

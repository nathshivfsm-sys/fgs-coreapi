using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.LookupTitlesOfCourtesy;

public sealed record LookupTitlesOfCourtesyQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<TitleOfCourtesyLookupDto>>>;

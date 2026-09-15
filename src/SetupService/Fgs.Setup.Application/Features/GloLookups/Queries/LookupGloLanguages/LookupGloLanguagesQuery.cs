using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLanguages;

public sealed record LookupGloLanguagesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloLanguageLookupDto>>>;

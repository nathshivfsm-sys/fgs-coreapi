using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.LookupResolutionCodes;

public sealed record LookupResolutionCodesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>>;

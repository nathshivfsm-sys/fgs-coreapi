using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;

public sealed record GetLookupsQuery(IReadOnlyList<LookupRequestDto> Requests)
    : IRequest<ApiResponse<IReadOnlyList<LookupResultDto>>>;

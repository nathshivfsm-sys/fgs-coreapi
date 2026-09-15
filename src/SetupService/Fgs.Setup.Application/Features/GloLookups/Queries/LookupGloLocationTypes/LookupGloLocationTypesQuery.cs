using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLocationTypes;

public sealed record LookupGloLocationTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloLocationTypeLookupDto>>>;

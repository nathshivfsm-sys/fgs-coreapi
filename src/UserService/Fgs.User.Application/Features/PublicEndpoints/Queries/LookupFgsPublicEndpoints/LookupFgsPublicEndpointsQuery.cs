using Fgs.Contracts.Api;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.LookupFgsPublicEndpoints;

public sealed record LookupFgsPublicEndpointsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsPublicEndpointLookupDto>>>;

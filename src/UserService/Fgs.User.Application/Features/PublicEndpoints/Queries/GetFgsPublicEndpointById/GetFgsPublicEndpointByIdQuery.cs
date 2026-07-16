using Fgs.Contracts.Api;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.GetFgsPublicEndpointById;

public sealed record GetFgsPublicEndpointByIdQuery(long Id)
    : IRequest<ApiResponse<FgsPublicEndpointDetailDto>>;

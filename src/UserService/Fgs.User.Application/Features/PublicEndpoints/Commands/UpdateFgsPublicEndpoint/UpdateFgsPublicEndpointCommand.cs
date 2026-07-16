using Fgs.Contracts.Api;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.UpdateFgsPublicEndpoint;

public sealed record UpdateFgsPublicEndpointCommand(long Id, FgsPublicEndpointUpdateDto Dto)
    : IRequest<ApiResponse<FgsPublicEndpointDetailDto>>;

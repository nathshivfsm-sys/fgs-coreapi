using Fgs.Contracts.Api;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;

public sealed record CreateFgsPublicEndpointCommand(FgsPublicEndpointCreateDto Dto)
    : IRequest<ApiResponse<FgsPublicEndpointDetailDto>>;

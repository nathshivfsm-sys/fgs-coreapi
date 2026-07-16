using Fgs.Contracts.Api;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.PatchFgsPublicEndpoint;

public sealed record PatchFgsPublicEndpointCommand(long Id, FgsPublicEndpointPatchDto Dto)
    : IRequest<ApiResponse<FgsPublicEndpointDetailDto>>;

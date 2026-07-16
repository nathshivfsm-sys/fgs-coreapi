using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Auth.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;

public sealed record RefreshAuthTokenCommand(string RefreshToken)
    : IRequest<ApiResponse<LoginProfileDto>>;

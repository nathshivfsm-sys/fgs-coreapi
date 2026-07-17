using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Auth.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;

public sealed record ExchangeLoginCodeCommand(string Code, string State)
    : IRequest<ApiResponse<LoginProfileDto>>;

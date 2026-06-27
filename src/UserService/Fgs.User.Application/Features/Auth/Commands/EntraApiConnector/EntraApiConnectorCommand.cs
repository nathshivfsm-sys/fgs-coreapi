using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;

public sealed record EntraApiConnectorCommand(string? Email, string? ObjectId)
    : IRequest<EntraApiConnectorResponseDto>;

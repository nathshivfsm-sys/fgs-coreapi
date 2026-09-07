using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;

public sealed record EntraAttributeCollectionStartCommand(
    EntraAttributeCollectionStartRequestDto Request)
    : IRequest<EntraAttributeCollectionStartResponseDto>;

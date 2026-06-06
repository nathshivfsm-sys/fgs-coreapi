using MediatR;

namespace Fgs.User.Application.Features.Invitations.Commands.StartInvitation;

/// <summary>
/// Validates invite token and returns Entra authorization redirect URL.
/// </summary>
public sealed record StartInvitationCommand(string Token) : IRequest<StartInvitationResult>;

public sealed record StartInvitationResult(bool Success, string? RedirectUrl, string? ErrorMessage);

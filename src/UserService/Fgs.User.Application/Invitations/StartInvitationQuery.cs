using MediatR;

namespace Fgs.User.Application.Invitations;

/// <summary>
/// Validates invite token and returns Entra authorization redirect URL.
/// </summary>
public sealed record StartInvitationQuery(string Token) : IRequest<StartInvitationResult>;

public sealed record StartInvitationResult(bool Success, string? RedirectUrl, string? ErrorMessage);

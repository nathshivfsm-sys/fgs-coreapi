namespace Fgs.User.Application.Abstractions.Identity;

public interface IEntraExternalIdService
{
    string BuildAuthorizationUrl(Guid invitationId, string redirectUri);

    Task<EntraTokenResult> ExchangeCodeAsync(string code, string redirectUri, CancellationToken cancellationToken = default);
}

public sealed record EntraTokenResult(
    string ObjectId,
    string Email,
    string? DisplayName);

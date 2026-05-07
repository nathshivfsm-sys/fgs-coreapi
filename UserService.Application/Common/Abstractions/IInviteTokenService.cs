namespace UserService.Application.Common.Abstractions;

public interface IInviteTokenService
{
    /// <summary>
    /// Returns a URL-safe opaque token and its SHA-256 hash for persistence.
    /// </summary>
    (string PlainTextToken, byte[] TokenHash) CreateTokenWithHash();
}

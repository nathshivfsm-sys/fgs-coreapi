namespace Fgs.User.Application.Abstractions.Security;

public interface IInvitationTokenService
{
    string GenerateToken();

    string HashToken(string token);

    bool VerifyToken(string token, string tokenHash);
}

using System.Security.Cryptography;
using System.Text;
using Fgs.User.Application.Abstractions.Security;

namespace Fgs.User.Infrastructure.Common.Security;

public sealed class InvitationTokenService : IInvitationTokenService
{
    public string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public string HashToken(string token)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }

    public bool VerifyToken(string token, string tokenHash) =>
        CryptographicOperations.FixedTimeEquals(
            Convert.FromHexString(tokenHash),
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}

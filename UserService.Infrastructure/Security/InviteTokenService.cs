using System.Security.Cryptography;
using System.Text;
using UserService.Application.Common.Abstractions;

namespace UserService.Infrastructure.Security;

public sealed class InviteTokenService : IInviteTokenService
{
    public (string PlainTextToken, byte[] TokenHash) CreateTokenWithHash()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var plain = ToUrlSafeBase64(bytes);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(plain));
        return (plain, hash);
    }

    private static string ToUrlSafeBase64(byte[] bytes)
    {
        var s = Convert.ToBase64String(bytes);
        return s.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}

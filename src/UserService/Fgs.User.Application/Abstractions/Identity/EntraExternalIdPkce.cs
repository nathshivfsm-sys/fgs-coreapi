using System.Security.Cryptography;
using System.Text;

namespace Fgs.User.Application.Abstractions.Identity;

/// <summary>
/// PKCE helpers for returning-user login (Application-layer so handlers do not depend on Infrastructure).
/// </summary>
public static class EntraExternalIdPkce
{
    public static (string CodeVerifier, string CodeChallenge) CreatePair()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var verifier = Base64UrlEncode(bytes);
        var challenge = Base64UrlEncode(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)));
        return (verifier, challenge);
    }

    private static string Base64UrlEncode(byte[] input) =>
        Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}

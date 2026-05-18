namespace Fgs.User.Application.Abstractions.Security;

public interface IEmailNormalizer
{
    /// <summary>
    /// Returns a canonical form for case-insensitive email comparison (not for persistence).
    /// </summary>
    string Normalize(string email);
}

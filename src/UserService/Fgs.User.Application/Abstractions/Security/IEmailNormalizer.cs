namespace Fgs.User.Application.Abstractions.Security;

public interface IEmailNormalizer
{
    string Normalize(string email);
}

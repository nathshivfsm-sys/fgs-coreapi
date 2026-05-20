using Fgs.User.Application.Abstractions.Security;

namespace Fgs.User.Infrastructure.Common.Security;

public sealed class EmailNormalizer : IEmailNormalizer
{
    public string Normalize(string email) =>
        email.Trim().ToUpperInvariant();
}

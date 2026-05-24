using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Abstractions.Security;

public interface IJwtTokenService
{
    string CreateToken(FgsUser user, IReadOnlyList<string> roleCodes);
}

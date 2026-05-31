using Microsoft.AspNetCore.Authorization;

namespace Fgs.Security.Authorization.Requirements;

public sealed class FgsRoleRequirement : IAuthorizationRequirement
{
    public FgsRoleRequirement(params string[] roleCodes)
        : this(requireAll: false, roleCodes)
    {
    }

    public FgsRoleRequirement(bool requireAll, params string[] roleCodes)
    {
        if (roleCodes is null || roleCodes.Length == 0)
        {
            throw new ArgumentException("At least one role code is required.", nameof(roleCodes));
        }

        RequireAll = requireAll;
        RoleCodes = roleCodes;
    }

    public bool RequireAll { get; }

    public IReadOnlyList<string> RoleCodes { get; }
}

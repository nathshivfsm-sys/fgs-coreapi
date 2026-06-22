using Fgs.Security.Abstractions;

namespace Fgs.Security.Extensions;

public static class FgsUserContextExtensions
{
    /// <summary>
    /// Resolves the actor stored in audit columns (CreatedBy/UpdatedBy).
    /// Prefers display name over email or user id.
    /// </summary>
    public static string ResolveAuditActor(this IFgsUserContext userContext)
    {
        if (!string.IsNullOrWhiteSpace(userContext.DisplayName))
        {
            return userContext.DisplayName.Trim();
        }

        if (userContext.UserId is Guid userId)
        {
            return userId.ToString();
        }

        return "System";
    }
}

namespace Fgs.User.Application.Abstractions.Identity;

public interface IFgsUserRoleResolver
{
    Task<IReadOnlyList<string>> ResolveRoleCodesAsync(Guid userId, CancellationToken cancellationToken = default);
}

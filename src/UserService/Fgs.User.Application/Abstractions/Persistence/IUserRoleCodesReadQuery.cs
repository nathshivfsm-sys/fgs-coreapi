namespace Fgs.User.Application.Abstractions.Persistence;

public interface IUserRoleCodesReadQuery
{
    Task<IReadOnlyList<string>> GetRoleCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}

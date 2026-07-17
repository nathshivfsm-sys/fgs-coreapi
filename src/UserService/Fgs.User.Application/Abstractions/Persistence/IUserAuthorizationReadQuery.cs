namespace Fgs.User.Application.Abstractions.Persistence;

public interface IUserAuthorizationReadQuery
{
    Task<IReadOnlyList<string>> GetPermissionCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetDataAccessCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}

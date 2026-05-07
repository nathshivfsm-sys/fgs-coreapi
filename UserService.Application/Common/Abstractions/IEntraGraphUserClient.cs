namespace UserService.Application.Common.Abstractions;

public sealed record EntraUserCreationResult(string ObjectId, string UserPrincipalName);

public interface IEntraGraphUserClient
{
    Task<EntraUserCreationResult> CreateExternalIdUserAsync(
        string email,
        string displayName,
        string temporaryPassword,
        CancellationToken cancellationToken = default);
}

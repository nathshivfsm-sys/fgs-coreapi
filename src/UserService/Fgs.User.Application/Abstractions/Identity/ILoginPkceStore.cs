namespace Fgs.User.Application.Abstractions.Identity;

public sealed record LoginPkceState(
    string CodeVerifier,
    string RedirectUri,
    Guid UserId);

public interface ILoginPkceStore
{
    Task SaveAsync(string state, LoginPkceState pkceState, CancellationToken cancellationToken = default);

    Task<LoginPkceState?> TakeAsync(string state, CancellationToken cancellationToken = default);
}

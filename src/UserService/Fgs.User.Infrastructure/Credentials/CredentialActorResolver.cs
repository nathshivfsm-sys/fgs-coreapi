using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Credentials;

namespace Fgs.User.Infrastructure.Credentials;

public sealed class CredentialActorResolver : ICredentialActorResolver
{
    private readonly IFgsUserContext _userContext;

    public CredentialActorResolver(IFgsUserContext userContext) => _userContext = userContext;

    public string ResolveActorId() =>
        _userContext.UserId?.ToString()
        ?? _userContext.Email
        ?? "System";
}

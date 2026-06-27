using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.Setup.Application.Abstractions.Credentials;

namespace Fgs.Setup.Infrastructure.Credentials;

public sealed class CredentialActorResolver : ICredentialActorResolver
{
    private readonly IFgsUserContext _userContext;

    public CredentialActorResolver(IFgsUserContext userContext) => _userContext = userContext;

    public string ResolveActorId() => _userContext.ResolveAuditActor();
}

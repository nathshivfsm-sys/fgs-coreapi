using System.Net;
using Fgs.Foundation.Middleware;
using Fgs.User.Application.Credentials;

namespace Fgs.User.API.Middleware;

public sealed class CredentialSecretsExceptionMapper : IExceptionStatusMapper
{
    public bool TryMap(Exception exception, out ExceptionMapping mapping)
    {
        if (exception is not CredentialSecretsException vaultEx)
        {
            mapping = default!;
            return false;
        }

        mapping = new ExceptionMapping(
            vaultEx.IsAccessDenied ? HttpStatusCode.Forbidden : HttpStatusCode.BadGateway,
            new[] { vaultEx.Message });
        return true;
    }
}

using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.Models;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecret;

public sealed class GetCredentialSecretQueryHandler(ICredentialSecretResolver resolver)
    : IRequestHandler<GetCredentialSecretQuery, CredentialSecretResolution?>
{
    public Task<CredentialSecretResolution?> Handle(
        GetCredentialSecretQuery request,
        CancellationToken cancellationToken) =>
        resolver.ResolveAsync(
            request.TenantId,
            request.CompanyId,
            request.SecretId,
            request.AccessedBy,
            cancellationToken);
}

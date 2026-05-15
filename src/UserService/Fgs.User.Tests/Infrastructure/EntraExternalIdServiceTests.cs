using Fgs.User.Infrastructure.Identity;
using Fgs.User.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure;

public sealed class EntraExternalIdServiceTests
{
    [Fact]
    public void BuildAuthorizationUrl_IncludesStateAndClientId()
    {
        var invitationId = Guid.NewGuid();
        var service = new EntraExternalIdService(
            Options.Create(new EntraExternalIdOptions
            {
                TenantId = "tenant",
                ClientId = "client-id",
                Authority = "https://login.microsoftonline.com",
                Scopes = "openid profile email"
            }),
            new HttpClient());

        var url = service.BuildAuthorizationUrl(invitationId, "https://localhost/callback");

        url.Should().Contain("client_id=client-id");
        url.Should().Contain($"state={invitationId}");
        url.Should().Contain("redirect_uri=");
    }
}

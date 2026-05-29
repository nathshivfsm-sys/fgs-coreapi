using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.Models;
using Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecret;
using Moq;

namespace Fgs.User.Tests.Application.Features.Credentials;

public sealed class GetCredentialSecretQueryHandlerTests
{
    [Fact]
    public async Task Handle_uses_resolver_and_returns_resolution()
    {
        var secretId = Guid.NewGuid();
        var resolution = new CredentialSecretResolution(secretId, "STRIPE", """{"secretKey":"sk"}""", 1);

        var resolver = new Mock<ICredentialSecretResolver>();
        resolver
            .Setup(r => r.ResolveAsync(1, 2, secretId, "user-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(resolution);

        var handler = new GetCredentialSecretQueryHandler(resolver.Object);
        var result = await handler.Handle(
            new GetCredentialSecretQuery
            {
                TenantId = 1,
                CompanyId = 2,
                SecretId = secretId,
                AccessedBy = "user-1"
            },
            CancellationToken.None);

        result.Should().Be(resolution);
        resolver.Verify(
            r => r.ResolveAsync(1, 2, secretId, "user-1", It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

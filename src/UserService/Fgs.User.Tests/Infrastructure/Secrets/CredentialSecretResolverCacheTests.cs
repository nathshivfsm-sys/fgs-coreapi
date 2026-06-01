using Fgs.Foundation.Correlation;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Features.Credentials;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Secrets;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Infrastructure.Secrets;

public sealed class CredentialSecretResolverCacheTests
{
    [Fact]
    public async Task Second_resolve_uses_cache_without_second_aws_call()
    {
        var secretId = Guid.NewGuid();
        var providerId = Guid.NewGuid();
        var secret = new FgsCredentialSecret
        {
            Id = secretId,
            TenantId = 1,
            CompanyId = 2,
            CredentialProviderId = providerId,
            SecretName = "primary",
            VersionNo = 1,
            IsActive = true,
            IsRevoked = false
        };
        CredentialSecretStorageMapping.SetAwsSecretArn(secret, "arn:aws:secretsmanager:us-east-1:123:secret:test");

        var secretRepo = new Mock<IRepository<FgsCredentialSecret>>();
        secretRepo.Setup(r => r.GetByIdAsync(secretId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(secret);

        var provider = new FgsCredentialProvider
        {
            Id = providerId,
            CredentialProviderTypeId = 4,
            Code = "STRIPE",
            Name = "Stripe",
            Environment = "Production",
            TenantId = 1,
            CompanyId = 2
        };

        var providerRepo = new Mock<IRepository<FgsCredentialProvider>>();
        providerRepo.Setup(r => r.GetByIdAsync(providerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var typeRepo = new Mock<IRepository<GloCredentialProviderType>>();
        typeRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<GloCredentialProviderType, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GloCredentialProviderType { Id = 4, Code = "STRIPE", Name = "Stripe" });

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Repository<FgsCredentialSecret>()).Returns(secretRepo.Object);
        unitOfWork.Setup(u => u.Repository<FgsCredentialProvider>()).Returns(providerRepo.Object);
        unitOfWork.Setup(u => u.Repository<GloCredentialProviderType>()).Returns(typeRepo.Object);

        var secretsManager = new Mock<ISecretsManagerService>();
        secretsManager
            .Setup(s => s.GetSecretJsonAsync(secret.EncryptedSecretValue, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync("""{"secretKey":"sk_test"}""");

        var cache = new MemorySecretCache(
            new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()),
            Microsoft.Extensions.Options.Options.Create(new Fgs.User.Infrastructure.Common.Options.AwsCredentialsOptions { CacheTtlSeconds = 60 }));

        var audit = new Mock<ICredentialAuditWriter>();
        var correlation = new Mock<ICorrelationContext>();
        correlation.Setup(c => c.GetCorrelationId()).Returns(Guid.NewGuid());

        var deserializer = new Mock<ICredentialPayloadDeserializer>();
        var connectionBuilder = new Mock<ICredentialConnectionStringBuilder>();

        var sut = new CredentialSecretResolver(
            unitOfWork.Object,
            secretsManager.Object,
            cache,
            audit.Object,
            deserializer.Object,
            connectionBuilder.Object,
            correlation.Object,
            NullLogger<CredentialSecretResolver>.Instance);

        await sut.ResolveAsync(1, 2, secretId, "svc", CancellationToken.None);
        await sut.ResolveAsync(1, 2, secretId, "svc", CancellationToken.None);

        secretsManager.Verify(
            s => s.GetSecretJsonAsync(It.IsAny<string>(), null, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

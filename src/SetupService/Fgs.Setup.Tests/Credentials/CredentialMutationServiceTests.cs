using Fgs.Contracts.CredentialAudit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Credentials.Abstractions;
using Fgs.Messaging.Abstractions;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Queries.GetCredential;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Fgs.Persistence.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.Credentials;

public sealed class CredentialMutationServiceTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateTenantAsync_PersistsEncryptedPayloadWithoutKeyIdentifier()
    {
        var providerType = CreateProviderType();
        var repository = new Mock<ICredentialRepository>();
        repository
            .Setup(r => r.GetProviderTypeByCodeAsync("SMTP", It.IsAny<CancellationToken>()))
            .ReturnsAsync(providerType);
        repository
            .Setup(r => r.GetTenantByProviderTypeAsync(TenantId, CompanyId, providerType.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsCredential?)null);

        FgsCredential? saved = null;
        repository
            .Setup(r => r.AddTenantAsync(It.IsAny<FgsCredential>(), It.IsAny<CancellationToken>()))
            .Callback<FgsCredential, CancellationToken>((credential, _) => saved = credential)
            .Returns(Task.CompletedTask);

        var encryption = new Mock<ICredentialEncryptionService>();
        encryption
            .Setup(e => e.EncryptAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnvelopeEncryptionResult([9, 8, 7], [4, 5, 6]));

        var service = CreateService(repository.Object, encryption.Object);

        var (credential, providerCode) = await service.CreateTenantAsync(
            TenantId,
            CompanyId,
            "SMTP",
            "Primary SMTP",
            "Outbound mail",
            [1, 2, 3],
            CancellationToken.None);

        providerCode.Should().Be("SMTP");
        credential.CredentialData.Should().Equal([9, 8, 7]);
        credential.EncryptedDataKey.Should().Equal([4, 5, 6]);
        saved.Should().NotBeNull();
        saved!.CredentialData.Should().Equal([9, 8, 7]);
        saved.EncryptedDataKey.Should().Equal([4, 5, 6]);
    }

    [Fact]
    public async Task RotateTenantAsync_KmsReEncrypt_UsesNullSourceKeyIdentifier()
    {
        var credentialId = Guid.NewGuid();
        var existing = new FgsCredential
        {
            Id = credentialId,
            TenantId = TenantId,
            CompanyId = CompanyId,
            CredentialProviderTypeId = 1,
            CredentialName = "SMTP",
            CredentialData = [1, 2],
            EncryptedDataKey = [3, 4],
            IsActive = true,
            ProviderType = new GloCredentialProviderTypeCache
            {
                ProviderTypeId = 1,
                ProviderCode = "SMTP",
                ProviderName = "SMTP",
                ConfigurationSchema = "{}",
                IsActive = true
            }
        };

        var repository = new Mock<ICredentialRepository>();
        repository
            .Setup(r => r.GetTenantByIdAsync(credentialId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        string? capturedSourceKey = "unset";
        var encryption = new Mock<ICredentialEncryptionService>();
        encryption
            .Setup(e => e.ReEncryptDataKeyOnlyAsync(
                It.IsAny<byte[]>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Callback<byte[], string?, CancellationToken>((_, source, _) => capturedSourceKey = source)
            .ReturnsAsync([7, 8, 9]);

        var service = CreateService(repository.Object, encryption.Object);

        var rotated = await service.RotateTenantAsync(
            credentialId,
            CredentialRotationMode.KmsReEncrypt,
            CancellationToken.None);

        rotated.CredentialData.Should().Equal([1, 2]);
        rotated.EncryptedDataKey.Should().Equal([7, 8, 9]);
        capturedSourceKey.Should().BeNull();
        encryption.Verify(
            e => e.ReEncryptDataKeyOnlyAsync(
                It.IsAny<byte[]>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DecryptTenantAsync_DelegatesToEncryptionService()
    {
        var credential = new FgsCredential
        {
            Id = Guid.NewGuid(),
            TenantId = TenantId,
            CompanyId = CompanyId,
            CredentialProviderTypeId = 1,
            CredentialName = "SMTP",
            CredentialData = [1, 2],
            EncryptedDataKey = [3, 4],
            IsActive = true
        };

        var encryption = new Mock<ICredentialEncryptionService>();
        encryption
            .Setup(e => e.DecryptAsync(credential.CredentialData, credential.EncryptedDataKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync([5, 6, 7]);

        var service = CreateService(new Mock<ICredentialRepository>().Object, encryption.Object);

        var plaintext = await service.DecryptTenantAsync(credential, CancellationToken.None);

        plaintext.Should().Equal([5, 6, 7]);
    }

    [Fact]
    public async Task GetCredentialHandler_DetailDto_ExcludesKeyIdentifier()
    {
        var credentialId = Guid.NewGuid();
        var credential = new FgsCredential
        {
            Id = credentialId,
            TenantId = TenantId,
            CompanyId = CompanyId,
            CredentialProviderTypeId = 1,
            CredentialName = "SMTP",
            CredentialData = [1],
            EncryptedDataKey = [2],
            IsActive = true,
            ProviderType = new GloCredentialProviderTypeCache
            {
                ProviderTypeId = 1,
                ProviderCode = "SMTP",
                ProviderName = "SMTP Provider",
                ConfigurationSchema = "{}",
                IsActive = true
            }
        };

        var repository = new Mock<ICredentialRepository>();
        repository
            .Setup(r => r.GetTenantByIdAsync(credentialId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);

        var handler = new GetCredentialQueryHandler(repository.Object);
        var response = await handler.Handle(
            new GetCredentialQuery(CredentialScope.Tenant, credentialId.ToString("D")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Should().BeEquivalentTo(
            new CredentialDetailDto(
                CredentialScope.Tenant,
                credentialId.ToString("D"),
                "SMTP",
                "SMTP Provider",
                "SMTP",
                null,
                true));
    }

    private static CredentialMutationService CreateService(
        ICredentialRepository repository,
        ICredentialEncryptionService encryptionService)
    {
        var actorResolver = new Mock<ICredentialActorResolver>();
        actorResolver.Setup(a => a.ResolveActorId()).Returns("test-user");

        var dateTimeProvider = new Mock<Fgs.Setup.Application.Abstractions.Time.IDateTimeProvider>();
        dateTimeProvider.Setup(d => d.UtcNow).Returns(DateTimeOffset.UtcNow);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var configurationProvider = new Mock<ICredentialConfigurationProvider>();
        configurationProvider
            .Setup(c => c.ReloadAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var auditRecorder = new Mock<ICredentialAuditRecorder>();
        auditRecorder
            .Setup(a => a.RecordAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var outboxWriter = new Mock<IOutboxWriter>();
        outboxWriter
            .Setup(o => o.EnqueueAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new CredentialMutationService(
            repository,
            encryptionService,
            actorResolver.Object,
            dateTimeProvider.Object,
            unitOfWork.Object,
            configurationProvider.Object,
            auditRecorder.Object,
            outboxWriter.Object);
    }

    private static GloCredentialProviderType CreateProviderType() =>
        new()
        {
            Id = 1,
            ProviderCode = "SMTP",
            ProviderName = "SMTP Provider",
            ConfigurationSchema = "{}",
            IsActive = true
        };
}

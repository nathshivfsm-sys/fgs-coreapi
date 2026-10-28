using System.Text;
using Fgs.Contracts.CredentialAudit;
using Fgs.Credentials.Abstractions;
using Fgs.Foundation.Time;
using Fgs.Messaging.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.Queries.GetCompanyProviderCredential;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Entities;
using Moq;

namespace Fgs.Setup.Tests.Credentials;

public sealed class GetCompanyProviderCredentialQueryHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task Handle_ReturnsDecryptedSettings_AndRecordsSecretAccessWithoutPlaintext()
    {
        const string transactionKey = "secret-key";
        var json = $$"""
            {"ApiLoginId":"login-id","PublicClientKey":"public-key","TransactionKey":"{{transactionKey}}","UseSandbox":true}
            """;
        var credential = Credential();
        var repository = new Mock<ICredentialRepository>();
        repository.Setup(r => r.GetProviderTypeByCodeAsync("AUTHORIZE_NET", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Provider(active: true));
        repository.Setup(r => r.GetTenantByProviderTypeAsync(TenantId, CompanyId, 8, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);

        var encryption = new Mock<ICredentialEncryptionService>();
        encryption.Setup(e => e.DecryptAsync(credential.CredentialData, credential.EncryptedDataKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(json));

        RecordCredentialAuditRequest? audit = null;
        var auditRecorder = new Mock<ICredentialAuditRecorder>();
        auditRecorder.Setup(a => a.RecordAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()))
            .Callback<RecordCredentialAuditRequest, CancellationToken>((request, _) => audit = request)
            .Returns(Task.CompletedTask);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(repository.Object, encryption.Object, auditRecorder.Object, unitOfWork.Object, Tenant());
        var response = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.ApiLoginId.Should().Be("login-id");
        response.Data.PublicClientKey.Should().Be("public-key");
        response.Data.TransactionKey.Should().Be(transactionKey);
        response.Data.UseSandbox.Should().BeTrue();
        audit.Should().NotBeNull();
        audit!.ActionType.Should().Be(CredentialAuditActions.SecretAccessed);
        audit.CredentialId.Should().Be(credential.Id);
        audit.Remarks.Should().NotContain(transactionKey);
        audit.Remarks.Should().NotContain("login-id");
        audit.Remarks.Should().NotContain("public-key");
    }

    [Fact]
    public async Task Handle_Returns404_WhenCredentialIsMissingOrIncomplete()
    {
        var repository = new Mock<ICredentialRepository>();
        repository.Setup(r => r.GetProviderTypeByCodeAsync("AUTHORIZE_NET", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Provider(active: true));
        repository.Setup(r => r.GetTenantByProviderTypeAsync(TenantId, CompanyId, 8, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsCredential?)null);
        var auditRecorder = new Mock<ICredentialAuditRecorder>(MockBehavior.Strict);
        var handler = CreateHandler(
            repository.Object,
            new Mock<ICredentialEncryptionService>().Object,
            auditRecorder.Object,
            new Mock<IUnitOfWork>().Object,
            Tenant());

        var missing = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);

        missing.Success.Should().BeFalse();
        missing.StatusCode.Should().Be(404);

        var credential = Credential();
        repository.Setup(r => r.GetTenantByProviderTypeAsync(TenantId, CompanyId, 8, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);
        var encryption = new Mock<ICredentialEncryptionService>();
        encryption.Setup(e => e.DecryptAsync(It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes("""{"ApiLoginId":"login-id"}"""));
        handler = CreateHandler(repository.Object, encryption.Object, auditRecorder.Object, new Mock<IUnitOfWork>().Object, Tenant());

        var incomplete = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);

        incomplete.StatusCode.Should().Be(404);
        auditRecorder.Verify(
            a => a.RecordAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Returns400_WhenProviderCodeOrTenantIsMissing()
    {
        var handler = CreateHandler(
            new Mock<ICredentialRepository>(MockBehavior.Strict).Object,
            new Mock<ICredentialEncryptionService>().Object,
            new Mock<ICredentialAuditRecorder>(MockBehavior.Strict).Object,
            new Mock<IUnitOfWork>().Object,
            Tenant());

        var missingCode = await handler.Handle(new GetCompanyProviderCredentialQuery("  "), CancellationToken.None);
        missingCode.StatusCode.Should().Be(400);

        var accessor = new Mock<ITenantContextAccessor>();
        accessor.SetupGet(a => a.Current).Returns((ITenantContext?)null);
        handler = CreateHandler(
            new Mock<ICredentialRepository>(MockBehavior.Strict).Object,
            new Mock<ICredentialEncryptionService>().Object,
            new Mock<ICredentialAuditRecorder>(MockBehavior.Strict).Object,
            new Mock<IUnitOfWork>().Object,
            accessor.Object);

        var missingTenant = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);
        missingTenant.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Handle_Returns404_WhenProviderOrCredentialIsInactive()
    {
        var repository = new Mock<ICredentialRepository>();
        repository.Setup(r => r.GetProviderTypeByCodeAsync("AUTHORIZE_NET", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Provider(active: false));
        var handler = CreateHandler(
            repository.Object,
            new Mock<ICredentialEncryptionService>().Object,
            new Mock<ICredentialAuditRecorder>(MockBehavior.Strict).Object,
            new Mock<IUnitOfWork>().Object,
            Tenant());

        var inactiveProvider = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);
        inactiveProvider.StatusCode.Should().Be(404);

        repository.Setup(r => r.GetProviderTypeByCodeAsync("AUTHORIZE_NET", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Provider(active: true));
        var credential = Credential();
        credential.IsActive = false;
        repository.Setup(r => r.GetTenantByProviderTypeAsync(TenantId, CompanyId, 8, It.IsAny<CancellationToken>()))
            .ReturnsAsync(credential);
        handler = CreateHandler(
            repository.Object,
            new Mock<ICredentialEncryptionService>(MockBehavior.Strict).Object,
            new Mock<ICredentialAuditRecorder>(MockBehavior.Strict).Object,
            new Mock<IUnitOfWork>().Object,
            Tenant());

        var inactiveCredential = await handler.Handle(new GetCompanyProviderCredentialQuery("AUTHORIZE_NET"), CancellationToken.None);
        inactiveCredential.StatusCode.Should().Be(404);
    }

    private static GetCompanyProviderCredentialQueryHandler CreateHandler(
        ICredentialRepository repository,
        ICredentialEncryptionService encryption,
        ICredentialAuditRecorder auditRecorder,
        IUnitOfWork unitOfWork,
        ITenantContextAccessor tenantContextAccessor)
    {
        var actor = new Mock<ICredentialActorResolver>();
        actor.Setup(a => a.ResolveActorId()).Returns("tester");
        var mutationService = new CredentialMutationService(
            repository,
            encryption,
            actor.Object,
            new Mock<IDateTimeProvider>().Object,
            new Mock<IUnitOfWork>().Object,
            new Mock<ICredentialConfigurationProvider>().Object,
            new Mock<ICredentialAuditRecorder>().Object,
            new Mock<IOutboxWriter>().Object);
        return new GetCompanyProviderCredentialQueryHandler(
            repository,
            mutationService,
            auditRecorder,
            unitOfWork,
            tenantContextAccessor,
            actor.Object);
    }

    private static ITenantContextAccessor Tenant()
    {
        var accessor = new Mock<ITenantContextAccessor>();
        accessor.SetupGet(a => a.Current).Returns(new TenantContext { TenantId = TenantId, CompanyId = CompanyId });
        return accessor.Object;
    }

    private static GloCredentialProviderType Provider(bool active) =>
        new()
        {
            Id = 8,
            ProviderCode = "AUTHORIZE_NET",
            ProviderName = "Authorize.Net",
            ConfigurationSchema = "{}",
            IsActive = active
        };

    private static FgsCredential Credential() =>
        new()
        {
            Id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            TenantId = TenantId,
            CompanyId = CompanyId,
            CredentialProviderTypeId = 8,
            CredentialName = "Authorize.Net",
            CredentialData = [1],
            EncryptedDataKey = [2],
            IsActive = true
        };
}

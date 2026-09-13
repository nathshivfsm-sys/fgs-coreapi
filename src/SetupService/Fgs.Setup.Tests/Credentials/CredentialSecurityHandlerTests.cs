using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Options;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.Queries.GetResolvedCredentialConfiguration;
using Fgs.Setup.Application.Features.Credentials.Queries.ListCredentials;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Setup.Tests.Credentials;

public sealed class CredentialSecurityHandlerTests
{
    [Fact]
    public async Task GetResolved_RejectsUnknownService()
    {
        var provider = new Mock<ICredentialConfigurationProvider>();
        provider.SetupGet(p => p.Values).Returns(new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUser"] = "db"
        });

        var handler = CreateResolvedHandler(provider.Object, "strong-key");
        var response = await handler.Handle(
            new GetResolvedCredentialConfigurationQuery("strong-key", "not-a-service"),
            CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Forbidden);
    }

    [Fact]
    public async Task GetResolved_FiltersProvidersAndExcludesTenantKeys()
    {
        var provider = new Mock<ICredentialConfigurationProvider>();
        provider.SetupGet(p => p.Values).Returns(new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUser"] = "db",
            ["Global:SENDGRID:ApiKey"] = "sg",
            ["Tenant:1:2:DATABASE:Secret"] = "tenant",
            ["Global:REDIS:ConnectionString"] = "redis:6379"
        });

        var handler = CreateResolvedHandler(provider.Object, "strong-key");
        var response = await handler.Handle(
            new GetResolvedCredentialConfigurationQuery("strong-key", "fgs-audit-service"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        response.Data.Values.Should().ContainKey("Global:REDIS:ConnectionString");
        response.Data.Values.Should().NotContainKey("Global:SENDGRID:ApiKey");
        response.Data.Values.Should().NotContainKey("Tenant:1:2:DATABASE:Secret");
    }

    [Fact]
    public async Task GetResolved_RejectsWrongKey()
    {
        var provider = new Mock<ICredentialConfigurationProvider>();
        provider.SetupGet(p => p.Values).Returns(new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUser"] = "db"
        });

        var handler = CreateResolvedHandler(
            provider.Object,
            InternalServiceAuthorization.DevelopInternalServiceKey);
        var response = await handler.Handle(
            new GetResolvedCredentialConfigurationQuery(
                "wrong-key",
                "fgs-audit-service"),
            CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
    }

    [Fact]
    public async Task ListTenant_RequiresTenantContext()
    {
        var repository = new Mock<ICredentialRepository>();
        var accessor = new TestTenantContextAccessor { Current = null };
        var handler = new ListCredentialsQueryHandler(repository.Object, accessor);

        var response = await handler.Handle(
            new ListCredentialsQuery(CredentialScope.Tenant),
            CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        repository.Verify(
            r => r.ListTenantAsync(
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ListTenant_UsesActiveTenantScope()
    {
        var repository = new Mock<ICredentialRepository>();
        repository
            .Setup(r => r.ListTenantAsync(10, 20, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<FgsCredential>());

        var accessor = new TestTenantContextAccessor
        {
            Current = new TestTenantContext(10, 20)
        };
        var handler = new ListCredentialsQueryHandler(repository.Object, accessor);

        var response = await handler.Handle(
            new ListCredentialsQuery(CredentialScope.Tenant, TenantId: 99, CompanyId: 88),
            CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Forbidden);
        repository.Verify(
            r => r.ListTenantAsync(
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static GetResolvedCredentialConfigurationQueryHandler CreateResolvedHandler(
        ICredentialConfigurationProvider provider,
        string configuredKey)
    {
        var audit = new Mock<ICredentialAuditRecorder>();
        audit
            .Setup(a => a.RecordAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var env = new Mock<IHostEnvironment>();
        env.SetupGet(e => e.EnvironmentName).Returns("Development");

        return new GetResolvedCredentialConfigurationQueryHandler(
            provider,
            audit.Object,
            uow.Object,
            Options.Create(new CredentialDistributionOptions { InternalServiceKey = configuredKey }),
            env.Object);
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }

    private sealed record TestTenantContext(long TenantId, long CompanyId) : ITenantContext;
}

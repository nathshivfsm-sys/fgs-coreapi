using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class GetActiveCommunicationTemplateQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenFgsTemplateExists_ReturnsFgsTemplate()
    {
        var fgsTemplate = CreateCandidate(1, 2, id: 10, name: "Tenant override");

        var handler = CreateHandler(fgsTemplates: [fgsTemplate], gloTemplate: null);

        var response = await handler.Handle(
            InternalQuery(1, 2),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(10);
        response.Data.TenantId.Should().Be(1);
        response.Data.CompanyId.Should().Be(2);
        response.Data.Name.Should().Be("Tenant override");
    }

    [Fact]
    public async Task Handle_WhenCompanyTenantAndGlobalExist_ReturnsCompanyScoped()
    {
        var handler = CreateHandler(
            fgsTemplates:
            [
                CreateCandidate(null, null, id: 1, name: "Global"),
                CreateCandidate(100, null, id: 2, name: "Tenant"),
                CreateCandidate(100, 200, id: 3, name: "Company")
            ],
            gloTemplate: null);

        var response = await handler.Handle(InternalQuery(100, 200), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(3);
        response.Data.Name.Should().Be("Company");
    }

    [Fact]
    public async Task Handle_WhenCompanyMissing_FallsBackToTenantScoped()
    {
        var handler = CreateHandler(
            fgsTemplates:
            [
                CreateCandidate(null, null, id: 1, name: "Global"),
                CreateCandidate(100, null, id: 2, name: "Tenant")
            ],
            gloTemplate: null);

        var response = await handler.Handle(InternalQuery(100, 200), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(2);
        response.Data.TenantId.Should().Be(100);
        response.Data.CompanyId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenTenantMissing_FallsBackToGlobal()
    {
        var handler = CreateHandler(
            fgsTemplates: [CreateCandidate(null, null, id: 1, name: "Global")],
            gloTemplate: null);

        var response = await handler.Handle(InternalQuery(999, 888), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
        response.Data.TenantId.Should().BeNull();
        response.Data.CompanyId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenFgsTemplateMissing_FallsBackToGloTemplateByCode()
    {
        var gloTemplate = CreateCandidate(null, null, id: 99, name: "Company Admin Invitation Email") with
        {
            Subject = "Welcome to {{PlatformName}} – Activate Your Admin Account",
            Body = "Hello {{Name}}"
        };

        var handler = CreateHandler(fgsTemplates: [], gloTemplate: gloTemplate);

        var response = await handler.Handle(InternalQuery(1, 2), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(99);
        response.Data.TenantId.Should().BeNull();
        response.Data.CompanyId.Should().BeNull();
        response.Data.Code.Should().Be(CommunicationTemplateCodes.CompanyAdminInvitation);
        response.Data.Subject.Should().Be(gloTemplate.Subject);
    }

    [Fact]
    public async Task Handle_WhenTemplateMissingInBothTables_ReturnsNotFound()
    {
        var handler = CreateHandler(fgsTemplates: [], gloTemplate: null);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(null, null, "EMAIL", "MISSING_CODE", IsInternalService: true),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task Handle_WhenJwtScopeDoesNotMatch_ReturnsForbidden()
    {
        var read = new Mock<IActiveCommunicationTemplateReadRepository>();
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 2 }
        };
        var handler = new GetActiveCommunicationTemplateQueryHandler(read.Object, accessor);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(9, 2, "EMAIL", CommunicationTemplateCodes.CompanyAdminInvitation),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Forbidden);
        read.Verify(
            r => r.ListFgsMatchesAsync(
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static GetActiveCommunicationTemplateQuery InternalQuery(long? tenantId, long? companyId) =>
        new(tenantId, companyId, "EMAIL", CommunicationTemplateCodes.CompanyAdminInvitation, IsInternalService: true);

    private static ActiveCommunicationTemplateCandidate CreateCandidate(
        long? tenantId,
        long? companyId,
        long id,
        string name) =>
        new(
            id,
            tenantId,
            companyId,
            "EMAIL",
            CommunicationTemplateCodes.CompanyAdminInvitation,
            name,
            "Subject",
            "Body",
            true,
            true);

    private static GetActiveCommunicationTemplateQueryHandler CreateHandler(
        IReadOnlyList<ActiveCommunicationTemplateCandidate> fgsTemplates,
        ActiveCommunicationTemplateCandidate? gloTemplate)
    {
        var read = new Mock<IActiveCommunicationTemplateReadRepository>();
        read.Setup(r => r.ListFgsMatchesAsync(
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(fgsTemplates);
        read.Setup(r => r.FindLatestGloAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(gloTemplate);

        return new GetActiveCommunicationTemplateQueryHandler(read.Object, new TestTenantContextAccessor());
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

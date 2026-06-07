using System.Linq.Expressions;
using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Persistence.Abstractions;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;
using Fgs.Setup.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class GetActiveCommunicationTemplateQueryHandlerTests
{
    private const string InternalServiceKey = "test-internal-key";

    [Fact]
    public async Task Handle_WhenFgsTemplateExists_ReturnsFgsTemplate()
    {
        var fgsTemplate = new FgsSetupCommunicationTemplate
        {
            Id = 10,
            TenantId = 1,
            CompanyId = 2,
            TemplateType = "EMAIL",
            Code = CommunicationTemplateCodes.CompanyAdminInvitation,
            Name = "Tenant override",
            Subject = "Subject",
            Body = "Body",
            IsActive = true
        };

        var handler = CreateHandler(
            fgsTemplates: [fgsTemplate],
            gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(1, 2, "EMAIL", CommunicationTemplateCodes.CompanyAdminInvitation, InternalServiceKey),
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
        var global = CreateFgsTemplate(null, null, id: 1, name: "Global");
        var tenantTemplate = CreateFgsTemplate(100, null, id: 2, name: "Tenant");
        var companyTemplate = CreateFgsTemplate(100, 200, id: 3, name: "Company");

        var handler = CreateHandler(
            fgsTemplates: [global, tenantTemplate, companyTemplate],
            gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(
                100,
                200,
                "EMAIL",
                CommunicationTemplateCodes.CompanyAdminInvitation,
                InternalServiceKey),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(3);
        response.Data.Name.Should().Be("Company");
    }

    [Fact]
    public async Task Handle_WhenCompanyMissing_FallsBackToTenantScoped()
    {
        var global = CreateFgsTemplate(null, null, id: 1, name: "Global");
        var tenantTemplate = CreateFgsTemplate(100, null, id: 2, name: "Tenant");

        var handler = CreateHandler(
            fgsTemplates: [global, tenantTemplate],
            gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(
                100,
                200,
                "EMAIL",
                CommunicationTemplateCodes.CompanyAdminInvitation,
                InternalServiceKey),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(2);
        response.Data.TenantId.Should().Be(100);
        response.Data.CompanyId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenTenantMissing_FallsBackToGlobal()
    {
        var global = CreateFgsTemplate(null, null, id: 1, name: "Global");

        var handler = CreateHandler(
            fgsTemplates: [global],
            gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(
                999,
                888,
                "EMAIL",
                CommunicationTemplateCodes.CompanyAdminInvitation,
                InternalServiceKey),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
        response.Data.TenantId.Should().BeNull();
        response.Data.CompanyId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenFgsTemplateMissing_FallsBackToGloTemplateByCode()
    {
        var gloTemplate = new GloCommunicationTemplate
        {
            Id = 99,
            CommunicationChannel = "Email",
            TemplateCode = CommunicationTemplateCodes.CompanyAdminInvitation,
            Name = "Company Admin Invitation Email",
            Subject = "Welcome to {{PlatformName}} – Activate Your Admin Account",
            Body = "Hello {{Name}}",
            IsActive = true
        };

        var handler = CreateHandler(
            fgsTemplates: [],
            gloTemplates: [gloTemplate]);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(1, 2, "EMAIL", CommunicationTemplateCodes.CompanyAdminInvitation, InternalServiceKey),
            CancellationToken.None);

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
        var handler = CreateHandler(fgsTemplates: [], gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(null, null, "EMAIL", "MISSING_CODE", InternalServiceKey),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task Handle_WhenInternalServiceKeyInvalid_ReturnsUnauthorized()
    {
        var handler = CreateHandler(fgsTemplates: [], gloTemplates: []);

        var response = await handler.Handle(
            new GetActiveCommunicationTemplateQuery(null, null, "EMAIL", CommunicationTemplateCodes.CompanyAdminInvitation, "wrong-key"),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
    }

    private static FgsSetupCommunicationTemplate CreateFgsTemplate(
        long? tenantId,
        long? companyId,
        long id,
        string name) =>
        new()
        {
            Id = id,
            TenantId = tenantId,
            CompanyId = companyId,
            TemplateType = "EMAIL",
            Code = CommunicationTemplateCodes.CompanyAdminInvitation,
            Name = name,
            Subject = "Subject",
            Body = "Body",
            IsActive = true
        };

    private static GetActiveCommunicationTemplateQueryHandler CreateHandler(
        IReadOnlyList<FgsSetupCommunicationTemplate> fgsTemplates,
        IReadOnlyList<GloCommunicationTemplate> gloTemplates)
    {
        var fgsRepoMock = new Mock<IRepository<FgsSetupCommunicationTemplate>>();
        fgsRepoMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<FgsSetupCommunicationTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<FgsSetupCommunicationTemplate, bool>> predicate, CancellationToken _) =>
                fgsTemplates.Where(predicate.Compile()).ToList());

        var gloRepoMock = new Mock<IRepository<GloCommunicationTemplate>>();
        gloRepoMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<GloCommunicationTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<GloCommunicationTemplate, bool>> predicate, CancellationToken _) =>
                gloTemplates.Where(predicate.Compile()).ToList());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Repository<FgsSetupCommunicationTemplate>()).Returns(fgsRepoMock.Object);
        unitOfWorkMock.Setup(u => u.Repository<GloCommunicationTemplate>()).Returns(gloRepoMock.Object);

        var options = Options.Create(new CredentialDistributionOptions
        {
            InternalServiceKey = InternalServiceKey
        });

        return new GetActiveCommunicationTemplateQueryHandler(unitOfWorkMock.Object, options);
    }
}

using System.Linq.Expressions;
using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Common.Options;
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

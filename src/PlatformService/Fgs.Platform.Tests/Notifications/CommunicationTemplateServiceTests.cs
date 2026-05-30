using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Platform.Domain.Entities;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Platform.Infrastructure.Database.Seed;
using Fgs.Platform.Infrastructure.Notifications.Templates;
using Fgs.Platform.Tests;

namespace Fgs.Platform.Tests.Notifications;

public sealed class CommunicationTemplateServiceTests
{
    [Fact]
    public async Task GetActiveTemplateAsync_ReturnsCompanyScoped_WhenPresent()
    {
        await using var context = TestDbContextFactory.Create();
        var tenantId = 100L;
        var companyId = 200L;
        var global = CommunicationTemplateSeedData.CompanyAdminInvitationEmail();
        var companyTemplate = new FgsSetupCommunicationTemplate
        {
            Id = 2,
            TenantId = tenantId,
            CompanyId = companyId,
            TemplateType = CommunicationTemplateTypes.Email,
            Code = CommunicationTemplateCodes.CompanyAdminInvitation,
            Name = "Company-scoped invite",
            Subject = "Tenant subject {{Name}}",
            Body = "Company body",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

        context.CommunicationTemplates.Add(global);
        context.CommunicationTemplates.Add(companyTemplate);
        await context.SaveChangesAsync();

        var service = new CommunicationTemplateService(new CommunicationTemplateRepository(context));

        var result = await service.GetActiveTemplateAsync(
            tenantId,
            companyId,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation);

        result.Id.Should().Be(companyTemplate.Id);
    }

    [Fact]
    public async Task GetActiveTemplateAsync_FallsBackToGlobal_WhenScopedTemplateMissing()
    {
        await using var context = TestDbContextFactory.Create();
        var global = CommunicationTemplateSeedData.CompanyAdminInvitationEmail();
        context.CommunicationTemplates.Add(global);
        await context.SaveChangesAsync();

        var service = new CommunicationTemplateService(new CommunicationTemplateRepository(context));

        var result = await service.GetActiveTemplateAsync(
            999L,
            888L,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation);

        result.Id.Should().Be(global.Id);
        result.TenantId.Should().BeNull();
        result.CompanyId.Should().BeNull();
    }

    [Fact]
    public async Task GetActiveTemplateAsync_WhenNotFound_Throws()
    {
        await using var context = TestDbContextFactory.Create();
        var service = new CommunicationTemplateService(new CommunicationTemplateRepository(context));

        var act = () => service.GetActiveTemplateAsync(
            999L,
            null,
            NotificationChannel.Email,
            "MISSING_CODE");

        await act.Should().ThrowAsync<CommunicationTemplateNotFoundException>();
    }
}

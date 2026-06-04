using Fgs.Contracts.IntegrationEvents;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Notifications.Templates;

namespace Fgs.Notification.Tests.Notifications;

public sealed class CommunicationTemplateServiceTests
{
    [Fact]
    public async Task GetActiveTemplateAsync_ReturnsCompanyScoped_WhenPresent()
    {
        var tenantId = 100L;
        var companyId = 200L;
        var global = CreateTemplate(null, null, id: 1);
        var companyTemplate = CreateTemplate(tenantId, companyId, id: 2, subject: "Tenant subject {{Name}}");

        var repository = new StubCommunicationTemplateRepository(
            (tenantId, companyId, _, _) => companyTemplate,
            (_, _, _, _) => global);

        var service = new CommunicationTemplateService(repository);

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
        var global = CreateTemplate(null, null, id: 1);
        var repository = new StubCommunicationTemplateRepository(
            (_, _, _, _) => null,
            (_, _, _, _) => global);

        var service = new CommunicationTemplateService(repository);

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
        var repository = new StubCommunicationTemplateRepository((_, _, _, _) => null);
        var service = new CommunicationTemplateService(repository);

        var act = () => service.GetActiveTemplateAsync(
            999L,
            null,
            NotificationChannel.Email,
            "MISSING_CODE");

        await act.Should().ThrowAsync<CommunicationTemplateNotFoundException>();
    }

    private static FgsSetupCommunicationTemplate CreateTemplate(
        long? tenantId,
        long? companyId,
        long id = 1,
        string? subject = "Welcome to {{PlatformName}} – Activate Your Admin Account") =>
        new()
        {
            Id = id,
            TenantId = tenantId,
            CompanyId = companyId,
            TemplateType = CommunicationTemplateTypes.Email,
            Code = CommunicationTemplateCodes.CompanyAdminInvitation,
            Name = "Company Admin Invitation Email",
            Subject = subject,
            Body = "Hello {{Name}}",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class StubCommunicationTemplateRepository(
        params Func<long?, long?, string, string, FgsSetupCommunicationTemplate?>[] handlers)
        : ICommunicationTemplateRepository
    {
        private int _callIndex;

        public Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
            long? tenantId,
            long? companyId,
            string templateType,
            string code,
            CancellationToken cancellationToken = default)
        {
            var handler = handlers[Math.Min(_callIndex++, handlers.Length - 1)];
            return Task.FromResult(handler(tenantId, companyId, templateType, code));
        }
    }
}

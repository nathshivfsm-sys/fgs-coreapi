using Fgs.Contracts.IntegrationEvents;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Notifications.Templates;

namespace Fgs.Notification.Tests.Notifications;

public sealed class CommunicationTemplateServiceTests
{
    [Fact]
    public async Task GetActiveTemplateAsync_ReturnsTemplateFromRepository()
    {
        var tenantId = 100L;
        var companyId = 200L;
        var expected = CreateTemplate(tenantId, companyId, id: 2, subject: "Tenant subject {{Name}}");

        var repository = new StubCommunicationTemplateRepository(expected);
        var service = new CommunicationTemplateService(repository);

        var result = await service.GetActiveTemplateAsync(
            tenantId,
            companyId,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation);

        result.Id.Should().Be(expected.Id);
    }

    [Fact]
    public async Task GetActiveTemplateAsync_WhenNotFound_Throws()
    {
        var repository = new StubCommunicationTemplateRepository(null);
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
        FgsSetupCommunicationTemplate? template) : ICommunicationTemplateRepository
    {
        public Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
            long? tenantId,
            long? companyId,
            string templateType,
            string code,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(template);
    }
}

using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Platform.Infrastructure.Notifications.Templates;

public sealed class CommunicationTemplateRepository(FgsPlatformDbContext context) : ICommunicationTemplateRepository
{
    public Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        var normalizedType = templateType.Trim();

        return context.CommunicationTemplates
            .AsNoTracking()
            .Where(t =>
                t.TemplateType == normalizedType
                && t.Code == normalizedCode
                && t.IsActive
                && t.TenantId == tenantId
                && t.CompanyId == companyId)
            .OrderByDescending(t => t.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default) =>
        context.CommunicationTemplates.AnyAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(FgsSetupCommunicationTemplate template, CancellationToken cancellationToken = default)
    {
        await context.CommunicationTemplates.AddAsync(template, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}

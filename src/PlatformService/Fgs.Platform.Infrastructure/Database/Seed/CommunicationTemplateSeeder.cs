using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fgs.Platform.Infrastructure.Database.Seed;

public sealed class CommunicationTemplateSeeder(
    FgsPlatformDbContext context,
    ILogger<CommunicationTemplateSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var seeds = new[]
        {
            CommunicationTemplateSeedData.CompanyAdminInvitationEmail()
        };

        foreach (var seed in seeds)
        {
            var existing = await context.CommunicationTemplates
                .FirstOrDefaultAsync(t => t.Id == seed.Id, cancellationToken);

            if (existing is null)
            {
                await context.CommunicationTemplates.AddAsync(seed, cancellationToken);
                logger.LogInformation(
                    "Seeded communication template {Code} (Id={TemplateId}, Type={TemplateType}).",
                    seed.Code,
                    seed.Id,
                    seed.TemplateType);
                continue;
            }

            existing.Subject = seed.Subject;
            existing.Body = seed.Body;
            existing.Name = seed.Name;
            existing.IsActive = seed.IsActive;
            logger.LogInformation(
                "Updated communication template {Code} (Id={TemplateId}, Type={TemplateType}).",
                seed.Code,
                seed.Id,
                seed.TemplateType);
        }

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

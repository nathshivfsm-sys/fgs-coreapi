using Fgs.MultiTenancy;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Setup.Tests.Credentials;

public sealed class CredentialRepositoryScopeTests
{
    [Fact]
    public async Task GetTenantById_DoesNotReturnAnotherCompanysCredential()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 2 }
        };
        await using var context = await CreateContextAsync(accessor);
        var otherId = Guid.NewGuid();
        context.GloCredentialProviderTypeCaches.Add(new GloCredentialProviderTypeCache
        {
            ProviderTypeId = 1,
            ProviderCode = "SMTP",
            ProviderName = "SMTP",
            ConfigurationSchema = "{}"
        });
        context.FgsCredentials.Add(new FgsCredential
        {
            Id = otherId,
            TenantId = 9,
            CompanyId = 9,
            CredentialProviderTypeId = 1,
            CredentialName = "Other",
            CredentialData = [1],
            EncryptedDataKey = [2],
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();

        var repository = new CredentialRepository(context, accessor);

        var found = await repository.GetTenantByIdAsync(otherId, CancellationToken.None);
        var listed = await repository.ListTenantAsync(1, 2, activeOnly: true, CancellationToken.None);
        var snapshot = await repository.ListAllActiveTenantCredentialsAsync(CancellationToken.None);

        found.Should().BeNull();
        listed.Should().BeEmpty();
        snapshot.Should().ContainSingle(c => c.Id == otherId);
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync(ITenantContextAccessor accessor)
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new FgsSetupDbContext(options, accessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

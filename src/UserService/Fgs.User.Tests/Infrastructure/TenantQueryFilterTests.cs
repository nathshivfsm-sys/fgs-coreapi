using Fgs.MultiTenancy;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Infrastructure;

public sealed class TenantQueryFilterTests
{
    [Fact]
    public async Task ListUsers_WhenTenantContextUnresolved_ReturnsAllTenants()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        await SeedTenantCompanyAsync(context, 1, 1);
        await SeedTenantCompanyAsync(context, 2, 1);
        context.FgsUsers.AddRange(
            CreateUser(1, 1, "a@test.com"),
            CreateUser(2, 1, "b@test.com"));
        await context.SaveChangesAsync();

        var users = await context.FgsUsers.ToListAsync();

        users.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListUsers_WhenTenantContextResolved_FiltersByTenantAndCompany()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = 1,
                CompanyId = 1
            }
        };

        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        await SeedTenantCompanyAsync(context, 1, 1);
        await SeedTenantCompanyAsync(context, 2, 1);
        context.FgsUsers.AddRange(
            CreateUser(1, 1, "a@test.com"),
            CreateUser(2, 1, "b@test.com"));
        await context.SaveChangesAsync();

        var users = await context.FgsUsers.ToListAsync();

        users.Should().ContainSingle(u => u.Email == "a@test.com");
    }

    [Fact]
    public async Task ListUsers_WhenTenantContextResolved_IgnoresOtherCompanyInSameTenant()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = 1,
                CompanyId = 2
            }
        };

        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        await SeedTenantCompanyAsync(context, 1, 1);
        await SeedTenantCompanyAsync(context, 1, 2);
        context.FgsUsers.AddRange(
            CreateUser(1, 1, "company1@test.com"),
            CreateUser(1, 2, "company2@test.com"));
        await context.SaveChangesAsync();

        var users = await context.FgsUsers.ToListAsync();

        users.Should().ContainSingle(u => u.Email == "company2@test.com");
    }

    private static async Task SeedTenantCompanyAsync(
        FgsUserDbContext context,
        long tenantId,
        long companyId)
    {
        if (!await context.FgsTenants.AnyAsync(t => t.Id == tenantId))
        {
            context.FgsTenants.Add(new FgsTenant
            {
                Id = tenantId,
                TenantGuid = Guid.NewGuid(),
                TenantCode = $"tenant-{tenantId}",
                Name = $"Tenant {tenantId}"
            });
        }

        if (!await context.FgsTenantCompanies.AnyAsync(c => c.TenantId == tenantId && c.CompanyNumber == companyId))
        {
            context.FgsTenantCompanies.Add(new FgsTenantCompany
            {
                TenantId = tenantId,
                CompanyNumber = companyId,
                CompanyGuid = Guid.NewGuid(),
                Code = $"company-{companyId}",
                Name = $"Company {companyId}",
                BusinessTypeId = 1
            });
        }

        await context.SaveChangesAsync();
    }

    private static FgsUser CreateUser(long tenantId, long companyId, string email) =>
        new()
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CompanyId = companyId,
            Email = email,
            DisplayName = email,
            IsActive = true
        };
}

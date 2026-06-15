using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;

namespace Fgs.User.Tests.Infrastructure;

public sealed class DbFgsTenantScopeValidatorTests
{
    [Fact]
    public async Task IsValidScopeAsync_WhenTenantAndCompanyExist_ReturnsTrue()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = "t",
            Name = "Tenant",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenant.Id,
            CompanyNumber = 1,
            CompanyGuid = Guid.NewGuid(),
            BusinessTypeId = 1,
            Code = "c1",
            Name = "Company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var validator = new DbFgsTenantScopeValidator(new EfUnitOfWork<FgsUserDbContext>(context));
        var result = await validator.IsValidScopeAsync(tenant.Id, 1, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsValidScopeAsync_WhenCompanyMissing_ReturnsFalse()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = "t",
            Name = "Tenant",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        var validator = new DbFgsTenantScopeValidator(new EfUnitOfWork<FgsUserDbContext>(context));
        var result = await validator.IsValidScopeAsync(tenant.Id, 1, CancellationToken.None);

        result.Should().BeFalse();
    }
}

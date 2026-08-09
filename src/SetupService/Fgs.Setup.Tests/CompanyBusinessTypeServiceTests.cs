using Fgs.Contracts.Clients;
using Fgs.MultiTenancy.Persistence;
using Fgs.Setup.Domain.Entities;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Setup.Tests;

public sealed class CompanyBusinessTypeServiceTests
{
    [Fact]
    public async Task AddCompanyBusinessTypesAsync_UpsertsCacheAndInsertsBusinessTypes()
    {
        await using var context = await CreateContextAsync();
        context.GloBusinessTypes.Add(new GloBusinessType
        {
            Id = 1,
            Code = "HVAC",
            Name = "HVAC",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new CompanyBusinessTypeService(context, new DateTimeProvider());
        var companyGuid = Guid.NewGuid();

        await service.AddCompanyBusinessTypesAsync(
            10,
            1,
            new AddCompanyBusinessTypesRequest([1], companyGuid, "ACME", "Acme Co"),
            CancellationToken.None);

        var cache = await context.FgsTenantCompanyCaches.SingleAsync();
        cache.CompanyGuid.Should().Be(companyGuid);
        cache.Code.Should().Be("ACME");

        var businessType = await context.FgsBusinessTypes.SingleAsync();
        businessType.Code.Should().Be("HVAC");
        businessType.TenantId.Should().Be(10);
        businessType.CompanyId.Should().Be(1);
    }

    [Fact]
    public async Task AddCompanyBusinessTypesAsync_IsIdempotentForExistingCodes()
    {
        await using var context = await CreateContextAsync();
        context.GloBusinessTypes.AddRange(
            new GloBusinessType { Id = 1, Code = "HVAC", Name = "HVAC", IsActive = true, CreatedOn = DateTimeOffset.UtcNow },
            new GloBusinessType { Id = 2, Code = "PLUMBING", Name = "Plumbing", IsActive = true, CreatedOn = DateTimeOffset.UtcNow });
        await context.SaveChangesAsync();

        var service = new CompanyBusinessTypeService(context, new DateTimeProvider());
        var request = new AddCompanyBusinessTypesRequest([1, 2], Guid.NewGuid(), "ACME", "Acme Co");

        await service.AddCompanyBusinessTypesAsync(10, 1, request, CancellationToken.None);
        await service.AddCompanyBusinessTypesAsync(10, 1, request, CancellationToken.None);

        (await context.FgsBusinessTypes.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task AddCompanyBusinessTypesAsync_InsertsAllRequestedBusinessTypes()
    {
        await using var context = await CreateContextAsync();
        context.GloBusinessTypes.AddRange(
            new GloBusinessType { Id = 1, Code = "HVAC", Name = "HVAC", IsActive = true, CreatedOn = DateTimeOffset.UtcNow },
            new GloBusinessType { Id = 2, Code = "PLUMBING", Name = "Plumbing", IsActive = true, CreatedOn = DateTimeOffset.UtcNow },
            new GloBusinessType { Id = 3, Code = "ELECTRICAL", Name = "Electrical", IsActive = true, CreatedOn = DateTimeOffset.UtcNow });
        await context.SaveChangesAsync();

        var service = new CompanyBusinessTypeService(context, new DateTimeProvider());

        await service.AddCompanyBusinessTypesAsync(
            10,
            1,
            new AddCompanyBusinessTypesRequest([1, 2, 3], Guid.NewGuid(), "ACME", "Acme Co"),
            CancellationToken.None);

        var businessTypes = await context.FgsBusinessTypes
            .OrderBy(b => b.DisplayOrder)
            .Select(b => b.Code)
            .ToListAsync();

        businessTypes.Should().Equal("HVAC", "PLUMBING", "ELECTRICAL");
    }

    [Fact]
    public async Task AddCompanyBusinessTypesAsync_WhenBusinessTypeIsUnknown_Throws()
    {
        await using var context = await CreateContextAsync();
        var service = new CompanyBusinessTypeService(context, new DateTimeProvider());

        var act = () => service.AddCompanyBusinessTypesAsync(
            10,
            1,
            new AddCompanyBusinessTypesRequest([99], Guid.NewGuid(), "ACME", "Acme Co"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*invalid or inactive*99*");
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}

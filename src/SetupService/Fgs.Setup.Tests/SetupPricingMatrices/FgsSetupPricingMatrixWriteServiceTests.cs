using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixWriteServiceTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateAsync_CreatesHeaderOnly()
    {
        await using var context = await CreateContextAsync();

        var result = await CreateWriteService(context).CreateAsync(
            new(" MATRIX1 ", " Test matrix ", false, true, false, 2,
                new DateOnly(2026, 1, 1), null, false));

        result.Code.Should().Be("MATRIX1");
        result.Name.Should().Be("Test matrix");
        result.PriceAdjustmentTypeId.Should().Be(2);
        context.FgsSetupPricingMatrixLabors.Should().BeEmpty();
        context.FgsSetupPricingMatrixLaborTiers.Should().BeEmpty();
        context.FgsSetupPricingMatrixMaterialTiers.Should().BeEmpty();
        context.FgsSetupPricingMatrixOthers.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesHeaderFields()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(BuildCreate("OLD"));

        var result = await service.UpdateAsync(created.Id,
            new("OLD", "New description", false, true, true, 3,
                new DateOnly(2026, 2, 1), new DateOnly(2026, 12, 31), false));

        result.Code.Should().Be("OLD");
        result.Name.Should().Be("New description");
        result.IsLaborTierStructure.Should().BeTrue();
        result.IsLaborRateBySkillLevel.Should().BeTrue();
        result.IsMobileVisible.Should().BeFalse();
    }

    [Fact]
    public async Task PatchAsync_UpdatesOnlyProvidedHeaderFields()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(BuildCreate("ORIGINAL"));

        var result = await service.PatchAsync(created.Id,
            new(null, "Patched description", null, null, null, null, null, null, false, null));

        result.Code.Should().Be("ORIGINAL");
        result.Name.Should().Be("Patched description");
        result.IsMobileVisible.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_WhenNewMatrixIsDefault_UnsetsPreviousDefault()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var first = await service.CreateAsync(BuildCreate("FIRST", true));

        var second = await service.CreateAsync(BuildCreate("SECOND", true));

        (await context.FgsSetupPricingMatrices.FindAsync(first.Id))!.IsDefault.Should().BeFalse();
        second.IsDefault.Should().BeTrue();
        context.FgsSetupPricingMatrices.Count(x => x.IsDefault && x.IsActive).Should().Be(1);
    }

    private static FgsSetupPricingMatrixCreateDto BuildCreate(string name, bool isDefault = false) =>
        new(name, $"{name} description", isDefault, false, false, 1,
            new DateOnly(2026, 1, 1), null, true);

    private static FgsSetupPricingMatrixWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(x => x.TenantId).Returns(TenantId);
        user.SetupGet(x => x.CompanyId).Returns(CompanyId);
        user.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var audit = new SetupEntityAuditHelper(user.Object, CreateTenantAccessor(), new DateTimeProvider());
        return new(context, new EfUnitOfWork<FgsSetupDbContext>(context), audit);
    }

    private static ITenantContextAccessor CreateTenantAccessor() => new TestTenantContextAccessor
    {
        Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
    };

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new FgsSetupDbContext(options, CreateTenantAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

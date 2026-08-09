using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesHeaderOnlyAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var cache = new Mock<ICacheService>();
        var handler = new CreateFgsSetupPricingMatrixCommandHandler(
            CreateWriteService(context), cache.Object, CreateTenantAccessor(),
            NullLogger<CreateFgsSetupPricingMatrixCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupPricingMatrixCommand(new(
                "STANDARD", "Standard pricing", false, false, false, 1, null, null, true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Code.Should().Be("STANDARD");
        context.FgsSetupPricingMatrices.Should().ContainSingle();
        context.FgsSetupPricingMatrixLabors.Should().BeEmpty();
        context.FgsSetupPricingMatrixLaborTiers.Should().BeEmpty();
        context.FgsSetupPricingMatrixMaterialTiers.Should().BeEmpty();
        context.FgsSetupPricingMatrixOthers.Should().BeEmpty();
        cache.Verify(c => c.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(TenantId, CompanyId, "pricingmatrix"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_UpdatesHeaderOnlyAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(
            new("OLD", "Old name", false, false, false, 1, null, null, true));
        var cache = new Mock<ICacheService>();
        var handler = new UpdateFgsSetupPricingMatrixCommandHandler(
            service, cache.Object, CreateTenantAccessor(),
            NullLogger<UpdateFgsSetupPricingMatrixCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsSetupPricingMatrixCommand(created.Id,
                new("OLD", "Updated name", false, true, true, 2,
                    new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31), false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Code.Should().Be("OLD");
        response.Data.Name.Should().Be("Updated name");
        response.Data.IsLaborTierStructure.Should().BeTrue();
        context.FgsSetupPricingMatrixLabors.Should().BeEmpty();
        context.FgsSetupPricingMatrixMaterialTiers.Should().BeEmpty();
        context.FgsSetupPricingMatrixOthers.Should().BeEmpty();
        cache.Verify(c => c.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(TenantId, CompanyId, "pricingmatrix"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static ITenantContextAccessor CreateTenantAccessor() => new TestTenantContextAccessor
    {
        Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
    };

    private static FgsSetupPricingMatrixWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(x => x.TenantId).Returns(TenantId);
        user.SetupGet(x => x.CompanyId).Returns(CompanyId);
        user.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var audit = new SetupEntityAuditHelper(user.Object, CreateTenantAccessor(), new DateTimeProvider());
        return new(context, new EfUnitOfWork<FgsSetupDbContext>(context), audit);
    }

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

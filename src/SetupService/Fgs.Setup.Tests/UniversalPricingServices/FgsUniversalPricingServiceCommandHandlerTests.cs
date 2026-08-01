using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.DeleteFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecordWithNestedChildren()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsUniversalPricingServiceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsUniversalPricingServiceCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsUniversalPricingServiceCommand(
                new FgsUniversalPricingServiceCreateDto(
                    "TEST",
                    5,
                    Tiers: [new FgsUniversalMatrixTierItemDto(null, "Standard", 1.0m, 1)],
                    Items: [new FgsUniversalMatrixItemItemDto(null, "Filter", "EA", 10m, 1)],
                    AddOns: [new FgsUniversalMatrixAddOnItemDto(null, "Warranty", "EA", 25m, 1)])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.Tiers.Should().HaveCount(1);
        response.Data.Tiers[0].Name.Should().Be("Standard");
        response.Data.Items.Should().HaveCount(1);
        response.Data.AddOns.Should().HaveCount(1);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "universalpricingservice"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_UpsertsById_AndSoftDeactivatesOmittedChildren()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalPricingServiceCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<CreateFgsUniversalPricingServiceCommandHandler>.Instance);
        var updateHandler = new UpdateFgsUniversalPricingServiceCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<UpdateFgsUniversalPricingServiceCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalPricingServiceCommand(
                new FgsUniversalPricingServiceCreateDto(
                    "TEST",
                    5,
                    Tiers:
                    [
                        new FgsUniversalMatrixTierItemDto(null, "Keep", 1.0m, 1),
                        new FgsUniversalMatrixTierItemDto(null, "Drop", 1.5m, 2)
                    ])),
            CancellationToken.None);

        var keepId = created.Data!.Tiers.Single(t => t.Name == "Keep").Id;

        var updated = await updateHandler.Handle(
            new UpdateFgsUniversalPricingServiceCommand(
                created.Data.Id,
                new FgsUniversalPricingServiceUpdateDto(
                    "TEST",
                    5,
                    Tiers:
                    [
                        new FgsUniversalMatrixTierItemDto(keepId, "Keep Updated", 1.25m, 1),
                        new FgsUniversalMatrixTierItemDto(null, "New Tier", 2.0m, 2)
                    ])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Tiers.Should().HaveCount(2);
        updated.Data.Tiers.Should().Contain(t => t.Id == keepId && t.Name == "Keep Updated" && t.Multiplier == 1.25m);
        updated.Data.Tiers.Should().Contain(t => t.Name == "New Tier");
        updated.Data.Tiers.Should().NotContain(t => t.Name == "Drop");

        var dropped = await context.FgsUniversalMatrixTiers
            .AsNoTracking()
            .SingleAsync(t => t.Name == "Drop");
        dropped.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task PatchHandler_DoesNotAlterChildren()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalPricingServiceCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<CreateFgsUniversalPricingServiceCommandHandler>.Instance);
        var patchHandler = new PatchFgsUniversalPricingServiceCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<PatchFgsUniversalPricingServiceCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalPricingServiceCommand(
                new FgsUniversalPricingServiceCreateDto(
                    "TEST",
                    5,
                    Tiers: [new FgsUniversalMatrixTierItemDto(null, "Standard", 1.0m, 1)])),
            CancellationToken.None);

        var patched = await patchHandler.Handle(
            new PatchFgsUniversalPricingServiceCommand(
                created.Data!.Id,
                new FgsUniversalPricingServicePatchDto(null, 9, null)),
            CancellationToken.None);

        patched.Success.Should().BeTrue();
        patched.Data!.DisplayOrder.Should().Be(9);
        patched.Data.Tiers.Should().HaveCount(1);
        patched.Data.Tiers[0].Name.Should().Be("Standard");
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalPricingServiceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsUniversalPricingServiceCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsUniversalPricingServiceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsUniversalPricingServiceCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalPricingServiceCommand(new FgsUniversalPricingServiceCreateDto("TEST", 5)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsUniversalPricingServiceCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsUniversalPricingServiceWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new FgsUniversalPricingServiceWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

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

using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.CreateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.DeleteFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.UpdateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixTiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixTiers;

public sealed class FgsUniversalMatrixTierCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var parentId = await SeedParentAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsUniversalMatrixTierCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsUniversalMatrixTierCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsUniversalMatrixTierCommand(
                new FgsUniversalMatrixTierCreateDto(parentId, "Standard", 1.0m, 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.Name.Should().Be("Standard");
        response.Data.UniversalPricingServiceId.Should().Be(parentId);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "universalmatrixtier"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_UpdatesFields()
    {
        await using var context = await CreateContextAsync();
        var parentId = await SeedParentAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalMatrixTierCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<CreateFgsUniversalMatrixTierCommandHandler>.Instance);
        var updateHandler = new UpdateFgsUniversalMatrixTierCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<UpdateFgsUniversalMatrixTierCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalMatrixTierCommand(
                new FgsUniversalMatrixTierCreateDto(parentId, "Keep", 1.0m, 1)),
            CancellationToken.None);

        var updated = await updateHandler.Handle(
            new UpdateFgsUniversalMatrixTierCommand(
                created.Data!.Id,
                new FgsUniversalMatrixTierUpdateDto(parentId, "Keep Updated", 1.25m, 2)),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Name.Should().Be("Keep Updated");
        updated.Data.Multiplier.Should().Be(1.25m);
        updated.Data.DisplayOrder.Should().Be(2);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var parentId = await SeedParentAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalMatrixTierCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<CreateFgsUniversalMatrixTierCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsUniversalMatrixTierCommandHandler(
            writeService, cache.Object, tenantAccessor,
            NullLogger<DeleteFgsUniversalMatrixTierCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalMatrixTierCommand(
                new FgsUniversalMatrixTierCreateDto(parentId, "Standard", 1.0m, 1)),
            CancellationToken.None);

        var response = await deleteHandler.Handle(
            new DeleteFgsUniversalMatrixTierCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static async Task<long> SeedParentAsync(FgsSetupDbContext context)
    {
        var parent = new FgsUniversalPricingService
        {
            UniversalPricingServiceCode = "TEST",
            DisplayOrder = 1,
            TenantId = TenantId,
            CompanyId = CompanyId,
            IsActive = true
        };
        context.FgsUniversalPricingServices.Add(parent);
        await context.SaveChangesAsync();
        return parent.Id;
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsUniversalMatrixTierWriteService CreateWriteService(FgsSetupDbContext context)
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
        return new FgsUniversalMatrixTierWriteService(context, unitOfWork, auditHelper);
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

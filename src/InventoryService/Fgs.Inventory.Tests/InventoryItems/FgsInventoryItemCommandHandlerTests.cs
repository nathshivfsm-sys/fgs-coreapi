using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.CreateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.UpdateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.CreateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.UpdateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.InventoryItemAlternates;
using Fgs.Inventory.Infrastructure.InventoryItemDependencies;
using Fgs.Inventory.Infrastructure.InventoryItems;
using Microsoft.EntityFrameworkCore;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.InventoryItems;

public sealed class FgsInventoryItemCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsInventoryItemCreateDto SampleCreateDto(long inventoryItemTypeId) =>
        new(inventoryItemTypeId, "ITEM-MAIN", "Main Item");

    [Fact]
    public async Task CreateItem_Then_ReplaceAlternates_Then_Update_RemovesOmitted()
    {
        await using var context = await CreateContextAsync();
        var itemType = new FgsInventoryItemType
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ItemTypeCode = "PART",
            Name = "Part",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        var alternateItem = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            InventoryItemTypeId = 0,
            ItemCode = "ALT01",
            Name = "Alternate",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        var alternateItem2 = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            InventoryItemTypeId = 0,
            ItemCode = "ALT02",
            Name = "Alternate 2",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsInventoryItemTypes.Add(itemType);
        await context.SaveChangesAsync();

        alternateItem.InventoryItemTypeId = itemType.Id;
        alternateItem2.InventoryItemTypeId = itemType.Id;
        context.FgsInventoryItems.AddRange(alternateItem, alternateItem2);
        await context.SaveChangesAsync();

        var itemWriteService = CreateItemWriteService(context);
        var alternateWriteService = CreateAlternateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var created = await new CreateFgsInventoryItemCommandHandler(
            itemWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInventoryItemCommandHandler>.Instance).Handle(
            new CreateFgsInventoryItemCommand(SampleCreateDto(itemType.Id)),
            CancellationToken.None);

        created.Success.Should().BeTrue();
        created.Data!.Alternates.Should().BeEmpty();

        var assigned = await new CreateFgsInventoryItemAlternatesCommandHandler(
            alternateWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInventoryItemAlternatesCommandHandler>.Instance).Handle(
            new CreateFgsInventoryItemAlternatesCommand(
                new FgsInventoryItemAlternateReplaceDto(
                    created.Data.Id,
                    [
                        new FgsInventoryItemAlternateDto(null, alternateItem.Id, 1),
                        new FgsInventoryItemAlternateDto(null, alternateItem2.Id, 2)
                    ])),
            CancellationToken.None);

        assigned.Success.Should().BeTrue();
        assigned.Data!.Should().HaveCount(2);
        (await context.FgsInventoryItemAlternates.CountAsync()).Should().Be(2);

        var keepId = assigned.Data[0].Id;
        var updated = await new UpdateFgsInventoryItemAlternatesCommandHandler(
            alternateWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsInventoryItemAlternatesCommandHandler>.Instance).Handle(
            new UpdateFgsInventoryItemAlternatesCommand(
                new FgsInventoryItemAlternateReplaceDto(
                    created.Data.Id,
                    [new FgsInventoryItemAlternateDto(keepId, alternateItem.Id, 1)])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Should().HaveCount(1);
        updated.Data[0].AlternateInventoryItemId.Should().Be(alternateItem.Id);
        (await context.FgsInventoryItemAlternates.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task CreateItem_Then_ReplaceDependencies_Then_Update_RemovesOmitted()
    {
        await using var context = await CreateContextAsync();
        var itemType = new FgsInventoryItemType
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ItemTypeCode = "PART",
            Name = "Part",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        var dependent1 = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            InventoryItemTypeId = 0,
            ItemCode = "DEP01",
            Name = "Dependent 1",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        var dependent2 = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            InventoryItemTypeId = 0,
            ItemCode = "DEP02",
            Name = "Dependent 2",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsInventoryItemTypes.Add(itemType);
        await context.SaveChangesAsync();

        dependent1.InventoryItemTypeId = itemType.Id;
        dependent2.InventoryItemTypeId = itemType.Id;
        context.FgsInventoryItems.AddRange(dependent1, dependent2);
        await context.SaveChangesAsync();

        var itemWriteService = CreateItemWriteService(context);
        var dependencyWriteService = CreateDependencyWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var created = await new CreateFgsInventoryItemCommandHandler(
            itemWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInventoryItemCommandHandler>.Instance).Handle(
            new CreateFgsInventoryItemCommand(SampleCreateDto(itemType.Id)),
            CancellationToken.None);

        created.Success.Should().BeTrue();
        created.Data!.Dependencies.Should().BeEmpty();

        var assigned = await new CreateFgsInventoryItemDependenciesCommandHandler(
            dependencyWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInventoryItemDependenciesCommandHandler>.Instance).Handle(
            new CreateFgsInventoryItemDependenciesCommand(
                new FgsInventoryItemDependencyReplaceDto(
                    created.Data.Id,
                    [
                        new FgsInventoryItemDependencyDto(null, dependent1.Id, 1m, true, null, 1),
                        new FgsInventoryItemDependencyDto(null, dependent2.Id, 2m, false, null, 2)
                    ])),
            CancellationToken.None);

        assigned.Success.Should().BeTrue();
        assigned.Data!.Should().HaveCount(2);
        (await context.FgsInventoryItemDependencies.CountAsync()).Should().Be(2);

        var keepId = assigned.Data[0].Id;
        var updated = await new UpdateFgsInventoryItemDependenciesCommandHandler(
            dependencyWriteService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsInventoryItemDependenciesCommandHandler>.Instance).Handle(
            new UpdateFgsInventoryItemDependenciesCommand(
                new FgsInventoryItemDependencyReplaceDto(
                    created.Data.Id,
                    [new FgsInventoryItemDependencyDto(keepId, dependent1.Id, 1.5m, true, "kept", 1)])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Should().HaveCount(1);
        updated.Data[0].DependentInventoryItemId.Should().Be(dependent1.Id);
        updated.Data[0].Quantity.Should().Be(1.5m);
        (await context.FgsInventoryItemDependencies.CountAsync()).Should().Be(1);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsInventoryItemWriteService CreateItemWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        return new FgsInventoryItemWriteService(context, unitOfWork, auditHelper);
    }

    private static FgsInventoryItemAlternateWriteService CreateAlternateWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        return new FgsInventoryItemAlternateWriteService(context, unitOfWork, auditHelper);
    }

    private static FgsInventoryItemDependencyWriteService CreateDependencyWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        return new FgsInventoryItemDependencyWriteService(context, unitOfWork, auditHelper);
    }

    private static (InventoryEntityAuditHelper AuditHelper, EfUnitOfWork<FgsInventoryDbContext> UnitOfWork) CreateAuditAndUow(
        FgsInventoryDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var dateTime = new Mock<IDateTimeProvider>();
        dateTime.SetupGet(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        var auditHelper = new InventoryEntityAuditHelper(userContext.Object, tenantAccessor, dateTime.Object);
        var unitOfWork = new EfUnitOfWork<FgsInventoryDbContext>(context);
        return (auditHelper, unitOfWork);
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(options, new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        });
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

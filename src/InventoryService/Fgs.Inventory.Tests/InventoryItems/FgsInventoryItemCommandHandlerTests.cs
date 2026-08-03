using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Common.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.InventoryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.InventoryItems;

public sealed class FgsInventoryItemCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsInventoryItemCreateDto SampleCreateDto(
        long inventoryItemTypeId,
        IReadOnlyList<FgsInventoryItemAlternateDto>? alternates = null,
        IReadOnlyList<FgsInventoryItemDependencyDto>? dependencies = null) =>
        new(
            inventoryItemTypeId,
            "ITEM-MAIN",
            "Main Item",
            Alternates: alternates,
            Dependencies: dependencies);

    [Fact]
    public async Task CreateHandler_WithAlternates_Then_Update_RemovesOmitted()
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

        var writeService = CreateItemWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var created = await new CreateFgsInventoryItemCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInventoryItemCommandHandler>.Instance).Handle(
            new CreateFgsInventoryItemCommand(
                SampleCreateDto(
                    itemType.Id,
                    alternates:
                    [
                        new FgsInventoryItemAlternateDto(null, alternateItem.Id, 1),
                        new FgsInventoryItemAlternateDto(null, alternateItem2.Id, 2)
                    ])),
            CancellationToken.None);

        created.Success.Should().BeTrue();
        created.Data!.Alternates.Should().HaveCount(2);
        (await context.FgsInventoryItemAlternates.CountAsync()).Should().Be(2);

        var keepId = created.Data.Alternates[0].Id;
        var updated = await new UpdateFgsInventoryItemCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsInventoryItemCommandHandler>.Instance).Handle(
            new UpdateFgsInventoryItemCommand(
                created.Data.Id,
                new FgsInventoryItemUpdateDto(
                    itemType.Id,
                    "ITEM-MAIN",
                    "Main Item",
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    false,
                    0m,
                    0m,
                    0m,
                    true,
                    null,
                    null,
                    [new FgsInventoryItemAlternateDto(keepId, alternateItem.Id, 1)])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Alternates.Should().HaveCount(1);
        updated.Data.Alternates[0].AlternateInventoryItemId.Should().Be(alternateItem.Id);
        (await context.FgsInventoryItemAlternates.CountAsync()).Should().Be(1);
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

        var auditHelper = new InventoryEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsInventoryDbContext>(context);
        return (auditHelper, unitOfWork);
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

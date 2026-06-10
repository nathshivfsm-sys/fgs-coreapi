using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Persistence.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Setup;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class DeleteSetupEntityCommandHandlerTests
{
    [Fact]
    public async Task Handle_SoftDeletesBillingCategory()
    {
        await using var context = await CreateContextAsync();
        context.FgsBillingCategories.Add(new FgsBillingCategory
        {
            Id = 5,
            TenantId = 1,
            CompanyId = 2,
            BillingCategoryType = "LB",
            BillingCategoryName = "Labor",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entityRegistry = CreateEntityRegistry(FgsBillingCategoryDescriptor.Create());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        var writeService = new CatalogEntityWriteService<FgsSetupDbContext>(context, unitOfWork, CreateAuditStamper());
        var handler = new DeleteCatalogEntityCommandHandler(entityRegistry, writeService);

        var response = await handler.Handle(
            new DeleteCatalogEntityCommand(EntityKeys.BillingCategory, "5"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        var saved = await context.FgsBillingCategories.SingleAsync();
        saved.IsActive.Should().BeFalse();
        saved.UpdatedBy.Should().Be("user@test.com");
    }

    [Fact]
    public async Task Handle_HardDeletesVehicleMaintenance()
    {
        await using var context = await CreateContextAsync();
        context.FgsVehicleMaintenances.Add(new FgsVehicleMaintenance
        {
            Id = 7,
            TenantId = 1,
            CompanyId = 2,
            VehicleId = 1,
            VehicleMaintenanceTypeId = 1,
            ServiceDate = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entityRegistry = CreateEntityRegistry(FgsVehicleMaintenanceDescriptor.Create());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        var writeService = new CatalogEntityWriteService<FgsSetupDbContext>(context, unitOfWork, CreateAuditStamper());
        var handler = new DeleteCatalogEntityCommandHandler(entityRegistry, writeService);

        var response = await handler.Handle(
            new DeleteCatalogEntityCommand(EntityKeys.VehicleMaintenance, "7"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        context.FgsVehicleMaintenances.Count().Should().Be(0);
    }

    private static IEntityRegistry CreateEntityRegistry(CatalogEntityDescriptor descriptor)
    {
        var registry = new EntityRegistry();
        registry.Register(descriptor);
        return registry;
    }

    private static CatalogEntityAuditStamper CreateAuditStamper()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(context => context.Email).Returns("user@test.com");
        return new CatalogEntityAuditStamper(userContext.Object, new SetupCatalogDateTimeProvider(new DateTimeProvider()));
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, new TestTenantContextAccessor(1, 2));
        await context.Database.EnsureCreatedAsync();
        context.FgsTenantCompanyCaches.Add(new FgsTenantCompanyCache
        {
            TenantId = 1,
            CompanyId = 2,
            CompanyGuid = Guid.NewGuid(),
            Code = "ACME",
            Name = "Acme",
            IsActive = true
        });
        await context.SaveChangesAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor(long tenantId, long companyId) : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; } = new TenantContext
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsResolved = true
        };
    }
}

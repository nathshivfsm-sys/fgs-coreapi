using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Persistence.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Setup;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class CreateSetupEntityCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesBillingCategoryWithAuditFields()
    {
        var entityRegistry = CreateEntityRegistry(FgsBillingCategoryDescriptor.Create());

        await using var context = await CreateContextAsync();
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        var writeService = new CatalogEntityWriteService<FgsSetupDbContext>(
            context,
            unitOfWork,
            CreateAuditStamper());

        var handler = new CreateCatalogEntityCommandHandler<FgsBillingCategoryCreateDto, FgsBillingCategoryDetailDto>(
            entityRegistry,
            writeService);

        var response = await handler.Handle(
            new CreateCatalogEntityCommand<FgsBillingCategoryCreateDto, FgsBillingCategoryDetailDto>(
                EntityKeys.BillingCategory,
                new FgsBillingCategoryCreateDto("LB", "Labor", null, 1, false, false, true)),
            CancellationToken.None);

        response.Success.Should().BeTrue($"Expected success but got: {string.Join(", ", response.Errors)}");
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data!.BillingCategoryName.Should().Be("Labor");

        var saved = await context.FgsBillingCategories.SingleAsync();
        saved.TenantId.Should().Be(1);
        saved.CompanyId.Should().Be(2);
        saved.CreatedBy.Should().Be("user@test.com");
        saved.IsActive.Should().BeTrue();
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
        userContext.SetupGet(context => context.TenantId).Returns(1);
        userContext.SetupGet(context => context.CompanyId).Returns(2);
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

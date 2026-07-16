using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.DeleteFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixAddOns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixAddOns;

public sealed class FgsUniversalMatrixAddOnCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsUniversalMatrixAddOnCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsUniversalMatrixAddOnCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsUniversalMatrixAddOnCommand(new FgsUniversalMatrixAddOnCreateDto(1, "Name", "UnitType", 10.5m, 5)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "universalmatrixaddon"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsUniversalMatrixAddOnCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsUniversalMatrixAddOnCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsUniversalMatrixAddOnCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsUniversalMatrixAddOnCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsUniversalMatrixAddOnCommand(new FgsUniversalMatrixAddOnCreateDto(1, "Name", "UnitType", 10.5m, 5)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsUniversalMatrixAddOnCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsUniversalMatrixAddOnWriteRepository CreateWriteService(FgsSetupDbContext context)
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
        return new FgsUniversalMatrixAddOnWriteRepository(context, unitOfWork, auditHelper);
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

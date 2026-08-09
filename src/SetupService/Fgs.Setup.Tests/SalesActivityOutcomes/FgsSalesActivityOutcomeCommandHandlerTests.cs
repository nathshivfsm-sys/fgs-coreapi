using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.CreateFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.DeleteFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.UpdateFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.SalesActivityOutcomes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SalesActivityOutcomes;

public sealed class FgsSalesActivityOutcomeCommandHandlerTests
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
        var handler = new CreateFgsSalesActivityOutcomeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSalesActivityOutcomeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSalesActivityOutcomeCommand(new FgsSalesActivityOutcomeCreateDto("TEST", "OutcomeName", "Description", 5, false, true, true, null, false, false, true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "salesactivityoutcome"),
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
        var createHandler = new CreateFgsSalesActivityOutcomeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSalesActivityOutcomeCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsSalesActivityOutcomeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsSalesActivityOutcomeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSalesActivityOutcomeCommand(new FgsSalesActivityOutcomeCreateDto("TEST", "OutcomeName", "Description", 5, false, true, true, null, false, false, true)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsSalesActivityOutcomeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsSalesActivityOutcomeWriteService CreateWriteService(FgsSetupDbContext context)
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
        return new FgsSalesActivityOutcomeWriteService(context, unitOfWork, auditHelper);
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

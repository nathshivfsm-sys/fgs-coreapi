using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.DeleteTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.TechTrades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.TechTrades;

public sealed class TechTradeCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesTechTradeWithAuditFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateTechTradeCommandHandler(
            writeService,
            NullLogger<CreateTechTradeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateTechTradeCommand(new TechTradeCreateDto("hvac", "HVAC Services", "Primary trade", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.TradeCode.Should().Be("HVAC");
        response.Data.Name.Should().Be("HVAC Services");
        response.Data.IsActive.Should().BeTrue();
        response.Data.CreatedBy.Should().Be("11111111-1111-1111-1111-111111111111");
        response.Data.TenantId.Should().Be(TenantId);
        response.Data.CompanyId.Should().Be(CompanyId);
    }

    [Fact]
    public async Task UpdateHandler_UpdatesMutableFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateTechTradeCommandHandler(
            writeService,
            NullLogger<CreateTechTradeCommandHandler>.Instance);
        var updateHandler = new UpdateTechTradeCommandHandler(
            writeService,
            NullLogger<UpdateTechTradeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateTechTradeCommand(new TechTradeCreateDto("HVAC", "Old Name", null, 0)),
            CancellationToken.None);
        created.Success.Should().BeTrue();
        created.Data!.Id.Should().BeGreaterThan(0);
        (await context.FgsSetupTechTrades.CountAsync()).Should().Be(1);

        var response = await updateHandler.Handle(
            new UpdateTechTradeCommand(created.Data.Id, new TechTradeUpdateDto("PLUMB", "Plumbing", "Updated", 2)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TradeCode.Should().Be("PLUMB");
        response.Data.Name.Should().Be("Plumbing");
        response.Data.UpdatedBy.Should().Be("11111111-1111-1111-1111-111111111111");
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletesTechTrade()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateTechTradeCommandHandler(
            writeService,
            NullLogger<CreateTechTradeCommandHandler>.Instance);
        var deleteHandler = new DeleteTechTradeCommandHandler(
            writeService,
            NullLogger<DeleteTechTradeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateTechTradeCommand(new TechTradeCreateDto("ELEC", "Electrical", null, 0)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteTechTradeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();

        var entity = await context.FgsSetupTechTrades.SingleAsync();
        entity.IsActive.Should().BeFalse();
    }

    private static TechTradeWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = TenantId,
                CompanyId = CompanyId,
                IsResolved = true
            }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new TechTradeWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = TenantId,
                CompanyId = CompanyId,
                IsResolved = true
            }
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

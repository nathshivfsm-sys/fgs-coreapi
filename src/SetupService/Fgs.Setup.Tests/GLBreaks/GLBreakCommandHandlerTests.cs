using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.DeleteGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.GLBreaks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.GLBreaks;

public sealed class GLBreakCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesGLBreakWithAddressAndTrades()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        await SeedTechTradeAsync(context, "HVAC");
        var writeService = CreateWriteService(context);
        var handler = new CreateGLBreakCommandHandler(
            writeService,
            NullLogger<CreateGLBreakCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateGLBreakCommand(new GLBreakCreateDto(
                "HVAC",
                "HVAC Division",
                "Heating & Cooling",
                1,
                null,
                new LocationWriteDto("123 Main St", null, null, null, "Dallas", "TX", null, "US", "75201", null, null, null, null),
                ["HVAC"])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Code.Should().Be("HVAC");
        response.Data.Address.Should().NotBeNull();
        response.Data.Address!.AddressLine1.Should().Be("123 Main St");
        response.Data.Trades.Should().ContainSingle();
        response.Data.Trades[0].TradeCode.Should().Be("HVAC");
    }

    [Fact]
    public async Task UpdateHandler_ReplacesTradeMappings()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        await SeedTechTradeAsync(context, "HVAC");
        await SeedTechTradeAsync(context, "PLUMB");
        var writeService = CreateWriteService(context);
        var createHandler = new CreateGLBreakCommandHandler(
            writeService,
            NullLogger<CreateGLBreakCommandHandler>.Instance);
        var updateHandler = new UpdateGLBreakCommandHandler(
            writeService,
            NullLogger<UpdateGLBreakCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateGLBreakCommand(new GLBreakCreateDto(
                "HVAC", "HVAC Division", null, 1, null, null, ["HVAC"])),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await updateHandler.Handle(
            new UpdateGLBreakCommand(
                created.Data!.Id,
                new GLBreakUpdateDto(
                    "HVAC",
                    "HVAC Division Updated",
                    null,
                    1,
                    null,
                    null,
                    ["PLUMB"])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("HVAC Division Updated");
        response.Data.Trades.Should().ContainSingle();
        response.Data.Trades[0].TradeCode.Should().Be("PLUMB");
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletesGLBreak()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var createHandler = new CreateGLBreakCommandHandler(
            writeService,
            NullLogger<CreateGLBreakCommandHandler>.Instance);
        var deleteHandler = new DeleteGLBreakCommandHandler(
            writeService,
            NullLogger<DeleteGLBreakCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateGLBreakCommand(new GLBreakCreateDto(
                "ELEC", "Electrical", null, 2, null, null, [])),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteGLBreakCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();

        var entity = await context.FgsSetupGLBreaks.SingleAsync();
        entity.IsActive.Should().BeFalse();
    }

    private static GLBreakWriteService CreateWriteService(FgsSetupDbContext context)
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
                CompanyId = CompanyId
            }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        ISetupLocationWriteService locationWriteService = new SetupLocationWriteService(
            context,
            unitOfWork,
            auditHelper);

        return new GLBreakWriteService(context, unitOfWork, auditHelper, locationWriteService);
    }

    private static async Task SeedMasterEntityTypeAsync(FgsSetupDbContext context)
    {
        await context.GloMasterEntityTypes.AddAsync(new GloMasterEntityType
        {
            Id = 2,
            Code = "COMPANY",
            IsDocumentAllowed = true,
            IsActive = true,
            SortOrder = 2,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();
    }

    private static async Task SeedTechTradeAsync(FgsSetupDbContext context, string tradeCode)
    {
        await context.FgsSetupTechTrades.AddAsync(new FgsSetupTechTrade
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            TradeCode = tradeCode,
            Name = tradeCode,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = TenantId,
                CompanyId = CompanyId
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

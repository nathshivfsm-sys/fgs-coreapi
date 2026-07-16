using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.DeleteTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.TitlesOfCourtesy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.TitlesOfCourtesy;

public sealed class TitleOfCourtesyCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesTitleOfCourtesy()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateTitleOfCourtesyCommandHandler(
            writeService,
            NullLogger<CreateTitleOfCourtesyCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("mr", "Mr.", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Code.Should().Be("MR");
        response.Data.DisplayName.Should().Be("Mr.");
        response.Data.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateHandler_UpdatesMutableFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateTitleOfCourtesyCommandHandler(
            writeService,
            NullLogger<CreateTitleOfCourtesyCommandHandler>.Instance);
        var updateHandler = new UpdateTitleOfCourtesyCommandHandler(
            writeService,
            NullLogger<UpdateTitleOfCourtesyCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("MR", "Mr.", 1)),
            CancellationToken.None);
        created.Success.Should().BeTrue();
        created.Data!.Id.Should().BeGreaterThan(0);
        (await context.FgsSetupTitlesOfCourtesy.CountAsync()).Should().Be(1);

        var response = await updateHandler.Handle(
            new UpdateTitleOfCourtesyCommand(created.Data.Id, new TitleOfCourtesyUpdateDto("MR", "Mister", 2)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Code.Should().Be("MR");
        response.Data.DisplayName.Should().Be("Mister");
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletesTitleOfCourtesy()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateTitleOfCourtesyCommandHandler(
            writeService,
            NullLogger<CreateTitleOfCourtesyCommandHandler>.Instance);
        var deleteHandler = new DeleteTitleOfCourtesyCommandHandler(
            writeService,
            NullLogger<DeleteTitleOfCourtesyCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("DR", "Dr.", 5)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteTitleOfCourtesyCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();

        var entity = await context.FgsSetupTitlesOfCourtesy.SingleAsync();
        entity.IsActive.Should().BeFalse();
    }

    private static TitleOfCourtesyWriteService CreateWriteService(FgsSetupDbContext context)
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
        return new TitleOfCourtesyWriteService(context, unitOfWork, auditHelper);
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

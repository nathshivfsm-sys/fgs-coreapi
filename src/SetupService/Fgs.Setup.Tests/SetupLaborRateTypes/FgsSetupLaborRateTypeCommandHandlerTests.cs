using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.CreateFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.DeleteFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.UpdateFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.SetupLaborRateTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupLaborRateTypes;

public sealed class FgsSetupLaborRateTypeCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesWithAuditFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateFgsSetupLaborRateTypeCommandHandler(
            writeService,
            NullLogger<CreateFgsSetupLaborRateTypeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupLaborRateTypeCommand(new FgsSetupLaborRateTypeCreateDto("Name value", "Description value", 1, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.TenantId.Should().Be(TenantId);
        response.Data.CompanyId.Should().Be(CompanyId);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateFgsSetupLaborRateTypeCommandHandler(
            writeService,
            NullLogger<CreateFgsSetupLaborRateTypeCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsSetupLaborRateTypeCommandHandler(
            writeService,
            NullLogger<DeleteFgsSetupLaborRateTypeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSetupLaborRateTypeCommand(new FgsSetupLaborRateTypeCreateDto("Name value", "Description value", 1, false)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsSetupLaborRateTypeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static FgsSetupLaborRateTypeWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new FgsSetupLaborRateTypeWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }
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

using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.CreateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.DeleteFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.SetupTaxAuthorities;
using Fgs.Setup.Infrastructure.SetupTaxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxes;

public sealed class FgsSetupTaxCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesWithAuditFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.TenantId.Should().Be(TenantId);
        response.Data.CompanyId.Should().Be(CompanyId);
        response.Data.TaxDetails.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateHandler_CreatesWithNestedTaxDetails()
    {
        await using var context = await CreateContextAsync();
        var authorityWriteService = CreateTaxAuthorityWriteService(context);
        var authority = await authorityWriteService.CreateAsync(
            new FgsSetupTaxAuthorityCreateDto("STATE", "State Tax", "TX", false, 6.25m, null),
            CancellationToken.None);

        var writeService = CreateWriteService(context);
        var handler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto(
                "COMBINED",
                "Combined Tax",
                false,
                null,
                null,
                true,
                "Combined rate",
                [
                    new FgsSetupTaxAuthorityAssignmentWriteDto(
                        authority.Id,
                        new DateOnly(2026, 1, 1),
                        null,
                        false)
                ])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TaxDetails.Should().HaveCount(1);
        response.Data.TaxDetails[0].FgsSetupTaxAuthorityId.Should().Be(authority.Id);
        response.Data.TaxDetails[0].TaxAuthorityCode.Should().Be("STATE");
        response.Data.TaxDetails[0].TaxPercent.Should().Be(6.25m);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsSetupTaxCommandHandler(
            writeService,
            NullLogger<DeleteFgsSetupTaxCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value")),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsSetupTaxCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static FgsSetupTaxWriteService CreateWriteService(FgsSetupDbContext context) =>
        new(context, new EfUnitOfWork<FgsSetupDbContext>(context), CreateAuditHelper());

    private static FgsSetupTaxAuthorityWriteService CreateTaxAuthorityWriteService(FgsSetupDbContext context) =>
        new(context, new EfUnitOfWork<FgsSetupDbContext>(context), CreateAuditHelper());

    private static SetupEntityAuditHelper CreateAuditHelper()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }
        };

        return new SetupEntityAuditHelper(userContext.Object, tenantAccessor, new DateTimeProvider());
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

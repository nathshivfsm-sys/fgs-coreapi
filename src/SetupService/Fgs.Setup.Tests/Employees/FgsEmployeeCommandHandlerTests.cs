using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.DeleteFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.Employees;

public sealed class FgsEmployeeCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesEmployeeWithAddressAndCalculatedRates()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsEmployeeCommand(CreateDto(regularRate: 40m)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Active);
        response.Data.OvertimeRate.Should().Be(60m);
        response.Data.DoubleTimeRate.Should().Be(80m);
        response.Data.Address.Should().NotBeNull();
        response.Data.Address!.AddressLine1.Should().Be("100 Main St");
        response.Data.Address.PostalCode.Should().Be("78701");
        context.FgsLocations.Should().ContainSingle(l => l.IsActive && l.AddressLine1 == "100 Main St");
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "employees"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SetsStatusInactiveAndSoftDeletesAddress()
    {
        await using var context = await CreateContextAsync();
        await SeedMasterEntityTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsEmployeeCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsEmployeeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsEmployeeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsEmployeeCommand(CreateDto()),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsEmployeeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.StatusId.Should().Be(EmployeeStatusIds.Inactive);
        context.FgsLocations.Single().IsActive.Should().BeFalse();
    }

    private static FgsEmployeeCreateDto CreateDto(decimal? regularRate = null) =>
        new(
            UserId: null,
            EmployeeNumber: "EMP-001",
            EmployeeTypeId: EmployeeTypeIds.Technician,
            DisplayName: "Alex Tech",
            LegalFirstName: "Alex",
            LegalMiddleName: null,
            LegalLastName: "Tech",
            BirthDate: null,
            HireDate: new DateOnly(2026, 1, 15),
            TerminationDate: null,
            StatusId: EmployeeStatusIds.Active,
            PersonalEmail: "alex.personal@example.com",
            OfficeEmail: "alex@example.com",
            PersonalPhone: null,
            OfficePhone: "+15551234567",
            Address: new LocationWriteDto(
                "100 Main St",
                "Apt 2",
                null,
                null,
                "Austin",
                "TX",
                null,
                "US",
                "78701",
                null,
                null,
                null,
                null),
            ProfilePhotoFileId: null,
            RegularRate: regularRate,
            OvertimeRate: null,
            DoubleTimeRate: null,
            LaborBurdenTypeId: LaborBurdenTypeIds.Percentage,
            LaborBurdenValue: 25m,
            IsPurchaser: false,
            Notes: "Field tech");

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsEmployeeWriteService CreateWriteService(FgsSetupDbContext context)
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
        ISetupLocationWriteService locationWriteService = new SetupLocationWriteService(
            context,
            unitOfWork,
            auditHelper);
        return new FgsEmployeeWriteService(context, unitOfWork, auditHelper, locationWriteService);
    }

    private static async Task SeedMasterEntityTypeAsync(FgsSetupDbContext context)
    {
        await context.GloMasterEntityTypes.AddAsync(new GloMasterEntityType
        {
            Id = 15,
            Code = "EMPLOYEE",
            IsDocumentAllowed = true,
            IsActive = true,
            SortOrder = 15,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();
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

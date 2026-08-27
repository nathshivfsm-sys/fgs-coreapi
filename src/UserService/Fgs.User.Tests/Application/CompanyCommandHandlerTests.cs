using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Companies.Commands.CreateCompany;
using Fgs.User.Application.Features.Companies.Commands.PatchCompany;
using Fgs.User.Application.Features.Companies.Commands.UpdateCompany;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Companies.Validators;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class CompanyCommandHandlerTests
{
    [Fact]
    public async Task CreateHandler_CreatesCompanyWithLocations()
    {
        var (context, tenantId) = await CreateSeededContextAsync();
        await using var _ = context;
        var companyDetail = SampleCompanyDetail(tenantId, 1);

        var readRepo = new Mock<IUserReadRepository<FgsTenantCompany>>();
        readRepo.Setup(r => r.AnyAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        readRepo.Setup(r => r.QueryFirstAsync<long>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0L);

        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCompanyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanyDetailDto>.Ok(companyDetail, ApiStatusCodes.Created));

        var handler = new CreateCompanyCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            TestUserRepositories.Write<FgsTenantCompany>(context),
            TestUserRepositories.Write<FgsTenantCompanyCache>(context),
            TestUserRepositories.Write<FgsLocation>(context),
            TestUserRepositories.Write<FgsTenantServiceSetup>(context),
            TestUserRepositories.Write<FgsTenantServiceAccountsSetup>(context),
            readRepo.Object,
            new EfUnitOfWork<FgsUserDbContext>(context),
            mediator.Object,
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

        var response = await handler.Handle(
            new CreateCompanyCommand(tenantId, new CompanyCreateDto(
                "BRANCH2",
                "Branch Two",
                "Branch Two LLC",
                "branch@test.com",
                "+15550199",
                "https://example.com",
                null,
                null,
                "America/Chicago",
                PhysicalAddress(),
                null)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        context.FgsTenantCompanies.IgnoreQueryFilters().Should().ContainSingle(c => c.Code == "BRANCH2");
        context.FgsLocations.IgnoreQueryFilters().Should().ContainSingle();
    }

    [Fact]
    public async Task CreateHandler_WhenTenantMissing_ReturnsNotFound()
    {
        await using var context = await CreateContextAsync();
        var handler = BuildCreateHandler(context, Mock.Of<IUserReadRepository<FgsTenantCompany>>(), Mock.Of<IMediator>());
        var response = await handler.Handle(
            new CreateCompanyCommand(99, new CompanyCreateDto("X", "Name", null, null, null, null, null, null, null, null, null)),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreateHandler_WhenCodeExists_ReturnsConflict()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        await SeedCompanyAsync(context, tenant.Id, 1, "EXISTING", "Existing Co");

        var readRepo = new Mock<IUserReadRepository<FgsTenantCompany>>();
        readRepo.Setup(r => r.AnyAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = BuildCreateHandler(context, readRepo.Object, Mock.Of<IMediator>());
        var response = await handler.Handle(
            new CreateCompanyCommand(tenant.Id, new CompanyCreateDto("EXISTING", "Duplicate", null, null, null, null, null, null, null, null, null)),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task PatchHandler_UpdatesCompanyAndLocations()
    {
        var (context, tenantId) = await CreateSeededContextAsync();
        await using var _ = context;
        await SeedCompanyAsync(context, tenantId, 1, "ACME", "Acme Co");
        await SeedCompanyCacheAsync(context, tenantId, 1, "ACME", "Acme Co");

        var companyDetail = SampleCompanyDetail(tenantId, 1) with { Name = "Acme Updated" };
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCompanyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanyDetailDto>.Ok(companyDetail));

        var handler = BuildPatchHandler(context, mediator.Object);
        var response = await handler.Handle(
            new PatchCompanyCommand(tenantId, 1, new CompanyPatchDto(
                Name: "Acme Updated",
                PhysicalAddress: PhysicalAddress(),
                BillingAddress: BillingAddress())),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Acme Updated");
        context.FgsLocations.IgnoreQueryFilters().Count().Should().Be(2);
    }

    [Fact]
    public async Task PatchHandler_UpdatesExistingLocations()
    {
        var (context, tenantId) = await CreateSeededContextAsync();
        await using var _ = context;
        var physicalId = Guid.NewGuid();
        var billingId = Guid.NewGuid();
        context.FgsLocations.AddRange(
            new FgsLocation
            {
                Id = physicalId,
                TenantId = tenantId,
                CompanyId = 1,
                AddressLine1 = "Old",
                City = "Austin",
                State = "TX",
                PostalCode = "78701",
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            },
            new FgsLocation
            {
                Id = billingId,
                TenantId = tenantId,
                CompanyId = 1,
                AddressLine1 = "Old Billing",
                City = "Dallas",
                State = "TX",
                PostalCode = "75201",
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });
        await context.SaveChangesAsync();

        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenantId,
            CompanyNumber = 1,
            CompanyGuid = Guid.NewGuid(),
            Code = "ACME",
            Name = "Acme Co",
            PhysicalLocationId = physicalId,
            BillingLocationId = billingId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        await SeedCompanyCacheAsync(context, tenantId, 1, "ACME", "Acme Co");

        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCompanyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanyDetailDto>.Ok(SampleCompanyDetail(tenantId, 1)));

        var handler = BuildPatchHandler(context, mediator.Object);
        var response = await handler.Handle(
            new PatchCompanyCommand(tenantId, 1, new CompanyPatchDto(
                PhysicalAddress: PhysicalAddress(),
                BillingAddress: BillingAddress())),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        var physical = await context.FgsLocations.IgnoreQueryFilters().SingleAsync(l => l.Id == physicalId);
        physical.AddressLine1.Should().Be("123 Main St");
    }

    [Fact]
    public async Task PatchHandler_WhenCompanyMissing_ReturnsNotFound()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        var handler = BuildPatchHandler(context, Mock.Of<IMediator>());
        var response = await handler.Handle(
            new PatchCompanyCommand(tenant.Id, 99, new CompanyPatchDto(Name: "Missing")),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateHandler_UpdatesCompany()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        await SeedCompanyAsync(context, tenant.Id, 1, "ACME", "Acme Co");
        await SeedCompanyCacheAsync(context, tenant.Id, 1, "ACME", "Acme Co");

        var companyDetail = SampleCompanyDetail(tenant.Id, 1) with { Name = "Acme Renamed" };
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCompanyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanyDetailDto>.Ok(companyDetail));

        var handler = BuildUpdateHandler(context, mediator.Object);
        var response = await handler.Handle(
            new UpdateCompanyCommand(tenant.Id, 1, new CompanyUpdateDto(
                "Acme Renamed",
                "Acme Renamed LLC",
                "info@acme.com",
                "+15550199",
                "https://acme.com",
                null,
                null,
                "America/Chicago",
                true,
                PhysicalAddress(),
                BillingAddress())),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Acme Renamed");
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidCompanyId()
    {
        var validator = new UpdateCompanyCommandValidator();
        var result = await validator.ValidateAsync(
            new UpdateCompanyCommand(1, 0, new CompanyUpdateDto("Name", null, null, null, null, null, null, null, true, null, null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidEmail()
    {
        var validator = new PatchCompanyCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchCompanyCommand(1, 1, new CompanyPatchDto(Email: "not-an-email")));

        result.IsValid.Should().BeFalse();
    }

    private static CreateCompanyCommandHandler BuildCreateHandler(
        FgsUserDbContext context,
        IUserReadRepository<FgsTenantCompany> readRepo,
        IMediator mediator) =>
        new(
            TestUserRepositories.Write<FgsTenant>(context),
            TestUserRepositories.Write<FgsTenantCompany>(context),
            TestUserRepositories.Write<FgsTenantCompanyCache>(context),
            TestUserRepositories.Write<FgsLocation>(context),
            TestUserRepositories.Write<FgsTenantServiceSetup>(context),
            TestUserRepositories.Write<FgsTenantServiceAccountsSetup>(context),
            readRepo,
            new EfUnitOfWork<FgsUserDbContext>(context),
            mediator,
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

    private static PatchCompanyCommandHandler BuildPatchHandler(FgsUserDbContext context, IMediator mediator) =>
        new(
            TestUserRepositories.Write<FgsTenantCompany>(context),
            TestUserRepositories.Write<FgsTenantCompanyCache>(context),
            TestUserRepositories.Write<FgsLocation>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            mediator,
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

    private static UpdateCompanyCommandHandler BuildUpdateHandler(FgsUserDbContext context, IMediator mediator) =>
        new(
            TestUserRepositories.Write<FgsTenantCompany>(context),
            TestUserRepositories.Write<FgsTenantCompanyCache>(context),
            TestUserRepositories.Write<FgsLocation>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            mediator,
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

    private static async Task<(FgsUserDbContext Context, long TenantId)> CreateSeededContextAsync()
    {
        var accessor = new TestTenantContextAccessor();
        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        var tenant = await SeedTenantAsync(context);
        accessor.Current = new TenantContext { TenantId = tenant.Id, CompanyId = 1 };
        return (context, tenant.Id);
    }

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor();
        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        return context;
    }

    private static async Task<FgsTenant> SeedTenantAsync(FgsUserDbContext context)
    {
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = "TENANT",
            Name = "Tenant",
            FgsTenantStatusId = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();
        return tenant;
    }

    private static async Task<FgsTenantCompany> SeedCompanyAsync(
        FgsUserDbContext context,
        long tenantId,
        long companyNumber,
        string code,
        string name)
    {
        var company = new FgsTenantCompany
        {
            TenantId = tenantId,
            CompanyNumber = companyNumber,
            CompanyGuid = Guid.NewGuid(),
            Code = code,
            Name = name,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenantCompanies.Add(company);
        await context.SaveChangesAsync();
        return company;
    }

    private static async Task SeedCompanyCacheAsync(
        FgsUserDbContext context,
        long tenantId,
        long companyId,
        string code,
        string name)
    {
        context.FgsTenantCompanyCaches.Add(new FgsTenantCompanyCache
        {
            TenantId = tenantId,
            CompanyId = companyId,
            CompanyGuid = Guid.NewGuid(),
            CompanyCode = code,
            CompanyName = name,
            IsActive = true,
            UpdatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
    }

    private static CompanyDetailDto SampleCompanyDetail(long tenantId, long companyNumber) =>
        new(1, tenantId, companyNumber, Guid.NewGuid(), "ACME", "Acme Co", null, null, null, null, null, null, null, true, null, null);

    private static LocationWriteDto PhysicalAddress() =>
        new("123 Main St", null, null, null, "Austin", "TX", null, "US", "78701", null, null, null, null);

    private static LocationWriteDto BillingAddress() =>
        new("456 Billing Rd", null, null, null, "Dallas", "TX", null, "US", "75201", null, null, null, null);

    private static Mock<IFgsUserContext> UnauthenticatedContext()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        userContext.SetupGet(c => c.DisplayName).Returns("test");
        return userContext;
    }
}

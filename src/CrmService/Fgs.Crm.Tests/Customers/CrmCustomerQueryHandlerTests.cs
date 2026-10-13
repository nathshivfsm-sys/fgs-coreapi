using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerServiceLocationById;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCompanyServiceLocations;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomerServiceLocations;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Crm.Infrastructure.Persistence.Customers;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Crm.Tests.Customers;

public sealed class CrmCustomerQueryHandlerTests
{
    private static CrmCustomerDetailDto SampleDetail() =>
        new(
            1,
            "CUST01",
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
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
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            "ACCT-100",
            null,
            null,
            true);

    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(SampleDetail());

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetCrmCustomerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetCrmCustomerByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CrmCustomerDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetCrmCustomerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetCrmCustomerByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCustomerListFilters>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CrmCustomerListResultDto(
                [],
                1,
                25,
                0,
                new CrmCustomerListSummaryDto(0, 0, 0, 0, 0, null)));

        var handler = new ListCrmCustomersQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListCrmCustomersQuery(new CrmListQuery(), new CrmCustomerListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Summary.TotalCustomers.Should().Be(0);
    }

    [Fact]
    public async Task ListServiceLocations_WhenCustomerMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListServiceLocationsAsync(
                99,
                It.IsAny<CrmListQuery>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CrmServiceLocationListResultDto?)null);

        var handler = new ListCrmCustomerServiceLocationsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListCrmCustomerServiceLocationsQuery(99, new CrmListQuery()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task ListCompanyServiceLocations_ReturnsOkWithoutNotFound()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        var result = new CrmCompanyServiceLocationListResultDto(
            [],
            1,
            10,
            0,
            new CrmCompanyServiceLocationListSummaryDto(0, 0, 0, 0));
        readRepository
            .Setup(r => r.ListCompanyServiceLocationsAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCompanyServiceLocationListFilters>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var setupClient = new Mock<ISetupClient>(MockBehavior.Strict);
        var handler = new ListCrmCompanyServiceLocationsQueryHandler(readRepository.Object, setupClient.Object);
        var response = await handler.Handle(
            new ListCrmCompanyServiceLocationsQuery(
                new CrmListQuery(IsActive: null),
                new CrmCompanyServiceLocationListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data.Should().BeSameAs(result);
        setupClient.Verify(
            c => c.LookupGlBreaksAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ListCompanyServiceLocations_ResolvesBreakNamesFromSetup()
    {
        var resolved = CompanyLocation(1, break1Id: 100, break2Id: 200);
        var missing = CompanyLocation(2, break1Id: 300, break2Id: null);
        var result = new CrmCompanyServiceLocationListResultDto(
            [resolved, missing],
            1,
            10,
            2,
            new CrmCompanyServiceLocationListSummaryDto(2, 2, 0, 3));

        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListCompanyServiceLocationsAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCompanyServiceLocationListFilters>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.LookupGlBreaksAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<IReadOnlyList<GlBreakLookupItemDto>>.Ok(
            [
                new GlBreakLookupItemDto(100, "West"),
                new GlBreakLookupItemDto(200, "HVAC")
            ]));

        var handler = new ListCrmCompanyServiceLocationsQueryHandler(readRepository.Object, setupClient.Object);
        var response = await handler.Handle(
            new ListCrmCompanyServiceLocationsQuery(
                new CrmListQuery(IsActive: null),
                new CrmCompanyServiceLocationListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(2);
        response.Data.Items[0].Break1Name.Should().Be("West");
        response.Data.Items[0].Break2Name.Should().Be("HVAC");
        response.Data.Items[1].Break1Name.Should().BeNull();
        response.Data.Items[1].Break2Name.Should().BeNull();
        setupClient.Verify(c => c.LookupGlBreaksAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListCompanyServiceLocations_WhenSetupLookupFails_ReturnsFailure()
    {
        var result = new CrmCompanyServiceLocationListResultDto(
            [CompanyLocation(1, break1Id: 100, break2Id: 200)],
            1,
            10,
            1,
            new CrmCompanyServiceLocationListSummaryDto(1, 1, 0, 2));

        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListCompanyServiceLocationsAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCompanyServiceLocationListFilters>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.LookupGlBreaksAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<IReadOnlyList<GlBreakLookupItemDto>>.Fail(
                ["GL break lookup failed."],
                ApiStatusCodes.Forbidden));

        var handler = new ListCrmCompanyServiceLocationsQueryHandler(readRepository.Object, setupClient.Object);
        var response = await handler.Handle(
            new ListCrmCompanyServiceLocationsQuery(
                new CrmListQuery(IsActive: null),
                new CrmCompanyServiceLocationListFilters()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Forbidden);
        response.Errors.Should().Contain("GL break lookup failed.");
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task ListCompanyServiceLocations_WhenSetupLookupThrows_ReturnsFailure()
    {
        var result = new CrmCompanyServiceLocationListResultDto(
            [CompanyLocation(1, break1Id: 100, break2Id: null)],
            1,
            10,
            1,
            new CrmCompanyServiceLocationListSummaryDto(1, 1, 0, 1));

        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListCompanyServiceLocationsAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCompanyServiceLocationListFilters>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.LookupGlBreaksAsync(false, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Setup unreachable."));

        var handler = new ListCrmCompanyServiceLocationsQueryHandler(readRepository.Object, setupClient.Object);
        var response = await handler.Handle(
            new ListCrmCompanyServiceLocationsQuery(
                new CrmListQuery(IsActive: null),
                new CrmCompanyServiceLocationListFilters()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.InternalServerError);
        response.Errors.Should().Contain("Setup unreachable.");
        response.Data.Should().BeNull();
    }

    private static CrmCompanyServiceLocationSummaryDto CompanyLocation(
        long id,
        long? break1Id,
        long? break2Id) =>
        new(
            id,
            10,
            "Acme",
            "LOC-1",
            "Main",
            "Main",
            null,
            null,
            null,
            true,
            null,
            break1Id,
            break2Id);

    [Fact]
    public async Task GetServiceLocationById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.GetServiceLocationByIdAsync(1, 99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CrmServiceLocationDetailDto?)null);

        var handler = new GetCrmCustomerServiceLocationByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new GetCrmCustomerServiceLocationByIdQuery(1, 99),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }
}

public sealed class CrmCustomerReadRepositoryTests
{
    [Fact]
    public async Task List_ActiveAndInactiveQueriesDoNotMix()
    {
        await using var context = await CreateContextAsync();
        var active = await SeedCustomerAsync(context, "CUST01", "Acme", isActive: true);
        await SeedCustomerAsync(context, "CUST02", "Inactive Co", isActive: false);

        var repository = new CrmCustomerReadRepository(context);
        var activePage = await repository.ListAsync(new CrmListQuery(IsActive: true), new CrmCustomerListFilters(), false);
        var inactivePage = await repository.ListAsync(new CrmListQuery(IsActive: false), new CrmCustomerListFilters(), false);

        activePage.Items.Select(i => i.Id).Should().Equal(active.Id);
        inactivePage.Items.Should().ContainSingle(i => i.CustomerNumber == "CUST02");
        inactivePage.Items.Should().NotContain(i => i.Id == active.Id);
    }

    [Fact]
    public async Task List_SearchMatchesInsideSelectedTab()
    {
        await using var context = await CreateContextAsync();
        var active = await SeedCustomerAsync(
            context,
            "CUST01",
            "Acme",
            formattedAddress: "100 Main St, Austin",
            addressLine1: "100 Main St",
            postalCode: "78701",
            contactName: "Jane Doe",
            email: "jane@acme.com",
            phone: "+15550100");
        await SeedCustomerAsync(
            context,
            "CUST09",
            "Ghost",
            isActive: false,
            email: "jane@acme.com",
            contactName: "Jane Doe",
            postalCode: "78701");

        var repository = new CrmCustomerReadRepository(context);

        var byNumber = await repository.ListAsync(new CrmListQuery(Search: "CUST01", IsActive: true), new CrmCustomerListFilters(), false);
        var byName = await repository.ListAsync(new CrmListQuery(Search: "acme", IsActive: true), new CrmCustomerListFilters(), false);
        var byEmail = await repository.ListAsync(new CrmListQuery(Search: "jane@acme.com", IsActive: true), new CrmCustomerListFilters(), false);
        var byPhone = await repository.ListAsync(new CrmListQuery(Search: "5550100", IsActive: true), new CrmCustomerListFilters(), false);
        var byAddress = await repository.ListAsync(new CrmListQuery(Search: "Main St", IsActive: true), new CrmCustomerListFilters(), false);
        var byPostal = await repository.ListAsync(new CrmListQuery(Search: "78701", IsActive: true), new CrmCustomerListFilters(), false);
        var inactiveEmail = await repository.ListAsync(new CrmListQuery(Search: "jane@acme.com", IsActive: false), new CrmCustomerListFilters(), false);

        byNumber.Items.Should().ContainSingle(i => i.Id == active.Id);
        byName.Items.Should().ContainSingle(i => i.Id == active.Id);
        byEmail.Items.Should().ContainSingle(i => i.Id == active.Id);
        byPhone.Items.Should().ContainSingle(i => i.Id == active.Id);
        byAddress.Items.Should().ContainSingle(i => i.Id == active.Id);
        byPostal.Items.Should().ContainSingle(i => i.Id == active.Id);
        inactiveEmail.Items.Should().ContainSingle(i => i.CustomerNumber == "CUST09");
        inactiveEmail.Items.Should().NotContain(i => i.Id == active.Id);
    }

    [Fact]
    public async Task List_IsPreferredCombinesWithSearch_AndDefaultOrderIsNameAscending()
    {
        await using var context = await CreateContextAsync();
        await SeedCustomerAsync(context, "CUST02", "Zebra", isPreferred: false);
        await SeedCustomerAsync(context, "CUST01", "Acme", isPreferred: true);
        await SeedCustomerAsync(context, "CUST03", "Acorn", isPreferred: false);

        var repository = new CrmCustomerReadRepository(context);
        var ordered = await repository.ListAsync(new CrmListQuery(IsActive: true), new CrmCustomerListFilters(), false);
        var preferred = await repository.ListAsync(
            new CrmListQuery(Search: "ac", IsActive: true),
            new CrmCustomerListFilters(IsPreferred: true),
            false);

        ordered.Items.Select(i => i.Name).Should().Equal("Acme", "Acorn", "Zebra");
        preferred.Items.Should().ContainSingle(i => i.Name == "Acme");
        preferred.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task List_SummaryIgnoresFilters_AndPercentIsNullWithoutPreviousMonth()
    {
        await using var context = await CreateContextAsync();
        await SeedCustomerAsync(context, "CUST01", "Acme", isPreferred: true);
        await SeedCustomerAsync(context, "CUST02", "Other", isPreferred: false);
        await SeedCustomerAsync(context, "CUST03", "Old", isActive: false, createdOn: DateTimeOffset.UtcNow);

        var repository = new CrmCustomerReadRepository(context);
        var filtered = await repository.ListAsync(
            new CrmListQuery(Search: "Acme", IsActive: true),
            new CrmCustomerListFilters(IsPreferred: true),
            includeSummary: true);
        var withoutSummary = await repository.ListAsync(
            new CrmListQuery(IsActive: null),
            new CrmCustomerListFilters(),
            includeSummary: false);

        filtered.TotalCount.Should().Be(1);
        filtered.Summary.TotalCustomers.Should().Be(3);
        filtered.Summary.ActiveCustomers.Should().Be(2);
        filtered.Summary.InactiveCustomers.Should().Be(1);
        filtered.Summary.NewThisMonth.Should().Be(3);
        filtered.Summary.PreviousMonthNewCustomers.Should().Be(0);
        filtered.Summary.NewCustomerPercentChange.Should().BeNull();
        withoutSummary.Summary.Should().Be(new CrmCustomerListSummaryDto(0, 0, 0, 0, 0, null));
        withoutSummary.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task List_SummaryPercentUsesPreviousUtcMonth_AndDetailIncludesContact()
    {
        await using var context = await CreateContextAsync();
        var monthStart = new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var current = await SeedCustomerAsync(context, "CUST01", "Acme", createdOn: monthStart.AddDays(1));
        await SeedCustomerAsync(context, "CUST02", "Prior", createdOn: monthStart.AddDays(-1));
        context.CrmEntityTags.AddRange(
            new CrmEntityTag
            {
                TenantId = 10,
                CompanyId = 20,
                TagId = 8,
                EntityTypeId = (int)CrmTaggedEntityType.Customer,
                EntityId = current.Id,
                DisplayOrder = 2,
                CreatedOn = DateTimeOffset.UtcNow
            },
            new CrmEntityTag
            {
                TenantId = 10,
                CompanyId = 20,
                TagId = 3,
                EntityTypeId = (int)CrmTaggedEntityType.Customer,
                EntityId = current.Id,
                DisplayOrder = 1,
                CreatedOn = DateTimeOffset.UtcNow
            },
            new CrmEntityTag
            {
                TenantId = 10,
                CompanyId = 20,
                TagId = 99,
                EntityTypeId = (int)CrmTaggedEntityType.ServiceLocation,
                EntityId = current.Id,
                DisplayOrder = 0,
                CreatedOn = DateTimeOffset.UtcNow
            });
        await context.SaveChangesAsync();

        var repository = new CrmCustomerReadRepository(context);
        var list = await repository.ListAsync(new CrmListQuery(IsActive: true), new CrmCustomerListFilters(), true);
        var detail = await repository.GetByIdAsync(current.Id);

        list.Summary.NewThisMonth.Should().Be(1);
        list.Summary.PreviousMonthNewCustomers.Should().Be(1);
        list.Summary.NewCustomerPercentChange.Should().Be(0m);
        var row = list.Items.Single(i => i.Id == current.Id);
        row.TagIds.Should().Equal(3L, 8L);
        row.Email.Should().Be("jane@acme.com");
        row.PrimaryContactName.Should().Be("Jane Doe");
        detail!.Website.Should().Be("https://acme.example");
        detail.PrimaryContactName.Should().Be("Jane Doe");
        detail.PrimaryContactEmail.Should().Be("jane@acme.com");
        detail.PrimaryContactPhone.Should().Be("+15550100");
        detail.CreatedOn.Should().Be(current.CreatedOn);
        detail.TagIds.Should().Equal(3L, 8L);
    }

    [Fact]
    public async Task ListServiceLocations_ActiveAndInactiveDoNotMix_AndSummaryIgnoresSearch()
    {
        await using var context = await CreateContextAsync();
        var customer = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var active = await SeedLocationAsync(
            context,
            customer.Id,
            1,
            "CUST01-1",
            "Warehouse",
            "Warehouse dock",
            formattedAddress: "200 Oak Ave",
            addressLine1: "200 Oak Ave",
            postalCode: "78702",
            email: "site@acme.com",
            phone: "+15550199",
            customerType: CustomerType.PropertyManagement);
        await SeedLocationAsync(
            context,
            customer.Id,
            2,
            "CUST01-2",
            "Closed yard",
            "Closed yard",
            isActive: false,
            email: "site@acme.com",
            postalCode: "78702");

        var repository = new CrmCustomerReadRepository(context);
        var activePage = await repository.ListServiceLocationsAsync(customer.Id, new CrmListQuery(IsActive: true), true);
        var inactivePage = await repository.ListServiceLocationsAsync(customer.Id, new CrmListQuery(IsActive: false), true);
        var searched = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "Warehouse", IsActive: true),
            true);
        var withoutSummary = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(IsActive: true),
            includeSummary: false);
        var missing = await repository.ListServiceLocationsAsync(999, new CrmListQuery());

        activePage!.Items.Select(i => i.Id).Should().Equal(active.Id);
        activePage.Items.Single().CustomerType.Should().Be(CustomerType.PropertyManagement);
        inactivePage!.Items.Single().CustomerType.Should().BeNull();
        activePage.Items.Should().NotContain(i => !i.IsActive);
        inactivePage!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-2");
        inactivePage.Items.Should().NotContain(i => i.Id == active.Id);
        searched!.TotalCount.Should().Be(1);
        searched.Items.Should().ContainSingle(i => i.Id == active.Id);
        searched.Summary.ActiveCount.Should().Be(1);
        searched.Summary.InactiveCount.Should().Be(1);
        withoutSummary!.Summary.Should().Be(new CrmServiceLocationListSummaryDto(0, 0));
        withoutSummary.TotalCount.Should().Be(1);
        missing.Should().BeNull();
    }

    [Fact]
    public async Task ListServiceLocations_SearchStaysInSelectedTab_AndDefaultSortIsName()
    {
        await using var context = await CreateContextAsync();
        var customer = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        await SeedLocationAsync(
            context,
            customer.Id,
            1,
            "CUST01-1",
            "Zebra",
            "Zebra site",
            email: "site@acme.com",
            phone: "+15550199",
            addressLine1: "9 Pine",
            postalCode: "78709");
        await SeedLocationAsync(
            context,
            customer.Id,
            2,
            "CUST01-2",
            "Acme Yard",
            "Acme display",
            formattedAddress: "100 Main St");
        await SeedLocationAsync(
            context,
            customer.Id,
            3,
            "CUST01-3",
            "Acme Yard",
            "Inactive twin",
            isActive: false,
            email: "site@acme.com",
            postalCode: "78709");

        var repository = new CrmCustomerReadRepository(context);
        var byEmail = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "site@acme.com", IsActive: true));
        var byPhone = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "5550199", IsActive: true));
        var byPostal = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "78709", IsActive: true));
        var byAddress = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "Main St", IsActive: true));
        var inactiveEmail = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(Search: "site@acme.com", IsActive: false));
        var ordered = await repository.ListServiceLocationsAsync(customer.Id, new CrmListQuery(IsActive: true));
        var byId = await repository.ListServiceLocationsAsync(
            customer.Id,
            new CrmListQuery(SortBy: "not-a-column", IsActive: true));

        byEmail!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-1");
        byEmail.Items.Single().Email.Should().Be("site@acme.com");
        byEmail.Items.Single().Phone.Should().Be("+15550199");
        byPhone!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-1");
        byPostal!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-1");
        byAddress!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-2");
        inactiveEmail!.Items.Should().ContainSingle(i => i.LocationNumber == "CUST01-3");
        inactiveEmail.Items.Should().NotContain(i => i.IsActive);
        ordered!.Items.Select(i => i.Name).Should().Equal("Acme Yard", "Zebra");
        byId!.Items.Select(i => i.LocationNumber).Should().Equal("CUST01-1", "CUST01-2");
    }

    [Fact]
    public async Task GetServiceLocationById_ReturnsLocationOnlyWhenItBelongsToCustomer()
    {
        await using var context = await CreateContextAsync();
        var owner = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var other = await SeedCustomerAsync(context, "CUST02", "Other", contactName: null, email: null, phone: null);
        var location = await SeedLocationAsync(
            context,
            owner.Id,
            1,
            "CUST01-1",
            "Warehouse",
            "Warehouse dock",
            addressLine1: "200 Oak Ave",
            postalCode: "78702",
            email: "site@acme.com",
            phone: "+15550199",
            customerType: CustomerType.PropertyManagement);
        var foreign = await SeedLocationAsync(
            context,
            other.Id,
            1,
            "CUST02-1",
            "Other site",
            "Other site");

        location.State = "TX";
        location.Country = "US";
        location.City = "Austin";
        location.ServiceLocationType = ServiceLocationType.Industrial;
        var contact = await context.CrmContacts.SingleAsync(c => c.ServiceLocationId == location.Id);
        contact.DisplayName = "Site Lead";
        contact.CanReceiveEstimates = true;
        contact.CanReceiveInvoices = true;
        contact.CanReceiveAppointments = false;
        context.CrmEntityTags.AddRange(
            new CrmEntityTag
            {
                TenantId = 10,
                CompanyId = 20,
                TagId = 4,
                EntityTypeId = (int)CrmTaggedEntityType.ServiceLocation,
                EntityId = location.Id,
                DisplayOrder = 0,
                CreatedOn = DateTimeOffset.UtcNow
            },
            new CrmEntityTag
            {
                TenantId = 10,
                CompanyId = 20,
                TagId = 1,
                EntityTypeId = (int)CrmTaggedEntityType.ServiceLocation,
                EntityId = location.Id,
                DisplayOrder = 1,
                CreatedOn = DateTimeOffset.UtcNow
            });
        await context.SaveChangesAsync();

        var repository = new CrmCustomerReadRepository(context);
        var detail = await repository.GetServiceLocationByIdAsync(owner.Id, location.Id);
        var ownedByOtherCustomer = await repository.GetServiceLocationByIdAsync(other.Id, location.Id);
        var otherLocationFromOwner = await repository.GetServiceLocationByIdAsync(owner.Id, foreign.Id);
        var missingCustomer = await repository.GetServiceLocationByIdAsync(999, location.Id);
        var missingLocation = await repository.GetServiceLocationByIdAsync(owner.Id, 999);

        detail.Should().NotBeNull();
        detail!.CustomerId.Should().Be(owner.Id);
        detail.LocationNumber.Should().Be("CUST01-1");
        detail.LocationSequence.Should().Be(1);
        detail.Name.Should().Be("Warehouse");
        detail.AddressLine1.Should().Be("200 Oak Ave");
        detail.City.Should().Be("Austin");
        detail.State.Should().Be("TX");
        detail.Country.Should().Be("US");
        detail.PostalCode.Should().Be("78702");
        detail.ServiceLocationType.Should().Be(ServiceLocationType.Industrial);
        detail.CustomerType.Should().Be(CustomerType.PropertyManagement);
        detail.PrimaryContactName.Should().Be("Site Lead");
        detail.PrimaryContactEmail.Should().Be("site@acme.com");
        detail.PrimaryContactPhone.Should().Be("+15550199");
        detail.CanReceiveEstimates.Should().BeTrue();
        detail.CanReceiveInvoices.Should().BeTrue();
        detail.CanReceiveAppointments.Should().BeFalse();
        detail.TagIds.Should().Equal(4L, 1L);
        ownedByOtherCustomer.Should().BeNull();
        otherLocationFromOwner.Should().BeNull();
        missingCustomer.Should().BeNull();
        missingLocation.Should().BeNull();
    }

    [Fact]
    public async Task ListCompanyServiceLocations_DefaultOrderIsLocationNumber_NotName()
    {
        await using var context = await CreateContextAsync();
        var firstCustomer = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var secondCustomer = await SeedCustomerAsync(context, "CUST02", "Bravo", contactName: null, email: null, phone: null);
        var laterNumber = await SeedLocationAsync(
            context,
            firstCustomer.Id,
            1,
            "ZZ-9",
            "Alpha",
            "Alpha site",
            email: "z@acme.com",
            phone: "+1999");
        var earlierNumber = await SeedLocationAsync(
            context,
            secondCustomer.Id,
            1,
            "AA-1",
            "Zulu",
            "Zulu site",
            email: "a@acme.com",
            phone: "+1000");

        var repository = new CrmCustomerReadRepository(context);
        var ordered = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: null),
            new CrmCompanyServiceLocationListFilters());
        var page = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(PageSize: 1, IsActive: null),
            new CrmCompanyServiceLocationListFilters());
        var byUnknown = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(SortBy: "not-a-column", IsActive: null),
            new CrmCompanyServiceLocationListFilters());
        var byEmail = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(SortBy: "email", IsActive: null),
            new CrmCompanyServiceLocationListFilters());
        var byPhone = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(SortBy: "phone", IsActive: null),
            new CrmCompanyServiceLocationListFilters());

        ordered.Items.Select(i => i.LocationNumber).Should().Equal("AA-1", "ZZ-9");
        ordered.Items.Select(i => i.Name).Should().Equal("Zulu", "Alpha");
        ordered.Items.Select(i => i.CustomerName).Should().Equal("Bravo", "Acme");
        page.Items.Should().ContainSingle(i => i.LocationNumber == "AA-1");
        page.TotalCount.Should().Be(2);
        page.PageSize.Should().Be(1);
        byUnknown.Items.Select(i => i.Id).Should().Equal(laterNumber.Id, earlierNumber.Id);
        byEmail.Items.Select(i => i.Email).Should().Equal("a@acme.com", "z@acme.com");
        byPhone.Items.Select(i => i.Phone).Should().Equal("+1000", "+1999");
    }

    [Fact]
    public async Task ListCompanyServiceLocations_SearchMatchesCityAndNumericId()
    {
        await using var context = await CreateContextAsync();
        var customer = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var inCity = await SeedLocationAsync(
            context,
            customer.Id,
            1,
            "ALPHA",
            "North",
            "North site",
            city: "Cedar Park");
        var other = await SeedLocationAsync(
            context,
            customer.Id,
            2,
            "BRAVO",
            "South",
            "South site",
            city: "Dallas");

        var repository = new CrmCustomerReadRepository(context);
        var byCity = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(Search: "cedar", IsActive: null),
            new CrmCompanyServiceLocationListFilters());
        var byId = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(Search: other.Id.ToString(), IsActive: null),
            new CrmCompanyServiceLocationListFilters());

        byCity.Items.Should().ContainSingle(i => i.Id == inCity.Id);
        byCity.TotalCount.Should().Be(1);
        byId.Items.Should().ContainSingle(i => i.Id == other.Id);
    }

    [Fact]
    public async Task ListCompanyServiceLocations_FiltersNarrowItems_SummaryCountsEveryLocation()
    {
        await using var context = await CreateContextAsync();
        var included = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var other = await SeedCustomerAsync(context, "CUST02", "Other", contactName: null, email: null, phone: null);
        var match = await SeedLocationAsync(
            context,
            included.Id,
            1,
            "MATCH",
            "Match",
            "Match",
            customerType: CustomerType.PropertyManagement);
        await SeedLocationAsync(
            context,
            included.Id,
            2,
            "TYPE",
            "Wrong type",
            "Wrong type",
            customerType: CustomerType.Commercial);
        await SeedLocationAsync(
            context,
            included.Id,
            3,
            "INACTIVE",
            "Inactive",
            "Inactive",
            isActive: false,
            customerType: CustomerType.PropertyManagement);
        await SeedLocationAsync(
            context,
            other.Id,
            1,
            "OTHER",
            "Other site",
            "Other site",
            customerType: CustomerType.PropertyManagement);

        var repository = new CrmCustomerReadRepository(context);
        var filters = new CrmCompanyServiceLocationListFilters(
            [included.Id],
            [CustomerType.PropertyManagement]);
        var page = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: true),
            filters,
            includeSummary: true);
        var withoutSummary = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: true),
            filters,
            includeSummary: false);

        page.Items.Should().ContainSingle(i => i.Id == match.Id);
        page.TotalCount.Should().Be(1);
        page.Summary.Should().Be(new CrmCompanyServiceLocationListSummaryDto(4, 3, 1, 0));
        withoutSummary.Summary.Should().Be(new CrmCompanyServiceLocationListSummaryDto(0, 0, 0, 0));
        withoutSummary.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task ListCompanyServiceLocations_AddressUsesFormattedValueOrJoinedParts()
    {
        await using var context = await CreateContextAsync();
        var customer = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var formatted = await SeedLocationAsync(
            context,
            customer.Id,
            1,
            "FMT",
            "Formatted",
            "Formatted",
            formattedAddress: "  500 Pine St  ",
            addressLine1: "Should not win",
            city: "Austin");
        var joined = await SeedLocationAsync(
            context,
            customer.Id,
            2,
            "JOIN",
            "Joined",
            "Joined",
            addressLine1: "10 Oak",
            postalCode: "78701",
            city: "Austin");
        joined.AddressLine2 = "Suite 4";
        joined.State = "TX";
        joined.Country = "US";
        joined.AddressLine3 = " ";
        var orphan = new CrmServiceLocation
        {
            TenantId = 10,
            CompanyId = 20,
            CustomerId = 999,
            LocationSequence = 1,
            LocationNumber = "ORPHAN",
            Name = "Orphan",
            DisplayName = "Orphan",
            ServiceLocationType = ServiceLocationType.Commercial,
            AddressLine1 = "1 Lone St",
            City = "Austin",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.CrmServiceLocations.Add(orphan);
        await context.SaveChangesAsync();

        var repository = new CrmCustomerReadRepository(context);
        var page = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: null),
            new CrmCompanyServiceLocationListFilters());

        page.Items.Single(i => i.Id == formatted.Id).Address.Should().Be("500 Pine St");
        page.Items.Single(i => i.Id == formatted.Id).CustomerName.Should().Be("Acme");
        page.Items.Single(i => i.Id == joined.Id).Address.Should().Be("10 Oak, Suite 4, Austin, TX, 78701, US");
        page.Items.Single(i => i.Id == orphan.Id).CustomerName.Should().BeEmpty();
    }

    [Fact]
    public async Task ListCompanyServiceLocations_BusinessUnitsCountDistinctBreakIdsAndIgnoreFilters()
    {
        await using var context = await CreateContextAsync();
        var included = await SeedCustomerAsync(context, "CUST01", "Acme", contactName: null, email: null, phone: null);
        var other = await SeedCustomerAsync(context, "CUST02", "Other", contactName: null, email: null, phone: null);
        var shared = await SeedLocationAsync(
            context,
            included.Id,
            1,
            "SHARED",
            "Shared",
            "Shared",
            break1Id: 10,
            break2Id: 20);
        var inactive = await SeedLocationAsync(
            context,
            included.Id,
            2,
            "INACTIVE",
            "Inactive",
            "Inactive",
            isActive: false,
            break1Id: 10);
        await SeedLocationAsync(
            context,
            other.Id,
            1,
            "EMPTY",
            "Empty",
            "Empty");

        var repository = new CrmCustomerReadRepository(context);
        var filters = new CrmCompanyServiceLocationListFilters([included.Id]);
        var page = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: true),
            filters,
            includeSummary: true);
        var withoutSummary = await repository.ListCompanyServiceLocationsAsync(
            new CrmListQuery(IsActive: true),
            filters,
            includeSummary: false);

        page.Items.Should().ContainSingle(i => i.Id == shared.Id);
        page.Items.Single().Break1Id.Should().Be(10);
        page.Items.Single().Break2Id.Should().Be(20);
        page.TotalCount.Should().Be(1);
        page.Summary.Should().Be(new CrmCompanyServiceLocationListSummaryDto(3, 2, 1, 2));
        inactive.Break2Id.Should().BeNull();
        withoutSummary.Summary.Should().Be(new CrmCompanyServiceLocationListSummaryDto(0, 0, 0, 0));
        withoutSummary.TotalCount.Should().Be(1);
    }

    private static async Task<CrmServiceLocation> SeedLocationAsync(
        FgsCrmDbContext context,
        long customerId,
        int sequence,
        string locationNumber,
        string name,
        string displayName,
        bool isActive = true,
        string? formattedAddress = null,
        string? addressLine1 = null,
        string? postalCode = null,
        string? email = null,
        string? phone = null,
        CustomerType? customerType = null,
        string? city = null,
        long? break1Id = null,
        long? break2Id = null)
    {
        var location = new CrmServiceLocation
        {
            TenantId = 10,
            CompanyId = 20,
            CustomerId = customerId,
            LocationSequence = sequence,
            LocationNumber = locationNumber,
            Name = name,
            DisplayName = displayName,
            ServiceLocationType = ServiceLocationType.Commercial,
            CustomerType = customerType,
            Break1Id = break1Id,
            Break2Id = break2Id,
            FormattedAddress = formattedAddress,
            AddressLine1 = addressLine1,
            PostalCode = postalCode,
            City = city,
            IsActive = isActive,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.CrmServiceLocations.Add(location);
        await context.SaveChangesAsync();

        if (email is null && phone is null)
        {
            return location;
        }

        var contact = new CrmContact
        {
            TenantId = 10,
            CompanyId = 20,
            CustomerId = null,
            ServiceLocationId = location.Id,
            DisplayName = "Site Contact",
            IsDefaultContact = true,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.CrmContacts.Add(contact);
        await context.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(email))
        {
            context.CrmContactCommunications.Add(new CrmContactCommunication
            {
                TenantId = 10,
                CompanyId = 20,
                ContactId = contact.Id,
                CommunicationTypeId = (short)ContactCommunicationType.Email,
                Value = email,
                IsPrimary = true,
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            context.CrmContactCommunications.Add(new CrmContactCommunication
            {
                TenantId = 10,
                CompanyId = 20,
                ContactId = contact.Id,
                CommunicationTypeId = (short)ContactCommunicationType.Phone,
                Value = phone,
                IsPrimary = true,
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        await context.SaveChangesAsync();
        return location;
    }

    private static async Task<CrmCustomer> SeedCustomerAsync(
        FgsCrmDbContext context,
        string number,
        string name,
        bool isActive = true,
        bool isPreferred = false,
        string? formattedAddress = null,
        string? addressLine1 = null,
        string? postalCode = null,
        string? contactName = "Jane Doe",
        string? email = "jane@acme.com",
        string? phone = "+15550100",
        DateTimeOffset? createdOn = null)
    {
        var customer = new CrmCustomer
        {
            TenantId = 10,
            CompanyId = 20,
            CustomerNumber = number,
            Name = name,
            DisplayName = name,
            FormattedAddress = formattedAddress,
            AddressLine1 = addressLine1,
            PostalCode = postalCode,
            Website = "https://acme.example",
            IsActive = isActive,
            IsPreferredCustomer = isPreferred,
            CreatedOn = createdOn ?? DateTimeOffset.UtcNow
        };
        context.CrmCustomers.Add(customer);
        await context.SaveChangesAsync();

        if (contactName is null && email is null && phone is null)
        {
            return customer;
        }

        var contact = new CrmContact
        {
            TenantId = 10,
            CompanyId = 20,
            CustomerId = customer.Id,
            ServiceLocationId = null,
            DisplayName = contactName ?? "Contact",
            IsDefaultContact = true,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.CrmContacts.Add(contact);
        await context.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(email))
        {
            context.CrmContactCommunications.Add(new CrmContactCommunication
            {
                TenantId = 10,
                CompanyId = 20,
                ContactId = contact.Id,
                CommunicationTypeId = (short)ContactCommunicationType.Email,
                Value = email,
                IsPrimary = true,
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            context.CrmContactCommunications.Add(new CrmContactCommunication
            {
                TenantId = 10,
                CompanyId = 20,
                ContactId = contact.Id,
                CommunicationTypeId = (short)ContactCommunicationType.Phone,
                Value = phone,
                IsPrimary = true,
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow
            });
        }

        await context.SaveChangesAsync();
        return customer;
    }

    private static async Task<FgsCrmDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsCrmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsCrmDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}

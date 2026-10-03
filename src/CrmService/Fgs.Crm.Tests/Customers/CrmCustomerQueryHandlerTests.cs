using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;
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
        CustomerType? customerType = null)
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
            FormattedAddress = formattedAddress,
            AddressLine1 = addressLine1,
            PostalCode = postalCode,
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

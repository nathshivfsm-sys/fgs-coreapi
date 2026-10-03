using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Crm.Infrastructure.Persistence.Customers;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Crm.Tests.Customers;

public sealed class CrmCustomerCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static CrmCustomerCreateDto SampleCreateDto() =>
        new(
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
            null);

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateCrmCustomerCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateCrmCustomerCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateCrmCustomerCommand(SampleCreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Customer.IsActive.Should().BeTrue();
        response.Data.Customer.CustomerNumber.Should().Be("CUST01");
        response.Data.ServiceLocationId.Should().BeNull();
        (await context.CrmServiceLocations.CountAsync()).Should().Be(0);
        (await context.CrmContacts.CountAsync()).Should().Be(0);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "customer"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_BillToOnly_RejectsLocationPayloadBeforeInsert()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var dto = SampleCreateDto() with
        {
            ServiceLocation = new CrmServiceLocationCreateDto(ServiceLocationType.Residential)
        };

        var act = () => writeService.CreateAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>();
        (await context.CrmCustomers.CountAsync()).Should().Be(0);
        (await context.CrmServiceLocations.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Create_BillToAndServiceLocation_RequiresLocationPayload()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var dto = SampleCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = null
        };

        var act = () => writeService.CreateAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>();
        (await context.CrmCustomers.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Create_CombinedOption_CreatesLocationCopiesDefaultsAndGeneratesNumber()
    {
        await using var context = await CreateContextAsync();
        context.CrmDefaultCustomers.Add(new CrmDefaultCustomer
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            DefaultPaymentTermId = 99,
            DefaultMaterialPricingMatrixId = 11,
            DefaultLaborPricingMatrixId = 12,
            DefaultOtherPricingMatrixId = 13,
            DefaultPORequired = true,
            TaxExempt = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        context.CrmDefaultServiceLocations.Add(new CrmDefaultServiceLocation
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            DefaultPaymentMethodId = 21,
            DefaultMaterialPricingMatrixId = 31,
            DefaultLaborPricingMatrixId = 32,
            DefaultOtherPricingMatrixId = 33,
            InvoiceEmailTemplateId = 41,
            EstimateEmailTemplateId = 42,
            InvoiceSmsTemplateId = 43,
            EstimateSmsTemplateId = 44,
            EmailAllowed = false,
            SmsAllowed = false,
            TaxExempt = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.SaveChangesAsync();

        var writeService = CreateWriteService(context);
        var result = await writeService.CreateAsync(SampleCreateDto() with
        {
            DefaultPaymentTermId = 5,
            DefaultMaterialPricingMatrixId = null,
            DefaultLaborPricingMatrixId = 77,
            DefaultPORequired = false,
            TaxExempt = false,
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                AddressLine1: "200 Oak")
        });

        result.ServiceLocationId.Should().NotBeNull();
        result.Customer.DefaultPaymentTermId.Should().Be(5);
        result.Customer.DefaultMaterialPricingMatrixId.Should().Be(11);
        result.Customer.DefaultLaborPricingMatrixId.Should().Be(77);
        result.Customer.DefaultOtherPricingMatrixId.Should().Be(13);
        result.Customer.DefaultPORequired.Should().BeFalse();
        result.Customer.TaxExempt.Should().BeFalse();

        var customer = await context.CrmCustomers.SingleAsync();
        customer.LastServiceLocationSequence.Should().Be(1);

        var location = await context.CrmServiceLocations.SingleAsync();
        location.Id.Should().Be(result.ServiceLocationId);
        location.CustomerId.Should().Be(customer.Id);
        location.LocationSequence.Should().Be(1);
        location.LocationNumber.Should().Be("CUST01-1");
        location.Name.Should().BeEmpty();
        location.DisplayName.Should().BeEmpty();
        location.ServiceLocationType.Should().Be(ServiceLocationType.Commercial);
        location.AddressLine1.Should().Be("200 Oak");
        location.DefaultPaymentMethodId.Should().Be(21);
        location.DefaultMaterialPricingMatrixId.Should().Be(31);
        location.DefaultLaborPricingMatrixId.Should().Be(32);
        location.DefaultOtherPricingMatrixId.Should().Be(33);
        location.InvoiceEmailTemplateId.Should().Be(41);
        location.EstimateEmailTemplateId.Should().Be(42);
        location.InvoiceSmsTemplateId.Should().Be(43);
        location.EstimateSmsTemplateId.Should().Be(44);
        location.EmailAllowed.Should().BeFalse();
        location.SmsAllowed.Should().BeFalse();
        location.TaxExempt.Should().BeTrue();
    }

    [Fact]
    public async Task Create_CombinedOption_UsesColumnDefaultsWhenSettingsRowIsMissing()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(SampleCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(ServiceLocationType.Residential)
        });

        var location = await context.CrmServiceLocations.SingleAsync();
        location.EmailAllowed.Should().BeTrue();
        location.SmsAllowed.Should().BeTrue();
        location.TaxExempt.Should().BeFalse();
        location.DefaultPaymentMethodId.Should().BeNull();
        location.DefaultMaterialPricingMatrixId.Should().BeNull();
        location.InvoiceEmailTemplateId.Should().BeNull();
    }

    [Fact]
    public async Task Create_InsertsContactOnlyWhenContactFieldsAreSent()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(SampleCreateDto());
        (await context.CrmContacts.CountAsync()).Should().Be(0);

        var created = await writeService.CreateAsync(SampleCreateDto() with
        {
            CustomerNumber = "CUST02",
            PrimaryContactName = "Jane Doe",
            PrimaryContactEmail = "jane@example.com",
            PrimaryContactPhone = "+1 555-0100",
            Website = "https://acme.example"
        });

        created.Customer.Website.Should().Be("https://acme.example");
        created.Customer.PrimaryContactName.Should().Be("Jane Doe");
        created.Customer.PrimaryContactEmail.Should().Be("jane@example.com");
        created.Customer.PrimaryContactPhone.Should().Be("+1 555-0100");

        var contact = await context.CrmContacts.SingleAsync();
        contact.CustomerId.Should().Be(created.Customer.Id);
        contact.ServiceLocationId.Should().BeNull();
        contact.IsDefaultContact.Should().BeTrue();
        contact.DisplayName.Should().Be("Jane Doe");

        var communications = await context.CrmContactCommunications.OrderBy(c => c.CommunicationTypeId).ToListAsync();
        communications.Should().HaveCount(2);
        communications.Should().OnlyContain(c => c.ContactId == contact.Id && c.IsPrimary);
        communications.Select(c => c.CommunicationTypeId).Should().Equal(
            (short)ContactCommunicationType.Email,
            (short)ContactCommunicationType.Phone);
    }

    [Fact]
    public async Task AddServiceLocation_WorksForCustomerWithNoLocations()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var created = await writeService.CreateAsync(SampleCreateDto());

        var added = await writeService.AddServiceLocationAsync(
            created.Customer.Id,
            new CrmServiceLocationCreateDto(ServiceLocationType.Industrial, "Warehouse", "Warehouse"));

        added.CustomerId.Should().Be(created.Customer.Id);
        added.LocationSequence.Should().Be(1);
        added.LocationNumber.Should().Be("CUST01-1");
        (await context.CrmCustomers.SingleAsync()).LastServiceLocationSequence.Should().Be(1);
        (await context.CrmServiceLocations.SingleAsync()).CustomerType.Should().BeNull();

        var typed = await writeService.AddServiceLocationAsync(
            created.Customer.Id,
            new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                "Office",
                "Office",
                CustomerType: CustomerType.PropertyManagement));

        typed.LocationSequence.Should().Be(2);
        var locations = await context.CrmServiceLocations.OrderBy(l => l.LocationSequence).ToListAsync();
        locations[1].CustomerType.Should().Be(CustomerType.PropertyManagement);

        var missing = () => writeService.AddServiceLocationAsync(
            999,
            new CrmServiceLocationCreateDto(ServiceLocationType.Other));
        await missing.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Create_StoresLocationContactSeparatelyFromCustomerContact()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        var created = await writeService.CreateAsync(SampleCreateDto() with
        {
            PrimaryContactName = "Jane Doe",
            PrimaryContactEmail = "jane@acme.example",
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Residential,
                "Main",
                "Main site",
                PrimaryContactName: "Site Lead",
                PrimaryContactEmail: "site@acme.example",
                PrimaryContactPhone: "+1 555-0199",
                CanReceiveEstimates: true,
                CanReceiveInvoices: true,
                CanReceiveAppointments: false)
        });

        var contacts = await context.CrmContacts.ToListAsync();
        contacts.Should().HaveCount(2);

        var customerContact = contacts.Single(c => c.CustomerId == created.Customer.Id);
        customerContact.ServiceLocationId.Should().BeNull();
        customerContact.DisplayName.Should().Be("Jane Doe");
        customerContact.CanReceiveEstimates.Should().BeFalse();
        customerContact.CanReceiveInvoices.Should().BeFalse();
        customerContact.CanReceiveAppointments.Should().BeTrue();

        var locationContact = contacts.Single(c => c.ServiceLocationId == created.ServiceLocationId);
        locationContact.CustomerId.Should().BeNull();
        locationContact.IsDefaultContact.Should().BeTrue();
        locationContact.DisplayName.Should().Be("Site Lead");
        locationContact.CanReceiveEstimates.Should().BeTrue();
        locationContact.CanReceiveInvoices.Should().BeTrue();
        locationContact.CanReceiveAppointments.Should().BeFalse();

        var locationCommunications = await context.CrmContactCommunications
            .Where(c => c.ContactId == locationContact.Id)
            .OrderBy(c => c.CommunicationTypeId)
            .ToListAsync();
        locationCommunications.Should().OnlyContain(c => c.IsPrimary);
        locationCommunications.Select(c => c.CommunicationTypeId).Should().Equal(
            (short)ContactCommunicationType.Email,
            (short)ContactCommunicationType.Phone);
    }

    [Fact]
    public async Task Create_LocationTaxExemptUsesRequestValueOtherwiseDefaultSettings()
    {
        await using var context = await CreateContextAsync();
        context.CrmDefaultServiceLocations.Add(new CrmDefaultServiceLocation
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            TaxExempt = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.SaveChangesAsync();

        var writeService = CreateWriteService(context);
        await writeService.CreateAsync(SampleCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Residential,
                "Main",
                "Main site",
                TaxExempt: false)
        });

        (await context.CrmServiceLocations.SingleAsync()).TaxExempt.Should().BeFalse();

        var added = await writeService.AddServiceLocationAsync(
            (await context.CrmCustomers.SingleAsync()).Id,
            new CrmServiceLocationCreateDto(ServiceLocationType.Commercial, "Warehouse", "Warehouse"));

        var locations = await context.CrmServiceLocations.OrderBy(l => l.LocationSequence).ToListAsync();
        locations.Should().HaveCount(2);
        locations[1].Id.Should().Be(added.ServiceLocationId);
        locations[1].TaxExempt.Should().BeTrue();
    }

    [Fact]
    public async Task Create_StoresCustomerAndLocationTagsWithEntityTypes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        var created = await writeService.CreateAsync(SampleCreateDto() with
        {
            TagIds = [8, 3],
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Residential,
                "Main",
                "Main site",
                TagIds: [4, 1])
        });

        created.Customer.TagIds.Should().Equal(8L, 3L);
        created.Customer.CreatedOn.Should().NotBe(default);

        var tags = await context.CrmEntityTags.OrderBy(t => t.EntityTypeId).ThenBy(t => t.DisplayOrder).ToListAsync();
        tags.Should().HaveCount(4);
        tags.Where(t => t.EntityTypeId == (int)CrmTaggedEntityType.Customer)
            .Select(t => (t.EntityId, t.TagId, t.DisplayOrder))
            .Should().Equal(
                (created.Customer.Id, 8L, (short)0),
                (created.Customer.Id, 3L, (short)1));
        tags.Where(t => t.EntityTypeId == (int)CrmTaggedEntityType.ServiceLocation)
            .Select(t => (t.EntityId, t.TagId, t.DisplayOrder))
            .Should().Equal(
                (created.ServiceLocationId!.Value, 4L, (short)0),
                (created.ServiceLocationId!.Value, 1L, (short)1));

        var repository = new CrmCustomerReadRepository(context);
        var detail = await repository.GetByIdAsync(created.Customer.Id);
        var list = await repository.ListAsync(new CrmListQuery(IsActive: true), new CrmCustomerListFilters(), false);

        detail!.TagIds.Should().Equal(8L, 3L);
        list.Items.Single().TagIds.Should().Equal(8L, 3L);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static CrmCustomerWriteService CreateWriteService(FgsCrmDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var auditHelper = new CrmEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsCrmDbContext>(context);
        return new CrmCustomerWriteService(context, unitOfWork, auditHelper);
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

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

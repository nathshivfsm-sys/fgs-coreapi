using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerServiceLocation;
using Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultServiceLocation;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultCustomer;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultServiceLocation;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Crm.Infrastructure.Entities.Customers;
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
    public async Task Create_OmittedPaymentTermPoAndTaxExempt_CopyDefaultCustomerRow()
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
        await context.SaveChangesAsync();

        var writeService = CreateWriteService(context);
        var result = await writeService.CreateAsync(SampleCreateDto() with
        {
            DefaultPaymentTermId = null,
            DefaultPORequired = null,
            TaxExempt = null
        });

        result.Customer.DefaultPaymentTermId.Should().Be(99);
        result.Customer.DefaultPORequired.Should().BeTrue();
        result.Customer.TaxExempt.Should().BeTrue();
    }

    [Fact]
    public async Task GetDefaultCustomer_WhenMissing_ReturnsEmptyShape()
    {
        await using var context = await CreateContextAsync();
        var handler = new GetCrmDefaultCustomerQueryHandler(CreateReadRepository(context));

        var response = await handler.Handle(new GetCrmDefaultCustomerQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().BeEquivalentTo(CrmDefaultCustomerDto.Empty);
    }

    [Fact]
    public async Task UpsertDefaultCustomer_InsertsThenUpdatesCurrentCompanyRow()
    {
        await using var context = await CreateContextAsync();
        var handler = new UpsertCrmDefaultCustomerCommandHandler(CreateWriteService(context));
        var inserted = await handler.Handle(
            new UpsertCrmDefaultCustomerCommand(new CrmDefaultCustomerUpsertDto(99, 11, 12, 13, true, true)),
            CancellationToken.None);

        inserted.Success.Should().BeTrue();
        inserted.StatusCode.Should().Be(200);
        inserted.Data!.Id.Should().BeGreaterThan(0);
        inserted.Data.DefaultPaymentTermId.Should().Be(99);
        inserted.Data.DefaultPORequired.Should().BeTrue();
        inserted.Data.TaxExempt.Should().BeTrue();

        var created = await context.CrmDefaultCustomers.SingleAsync();
        created.TenantId.Should().Be(TenantId);
        created.CompanyId.Should().Be(CompanyId);
        created.CreatedBy.Should().Be("11111111-1111-1111-1111-111111111111");
        var createdOn = created.CreatedOn;

        var updated = await handler.Handle(
            new UpsertCrmDefaultCustomerCommand(new CrmDefaultCustomerUpsertDto(5, null, 77, null, false, false)),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Id.Should().Be(inserted.Data.Id);
        updated.Data.DefaultPaymentTermId.Should().Be(5);
        updated.Data.DefaultMaterialPricingMatrixId.Should().BeNull();
        updated.Data.DefaultLaborPricingMatrixId.Should().Be(77);
        updated.Data.DefaultPORequired.Should().BeFalse();
        updated.Data.TaxExempt.Should().BeFalse();

        var row = await context.CrmDefaultCustomers.SingleAsync();
        row.CreatedOn.Should().Be(createdOn);
        row.UpdatedBy.Should().Be("11111111-1111-1111-1111-111111111111");
        (await context.CrmDefaultCustomers.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetDefaultServiceLocation_WhenMissing_ReturnsEmptyShape()
    {
        await using var context = await CreateContextAsync();
        var handler = new GetCrmDefaultServiceLocationQueryHandler(CreateReadRepository(context));

        var response = await handler.Handle(new GetCrmDefaultServiceLocationQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().BeEquivalentTo(CrmDefaultServiceLocationDto.Empty);
    }

    [Fact]
    public async Task UpsertDefaultServiceLocation_InsertsThenUpdatesCurrentCompanyRow()
    {
        await using var context = await CreateContextAsync();
        var handler = new UpsertCrmDefaultServiceLocationCommandHandler(CreateWriteService(context));
        var inserted = await handler.Handle(
            new UpsertCrmDefaultServiceLocationCommand(
                new CrmDefaultServiceLocationUpsertDto(32, 31, 33, 21, false, 42, 44, 41, 43, false, true)),
            CancellationToken.None);

        inserted.Success.Should().BeTrue();
        inserted.StatusCode.Should().Be(200);
        inserted.Data!.Id.Should().BeGreaterThan(0);
        inserted.Data.EmailAllowed.Should().BeFalse();
        inserted.Data.SmsAllowed.Should().BeFalse();
        inserted.Data.TaxExempt.Should().BeTrue();
        inserted.Data.DefaultPaymentMethodId.Should().Be(21);

        var createdOn = (await context.CrmDefaultServiceLocations.SingleAsync()).CreatedOn;

        var updated = await handler.Handle(
            new UpsertCrmDefaultServiceLocationCommand(
                new CrmDefaultServiceLocationUpsertDto(null, 8, null, null, true, null, null, null, null, true, false)),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Id.Should().Be(inserted.Data.Id);
        updated.Data.DefaultLaborPricingMatrixId.Should().BeNull();
        updated.Data.DefaultMaterialPricingMatrixId.Should().Be(8);
        updated.Data.EmailAllowed.Should().BeTrue();
        updated.Data.SmsAllowed.Should().BeTrue();
        updated.Data.TaxExempt.Should().BeFalse();

        var row = await context.CrmDefaultServiceLocations.SingleAsync();
        row.TenantId.Should().Be(TenantId);
        row.CompanyId.Should().Be(CompanyId);
        row.CreatedOn.Should().Be(createdOn);
        row.UpdatedBy.Should().Be("11111111-1111-1111-1111-111111111111");
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
    public async Task AddServiceLocation_PersistsBreakIdsAndDoesNotCopyThemFromDefaults()
    {
        await using var context = await CreateContextAsync();
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
        var created = await writeService.CreateAsync(SampleCreateDto());
        var added = await writeService.AddServiceLocationAsync(
            created.Customer.Id,
            new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                "Warehouse",
                "Warehouse",
                Break1Id: 501,
                Break2Id: 502));

        var location = await context.CrmServiceLocations.SingleAsync(l => l.Id == added.ServiceLocationId);
        location.Break1Id.Should().Be(501);
        location.Break2Id.Should().Be(502);
        location.DefaultPaymentMethodId.Should().Be(21);

        var omitted = await writeService.AddServiceLocationAsync(
            created.Customer.Id,
            new CrmServiceLocationCreateDto(ServiceLocationType.Industrial, "Yard", "Yard"));
        var omittedLocation = await context.CrmServiceLocations.SingleAsync(l => l.Id == omitted.ServiceLocationId);
        omittedLocation.Break1Id.Should().BeNull();
        omittedLocation.Break2Id.Should().BeNull();
        omittedLocation.DefaultPaymentMethodId.Should().Be(21);

        var detail = await new CrmCustomerReadRepository(context)
            .GetServiceLocationByIdAsync(created.Customer.Id, added.ServiceLocationId);
        detail!.Break1Id.Should().Be(501);
        detail.Break2Id.Should().Be(502);
    }

    [Fact]
    public async Task UpdateServiceLocation_ChangesEditableFieldsAndLeavesNumberAndDefaults()
    {
        await using var context = await CreateContextAsync();
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
        var created = await writeService.CreateAsync(SampleCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                "Warehouse",
                "Warehouse dock",
                AddressLine1: "200 Oak",
                City: "Austin",
                State: "TX",
                Country: "US",
                PostalCode: "78702",
                PrimaryContactName: "Old Contact",
                PrimaryContactEmail: "old@acme.example",
                PrimaryContactPhone: "+1 555-0100",
                CanReceiveEstimates: false,
                CanReceiveInvoices: false,
                CanReceiveAppointments: true,
                TagIds: [4, 1])
        });

        var contactBefore = await context.CrmContacts.SingleAsync(c => c.ServiceLocationId == created.ServiceLocationId);
        var update = new CrmServiceLocationUpdateDto(
            ServiceLocationType.Industrial,
            "Plant",
            "Plant floor",
            "500 Pine",
            null,
            null,
            null,
            null,
            "CA",
            null,
            "US",
            "90001",
            "500 Pine, CA 90001",
            34.05m,
            -118.25m,
            "place-1",
            false,
            "New Contact",
            "new@acme.example",
            null,
            true,
            true,
            false,
            [9, 2],
            CustomerType.Builder,
            false);
        var cache = new Mock<ICacheService>();
        var handler = new UpdateCrmCustomerServiceLocationCommandHandler(
            writeService,
            cache.Object,
            CreateTenantContextAccessor(),
            NullLogger<UpdateCrmCustomerServiceLocationCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateCrmCustomerServiceLocationCommand(
                created.Customer.Id,
                created.ServiceLocationId!.Value,
                update),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        var detail = response.Data!;
        detail.Id.Should().Be(created.ServiceLocationId);
        detail.CustomerId.Should().Be(created.Customer.Id);
        detail.LocationSequence.Should().Be(1);
        detail.LocationNumber.Should().Be("CUST01-1");
        detail.Name.Should().Be("Plant");
        detail.DisplayName.Should().Be("Plant floor");
        detail.AddressLine1.Should().Be("500 Pine");
        detail.City.Should().BeNull();
        detail.State.Should().Be("CA");
        detail.Country.Should().Be("US");
        detail.PostalCode.Should().Be("90001");
        detail.ServiceLocationType.Should().Be(ServiceLocationType.Industrial);
        detail.CustomerType.Should().Be(CustomerType.Builder);
        detail.TaxExempt.Should().BeFalse();
        detail.IsActive.Should().BeFalse();
        detail.Latitude.Should().Be(34.05m);
        detail.Longitude.Should().Be(-118.25m);
        detail.PlaceId.Should().Be("place-1");
        detail.PrimaryContactName.Should().Be("New Contact");
        detail.PrimaryContactEmail.Should().Be("new@acme.example");
        detail.PrimaryContactPhone.Should().BeNull();
        detail.CanReceiveEstimates.Should().BeTrue();
        detail.CanReceiveInvoices.Should().BeTrue();
        detail.CanReceiveAppointments.Should().BeFalse();
        detail.TagIds.Should().Equal(9L, 2L);

        var location = await context.CrmServiceLocations.SingleAsync();
        location.LocationNumber.Should().Be("CUST01-1");
        location.LocationSequence.Should().Be(1);
        location.CustomerId.Should().Be(created.Customer.Id);
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

        var contact = await context.CrmContacts.SingleAsync(c => c.ServiceLocationId == location.Id);
        contact.Id.Should().Be(contactBefore.Id);
        var phone = await context.CrmContactCommunications.SingleAsync(
            c => c.ContactId == contact.Id && c.CommunicationTypeId == (short)ContactCommunicationType.Phone);
        phone.IsPrimary.Should().BeFalse();
        phone.IsActive.Should().BeFalse();

        var repository = new CrmCustomerReadRepository(context);
        var listed = await repository.ListServiceLocationsAsync(
            created.Customer.Id,
            new CrmListQuery(IsActive: false));
        listed!.Items.Should().ContainSingle();
        listed.Items.Single().Email.Should().Be("new@acme.example");
        listed.Items.Single().Phone.Should().BeNull();

        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "customer"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        var cleared = await writeService.UpdateServiceLocationAsync(
            created.Customer.Id,
            location.Id,
            update with { TagIds = null });
        cleared.TagIds.Should().BeEmpty();
        cleared.LocationNumber.Should().Be("CUST01-1");
        cleared.PrimaryContactEmail.Should().Be("new@acme.example");
    }

    [Fact]
    public async Task UpdateServiceLocation_ReplacesBreakIdsAndNullClearsThem()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var created = await writeService.CreateAsync(SampleCreateDto());
        var added = await writeService.AddServiceLocationAsync(
            created.Customer.Id,
            new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                "Warehouse",
                "Warehouse",
                AddressLine1: "200 Oak",
                State: "TX",
                Country: "US",
                PostalCode: "78702",
                Break1Id: 11,
                Break2Id: 22));

        var replaced = await writeService.UpdateServiceLocationAsync(
            created.Customer.Id,
            added.ServiceLocationId,
            BreakLocationUpdate(33, 44));
        replaced.Break1Id.Should().Be(33);
        replaced.Break2Id.Should().Be(44);

        var cleared = await writeService.UpdateServiceLocationAsync(
            created.Customer.Id,
            added.ServiceLocationId,
            BreakLocationUpdate(null, null));
        cleared.Break1Id.Should().BeNull();
        cleared.Break2Id.Should().BeNull();

        var location = await context.CrmServiceLocations.SingleAsync();
        location.Break1Id.Should().BeNull();
        location.Break2Id.Should().BeNull();
    }

    [Fact]
    public async Task UpdateServiceLocation_UnknownCustomerOrForeignLocation_IsNotFound()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var first = await writeService.CreateAsync(SampleCreateDto() with
        {
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Residential,
                "Main",
                "Main site")
        });
        var second = await writeService.CreateAsync(SampleCreateDto() with
        {
            CustomerNumber = "CUST02",
            CreationOption = CrmCustomerCreationOption.BillToAndServiceLocation,
            ServiceLocation = new CrmServiceLocationCreateDto(
                ServiceLocationType.Commercial,
                "Other",
                "Other site")
        });

        var update = new CrmServiceLocationUpdateDto(
            ServiceLocationType.Industrial,
            "Plant",
            "Plant floor",
            IsActive: true);

        var unknownCustomer = () => writeService.UpdateServiceLocationAsync(
            999,
            first.ServiceLocationId!.Value,
            update);
        var foreignLocation = () => writeService.UpdateServiceLocationAsync(
            first.Customer.Id,
            second.ServiceLocationId!.Value,
            update);

        await unknownCustomer.Should().ThrowAsync<KeyNotFoundException>();
        await foreignLocation.Should().ThrowAsync<KeyNotFoundException>();
        (await context.CrmServiceLocations.SingleAsync(l => l.Id == first.ServiceLocationId)).Name.Should().Be("Main");
        (await context.CrmServiceLocations.SingleAsync(l => l.Id == second.ServiceLocationId)).Name.Should().Be("Other");
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
        return new CrmCustomerWriteService(
            context,
            unitOfWork,
            auditHelper,
            new CrmCustomerReadRepository(context));
    }

    private static CrmCustomerReadRepository CreateReadRepository(FgsCrmDbContext context) =>
        new(context, CreateTenantContextAccessor());

    private static CrmServiceLocationUpdateDto BreakLocationUpdate(long? break1Id, long? break2Id) =>
        new(
            ServiceLocationType.Commercial,
            "Warehouse",
            "Warehouse",
            AddressLine1: "200 Oak",
            State: "TX",
            Country: "US",
            PostalCode: "78702",
            IsActive: true,
            Break1Id: break1Id,
            Break2Id: break2Id);

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

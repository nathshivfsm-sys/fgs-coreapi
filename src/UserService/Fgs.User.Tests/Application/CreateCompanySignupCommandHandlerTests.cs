using System.Text.Json;
using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Persistence.Database.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;

namespace Fgs.User.Tests.Application;

public sealed class CreateCompanySignupCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithDuplicateCompanyName_AllowsSecondSignup()
    {
        var (handler, context) = await CreateHandlerAsync();
        var companyName = $"Dup Co {Guid.NewGuid():N}"[..20];
        var command = ValidCommand(companyName: companyName);
        var second = ValidCommand(companyName: companyName);

        (await handler.Handle(command, CancellationToken.None)).Success.Should().BeTrue();
        (await handler.Handle(second, CancellationToken.None)).Success.Should().BeTrue();
        (await context.FgsTenants.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ReturnsConflict()
    {
        var (handler, _) = await CreateHandlerAsync();
        var email = $"dup-{Guid.NewGuid():N}@test.com";
        var first = ValidCommand(companyName: $"First {Guid.NewGuid():N}"[..20]);
        var second = ValidCommand(companyName: $"Second {Guid.NewGuid():N}"[..20]) with
        {
            Contact = first.Contact with { Email = email }
        };
        first = first with { Contact = first.Contact with { Email = email } };

        (await handler.Handle(first, CancellationToken.None)).Success.Should().BeTrue();

        var response = await handler.Handle(second, CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Conflict);
        response.Errors.Should().Contain(e => e.Contains("email", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesTenantCompanyUserInvitationLocationAndOutbox()
    {
        var (handler, context) = await CreateHandlerAsync();
        var companyName = $"Acme {Guid.NewGuid():N}"[..16];
        var command = ValidCommand(companyName: companyName);
        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().NotBeNull();

        (await context.FgsTenants.CountAsync()).Should().Be(1);
        (await context.FgsTenantCompanies.CountAsync()).Should().Be(1);
        (await context.FgsUsers.CountAsync()).Should().Be(1);
        (await context.FgsInvitations.CountAsync()).Should().Be(1);
        (await context.FgsLocations.CountAsync()).Should().Be(1);
        (await context.FgsOutboxMessages.CountAsync()).Should().Be(1);

        var tenant = await context.FgsTenants.SingleAsync();
        tenant.Name.Should().Be(companyName);
        tenant.LegalName.Should().Be(companyName);
        tenant.PhoneNumber.Should().Be("15550199");
        tenant.Website.Should().Be("https://example.com");
        tenant.TimeZone.Should().Be("America/Chicago");
        tenant.DefaultCurrency.Should().Be("USD");
        tenant.DefaultLanguageId.Should().Be(SignupConstants.DefaultLanguageId);
        tenant.PhysicalLocationId.Should().NotBeNull();
        tenant.BillingLocationId.Should().Be(tenant.PhysicalLocationId);
        tenant.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var company = await context.FgsTenantCompanies.SingleAsync();
        company.Name.Should().Be(companyName);
        company.LegalName.Should().Be(companyName);
        company.PhoneNumber.Should().Be("15550199");
        company.CompanySize.Should().Be("1-2");
        company.BusinessTypeId.Should().Be(1);
        company.Website.Should().Be("https://example.com");
        company.PhysicalLocationId.Should().Be(tenant.PhysicalLocationId);
        company.BillingLocationId.Should().Be(tenant.PhysicalLocationId);
        company.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var location = await context.FgsLocations.SingleAsync();
        location.AddressLine1.Should().Be(command.Company.Address.AddressLine1);
        location.City.Should().Be(command.Company.Address.City);
        location.State.Should().Be(command.Company.Address.State);
        location.PostalCode.Should().Be(command.Company.Address.PostalCode);
        location.FormattedAddress.Should().Be("100 Test Ave, Austin, TX 78701, US");
        location.MasterEntityTypeId.Should().Be(2);
        location.CompanyId.Should().Be(1);
        location.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var user = await context.FgsUsers.SingleAsync();
        user.Email.Should().Be(command.Contact.Email.Trim());
        user.DisplayName.Should().Be(command.Contact.Name);
        user.Role.ToString().Should().Be("Admin");
        user.CompanyId.Should().Be(1);
        user.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var outbox = await context.FgsOutboxMessages.SingleAsync();
        var evt = JsonSerializer.Deserialize<CompanySignupInviteEmailEvent>(outbox.Payload);
        evt.Should().NotBeNull();
        evt!.Email.Should().Be(user.Email);
        evt.EmailTemplateCode.Should().Be(CommunicationTemplateCodes.CompanyAdminInvitation);
        evt.Name.Should().Be(command.Contact.Name);
        evt.InviteLink.Should().Contain("token=");
        evt.ExpirationHours.Should().Be("168");
    }

    [Fact]
    public async Task Handle_WithInvalidBusinessType_ReturnsBadRequest()
    {
        var (handler, _) = await CreateHandlerAsync();
        var command = ValidCommand() with { BusinessTypeId = 9999 };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
    }

    private static CreateCompanySignupCommand ValidCommand(string? companyName = null) =>
        new(
            Contact: new SignupContactDto(
                Name: "Admin User",
                PhoneNumber: "+1 555-0199",
                Email: $"admin-{Guid.NewGuid():N}@test.com"),
            Company: new SignupCompanyDto(
                Name: companyName ?? $"Test Tenant {Guid.NewGuid():N}"[..24],
                Website: "https://example.com",
                Address: new SignupAddressDto(
                    AddressLine1: "100 Test Ave",
                    AddressLine2: null,
                    City: "Austin",
                    State: "TX",
                    PostalCode: "78701",
                    Country: "US"),
                CompanySize: "1-2"),
            BusinessTypeId: 1);

    private static async Task<(CreateCompanySignupCommandHandler Handler, FgsUserDbContext Context)> CreateHandlerAsync()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        context.GloBusinessTypes.Add(new GloBusinessType
        {
            Id = 1,
            Code = "HVAC",
            Name = "HVAC",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.GloCountries.Add(new GloCountry
        {
            CountryCode = "US",
            CountryName = "United States",
            CurrencyCode = "USD",
            IsActive = true
        });
        await context.SaveChangesAsync();

        IUnitOfWork unitOfWork = new UnitOfWork(context);
        IDateTimeProvider dateTime = new DateTimeProvider();
        IOutboxWriter outboxWriter = new OutboxWriter(context, dateTime);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Invitation:ExpiryDays"] = "7",
                ["Invitation:InviteBaseUrl"] = "https://localhost/api/invite/start"
            })
            .Build();

        IAddressLocaleResolver localeResolver = new AddressLocaleResolver(
            unitOfWork.Repository<GloCountry>(),
            Options.Create(new SignupLocaleOptions
            {
                DefaultTimeZone = "UTC",
                DefaultCurrency = "USD"
            }));

        var signupUniquenessValidator = new SignupUniquenessValidator(
            unitOfWork,
            new EmailNormalizer(),
            dateTime);

        var handler = new CreateCompanySignupCommandHandler(
            unitOfWork,
            new InvitationTokenService(),
            outboxWriter,
            dateTime,
            configuration,
            localeResolver,
            signupUniquenessValidator);

        return (handler, context);
    }
}

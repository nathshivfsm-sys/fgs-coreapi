using Fgs.User.Infrastructure.Common.Security;
using Fgs.Messaging.Options;
using Fgs.User.Infrastructure.Common.Options;
using System.Linq.Expressions;
using System.Text.Json;
using Fgs.User.Application.Abstractions.Geo;
using Fgs.Messaging.Abstractions;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.Foundation.Result;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Signup;
using Fgs.Contracts.IntegrationEvents;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.Security.Options;
using Fgs.Security.Constants;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database;
using SetupOutboxWriter = Fgs.Setup.Infrastructure.Messaging.OutboxWriter;
using Fgs.Setup.Infrastructure.Common.Time;
using SetupDateTimeProvider = Fgs.Setup.Infrastructure.Common.Time.DateTimeProvider;
using Fgs.User.Infrastructure.Database.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Fgs.User.Infrastructure.Database;

namespace Fgs.User.Tests.Application;

public sealed class CreateCompanySignupCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithDuplicateCompanyName_AllowsSecondSignup()
    {
        var (handler, user, _) = await CreateHandlerAsync();
        var companyName = $"Dup Co {Guid.NewGuid():N}"[..20];
        var command = ValidCommand(companyName: companyName);
        var second = ValidCommand(companyName: companyName);

        (await handler.Handle(command, CancellationToken.None)).Success.Should().BeTrue();
        (await handler.Handle(second, CancellationToken.None)).Success.Should().BeTrue();
        (await user.FgsTenants.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ReturnsConflict()
    {
        var (handler, _, _) = await CreateHandlerAsync();
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
        var (handler, userContext, setupContext) = await CreateHandlerAsync();
        var companyName = $"Acme {Guid.NewGuid():N}"[..16];
        var command = ValidCommand(companyName: companyName);
        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().NotBeNull();

        (await userContext.FgsTenants.CountAsync()).Should().Be(1);
        (await userContext.FgsTenantCompanies.CountAsync()).Should().Be(1);
        (await userContext.FgsUsers.CountAsync()).Should().Be(1);
        (await userContext.FgsInvitations.CountAsync()).Should().Be(1);
        (await userContext.FgsLocations.CountAsync()).Should().Be(1);
        (await setupContext.GloOutboxMessages.CountAsync()).Should().Be(1);

        var tenant = await userContext.FgsTenants.SingleAsync();
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

        var company = await userContext.FgsTenantCompanies.SingleAsync();
        company.Name.Should().Be(companyName);
        company.LegalName.Should().Be(companyName);
        company.PhoneNumber.Should().Be("15550199");
        company.CompanySize.Should().Be("1-2");
        company.BusinessTypeId.Should().Be(1);
        company.Website.Should().Be("https://example.com");
        company.PhysicalLocationId.Should().Be(tenant.PhysicalLocationId);
        company.BillingLocationId.Should().Be(tenant.PhysicalLocationId);
        company.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var location = await userContext.FgsLocations.SingleAsync();
        location.AddressLine1.Should().Be(command.Company.Address.AddressLine1);
        location.City.Should().Be(command.Company.Address.City);
        location.State.Should().Be(command.Company.Address.State);
        location.PostalCode.Should().Be(command.Company.Address.PostalCode);
        location.FormattedAddress.Should().Be("100 Test Ave, Austin, TX 78701, US");
        location.MasterEntityTypeId.Should().Be(SignupConstants.TenantCompanyMasterEntityTypeId);
        location.CompanyId.Should().Be(1);
        location.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var createdUser = await userContext.FgsUsers.SingleAsync();
        createdUser.Email.Should().Be(command.Contact.Email.Trim());
        createdUser.DisplayName.Should().Be(command.Contact.Name);
        createdUser.CompanyId.Should().Be(1);
        createdUser.CreatedBy.Should().Be(SignupConstants.ProspectActor);

        var userRole = await userContext.FgsUserRoles.SingleAsync();
        userRole.UserId.Should().Be(createdUser.Id);
        userRole.GloRoleId.Should().Be(1);
        userRole.FgsRoleId.Should().BeNull();

        var outbox = await setupContext.GloOutboxMessages.SingleAsync();
        outbox.ExchangeName.Should().Be(IntegrationEventExchanges.UserEvents);
        outbox.RoutingKey.Should().Be(IntegrationEventRoutingKeys.CompanySignupInviteEmail);
        outbox.CreatedBy.Should().Be(SignupConstants.ProspectActorUserId.ToString());
        var evt = JsonSerializer.Deserialize<CompanySignupInviteEmailEvent>(outbox.Payload);
        evt.Should().NotBeNull();
        evt!.Email.Should().Be(createdUser.Email);
        evt.CompanyId.Should().Be(1);
        evt.TenantId.Should().Be(tenant.Id);
        evt.EmailTemplateCode.Should().Be(CommunicationTemplateCodes.CompanyAdminInvitation);
        evt.Name.Should().Be(command.Contact.Name);
        evt.InviteLink.Should().Contain("token=");
        evt.ExpirationHours.Should().Be("168");
    }

    [Fact]
    public async Task Handle_WithMultipleBusinessTypes_PersistsAllSelections()
    {
        var (handler, userContext, setupContext) = await CreateHandlerAsync();
        setupContext.GloBusinessTypes.Add(new GloBusinessType
        {
            Id = 2,
            Code = "PLUMBING",
            Name = "Plumbing",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await setupContext.SaveChangesAsync();

        var command = ValidCommand() with { BusinessTypeIds = [1, 2] };
        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        (await setupContext.FgsBusinessTypes.CountAsync()).Should().Be(2);
        var company = await userContext.FgsTenantCompanies.SingleAsync();
        company.BusinessTypeId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithInvalidBusinessType_ReturnsBadRequest()
    {
        var (handler, _, _) = await CreateHandlerAsync();
        var command = ValidCommand() with { BusinessTypeIds = [9999] };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        response.Errors.Should().Contain(SignupErrorMessages.InvalidBusinessType);
    }

    [Fact]
    public async Task Handle_WhenUniqueTenantCodeCannotBeResolved_ReturnsConflict()
    {
        var tenantRepoMock = new Mock<IRepository<FgsTenant>>();
        tenantRepoMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FgsTenant, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var businessTypeRepoMock = new Mock<IRepository<GloBusinessType>>();
        businessTypeRepoMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<GloBusinessType, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new GloBusinessType { Id = 1, Code = "HVAC", Name = "HVAC", IsActive = true }]);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Repository<FgsTenant>()).Returns(tenantRepoMock.Object);

        var signupUniquenessValidatorMock = new Mock<ISignupUniquenessValidator>();
        signupUniquenessValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateCompanySignupCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var setupUnitOfWorkMock = new Mock<ISetupUnitOfWork>();
        setupUnitOfWorkMock.Setup(u => u.Repository<GloBusinessType>()).Returns(businessTypeRepoMock.Object);

        var handler = new CreateCompanySignupCommandHandler(
            unitOfWorkMock.Object,
            setupUnitOfWorkMock.Object,
            new InvitationTokenService(),
            Mock.Of<IOutboxWriter>(),
            new Fgs.User.Infrastructure.Common.Time.DateTimeProvider(),
            new ConfigurationBuilder().Build(),
            Mock.Of<IAddressLocaleResolver>(),
            signupUniquenessValidatorMock.Object);

        var response = await handler.Handle(ValidCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Conflict);
        response.Errors.Should().Contain(SignupErrorMessages.UniqueTenantCodeFailed);
    }

    [Fact]
    public async Task Handle_WhenTenantAdminRoleMissing_Throws()
    {
        var userContext = await TestDbContextFactory.CreateAndInitializeAsync();
        var setupContext = await TestSetupDbContextFactory.CreateAndInitializeAsync();
        setupContext.GloBusinessTypes.Add(new GloBusinessType
        {
            Id = 1,
            Code = "HVAC",
            Name = "HVAC",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await setupContext.SaveChangesAsync();

        var handler = CreateHandlerFromContexts(userContext, setupContext);

        var act = async () => await handler.Handle(ValidCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(SignupErrorMessages.TenantAdminRoleNotFound);
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
            BusinessTypeIds: [1]);

    private static async Task<(CreateCompanySignupCommandHandler Handler, FgsUserDbContext User, FgsSetupDbContext Setup)> CreateHandlerAsync()
    {
        var userContext = await TestDbContextFactory.CreateAndInitializeAsync();
        var setupContext = await TestSetupDbContextFactory.CreateAndInitializeAsync();
        setupContext.GloBusinessTypes.Add(new GloBusinessType
        {
            Id = 1,
            Code = "HVAC",
            Name = "HVAC",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        setupContext.GloRoles.Add(new GloRole
        {
            Id = 1,
            RoleCode = SignupConstants.TenantAdminRoleCode,
            Name = "Tenant Administrator",
            RoleLevel = "TENANT",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await setupContext.SaveChangesAsync();

        return (CreateHandlerFromContexts(userContext, setupContext), userContext, setupContext);
    }

    private static CreateCompanySignupCommandHandler CreateHandlerFromContexts(
        FgsUserDbContext userContext,
        FgsSetupDbContext setupContext)
    {
        IUnitOfWork unitOfWork = new UnitOfWork(userContext);
        ISetupUnitOfWork setupUnitOfWork = new SetupUnitOfWork(setupContext);
        IDateTimeProvider dateTime = new Fgs.User.Infrastructure.Common.Time.DateTimeProvider();
        IOutboxWriter outboxWriter = new SetupOutboxWriter(
            setupContext,
            new SetupDateTimeProvider(),
            Microsoft.Extensions.Options.Options.Create(new OutboxOptions()));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.Invitation.ExpiryDays] = "7",
                [ConfigurationKeys.Invitation.InviteBaseUrl] = "https://localhost/api/v1/invite/start"
            })
            .Build();

        IAddressLocaleResolver localeResolver = new AddressLocaleResolver(
            Microsoft.Extensions.Options.Options.Create(new SignupLocaleOptions
            {
                DefaultTimeZone = "UTC",
                DefaultCurrency = "USD"
            }));

        var signupUniquenessValidator = new SignupUniquenessValidator(
            unitOfWork,
            new EmailNormalizer(),
            dateTime);

        return new CreateCompanySignupCommandHandler(
            unitOfWork,
            setupUnitOfWork,
            new InvitationTokenService(),
            outboxWriter,
            dateTime,
            configuration,
            localeResolver,
            signupUniquenessValidator);
    }
}

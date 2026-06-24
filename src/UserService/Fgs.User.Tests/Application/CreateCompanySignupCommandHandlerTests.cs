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
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Signup;
using Fgs.Contracts.IntegrationEvents;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.Persistence.Implementations;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

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
        var (handler, userContext, setupClientMock) = await CreateHandlerAsync();
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
        (await userContext.TenantOutboxMessages.CountAsync()).Should().Be(1);

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
        userRole.GloRoleId.Should().Be(SignupConstants.TenantAdminGloRoleId);
        userRole.FgsRoleId.Should().BeNull();

        var outbox = await userContext.TenantOutboxMessages.SingleAsync();
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

        setupClientMock.Verify(
            c => c.AddCompanyBusinessTypesAsync(
                tenant.Id,
                1L,
                It.Is<AddCompanyBusinessTypesRequest>(r =>
                    r.BusinessTypeIds.SequenceEqual(new[] { 1 })
                    && r.CompanyGuid == company.CompanyGuid
                    && r.Code == tenant.TenantCode
                    && r.Name == companyName),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleBusinessTypes_PersistsPrimaryAndCallsSetupClient()
    {
        var (handler, userContext, setupClientMock) = await CreateHandlerAsync();
        var command = ValidCommand() with { BusinessTypeIds = [1, 2] };
        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        var company = await userContext.FgsTenantCompanies.SingleAsync();
        company.BusinessTypeId.Should().Be(1);

        setupClientMock.Verify(
            c => c.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.Is<AddCompanyBusinessTypesRequest>(r => r.BusinessTypeIds.SequenceEqual(new[] { 1, 2 })),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSetupClientFails_DoesNotEnqueueOutboxOrCommitSignup()
    {
        var userContext = await TestDbContextFactory.CreateAndInitializeAsync();
        var setupClientMock = new Mock<ISetupClient>();
        setupClientMock
            .Setup(c => c.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<AddCompanyBusinessTypesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["Setup unavailable"], ApiStatusCodes.BadRequest));

        var handler = CreateHandlerFromContext(userContext, setupClientMock.Object);

        var act = () => handler.Handle(ValidCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        (await userContext.TenantOutboxMessages.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenUniqueTenantCodeCannotBeResolved_ReturnsConflict()
    {
        var tenantRepoMock = new Mock<IRepository<FgsTenant>>();
        tenantRepoMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FgsTenant, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Repository<FgsTenant>()).Returns(tenantRepoMock.Object);

        var signupUniquenessValidatorMock = new Mock<ISignupUniquenessValidator>();
        signupUniquenessValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateCompanySignupCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var handler = new CreateCompanySignupCommandHandler(
            unitOfWorkMock.Object,
            Mock.Of<ISetupClient>(),
            new InvitationTokenService(),
            Mock.Of<IOutboxWriter>(),
            new DateTimeProvider(),
            new ConfigurationBuilder().Build(),
            Mock.Of<IAddressLocaleResolver>(),
            signupUniquenessValidatorMock.Object,
            Mock.Of<ICacheService>());

        var response = await handler.Handle(ValidCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Conflict);
        response.Errors.Should().Contain(SignupErrorMessages.UniqueTenantCodeFailed);
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

    private static async Task<(CreateCompanySignupCommandHandler Handler, FgsUserDbContext User, Mock<ISetupClient> SetupClient)> CreateHandlerAsync()
    {
        var userContext = await TestDbContextFactory.CreateAndInitializeAsync();
        var setupClientMock = new Mock<ISetupClient>();
        setupClientMock
            .Setup(c => c.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<AddCompanyBusinessTypesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));

        return (CreateHandlerFromContext(userContext, setupClientMock.Object), userContext, setupClientMock);
    }

    private static CreateCompanySignupCommandHandler CreateHandlerFromContext(
        FgsUserDbContext userContext,
        ISetupClient setupClient)
    {
        IUnitOfWork unitOfWork = new EfUnitOfWork<FgsUserDbContext>(userContext);
        IDateTimeProvider dateTime = new DateTimeProvider();
        IOutboxWriter outboxWriter = new OutboxWriter(
            userContext,
            dateTime,
            Options.Create(new OutboxOptions()));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.Invitation.ExpiryDays] = "7",
                [ConfigurationKeys.Invitation.InviteBaseUrl] = "https://localhost/api/v1/invite/start"
            })
            .Build();

        IAddressLocaleResolver localeResolver = new AddressLocaleResolver(
            Options.Create(new SignupLocaleOptions
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
            setupClient,
            new InvitationTokenService(),
            outboxWriter,
            dateTime,
            configuration,
            localeResolver,
            signupUniquenessValidator,
            Mock.Of<ICacheService>());
    }
}

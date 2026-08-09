using Fgs.Bff.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Signup;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Bff.Tests.Application;

public sealed class CreateCompanySignupCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserSucceedsAndSetupSucceeds_ReturnsCreated()
    {
        var identity = new CompanySignupResultDto(
            TenantId: 42,
            CompanyNumber: 1,
            CompanyGuid: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            InvitationId: Guid.NewGuid(),
            InviteUrl: "https://example.com/invite?token=abc",
            TenantCode: "ACME");

        var userClient = new Mock<IUserSignupClient>();
        userClient
            .Setup(c => c.CreateCompanySignupAsync(It.IsAny<CompanySignupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanySignupResultDto>.Ok(identity, ApiStatusCodes.Created));

        var tenantClient = new Mock<IUserTenantClient>();
        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.AddCompanyBusinessTypesAsync(
                42,
                1,
                It.Is<AddCompanyBusinessTypesRequest>(r =>
                    r.BusinessTypeIds.SequenceEqual(new[] { 1, 2 })
                    && r.CompanyGuid == identity.CompanyGuid
                    && r.Code == "ACME"
                    && r.Name == "Acme Co"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));

        var handler = new CreateCompanySignupCommandHandler(
            userClient.Object,
            tenantClient.Object,
            setupClient.Object,
            NullLogger<CreateCompanySignupCommandHandler>.Instance);

        var response = await handler.Handle(ValidCommand(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().BeEquivalentTo(identity);
        userClient.Verify(
            c => c.CreateCompanySignupAsync(
                It.Is<CompanySignupRequest>(r =>
                    r.AuthenticationMethod == AuthenticationMethod.Password
                    && r.BusinessTypeIds.SequenceEqual(new[] { 1, 2 })),
                It.IsAny<CancellationToken>()),
            Times.Once);
        tenantClient.Verify(
            c => c.UpdateStatusAsync(It.IsAny<long>(), It.IsAny<UpdateTenantStatusRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserFails_DoesNotCallSetup()
    {
        var userClient = new Mock<IUserSignupClient>();
        userClient
            .Setup(c => c.CreateCompanySignupAsync(It.IsAny<CompanySignupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanySignupResultDto>.Fail(
                ["email already used"],
                ApiStatusCodes.Conflict));

        var tenantClient = new Mock<IUserTenantClient>();
        var setupClient = new Mock<ISetupClient>();
        var handler = new CreateCompanySignupCommandHandler(
            userClient.Object,
            tenantClient.Object,
            setupClient.Object,
            NullLogger<CreateCompanySignupCommandHandler>.Instance);

        var response = await handler.Handle(ValidCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Conflict);
        setupClient.Verify(
            c => c.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<AddCompanyBusinessTypesRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSetupFails_MarksTenantProvisioningFailed()
    {
        var identity = new CompanySignupResultDto(
            7, 1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "https://x/invite", "TENANT");

        var userClient = new Mock<IUserSignupClient>();
        userClient
            .Setup(c => c.CreateCompanySignupAsync(It.IsAny<CompanySignupRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<CompanySignupResultDto>.Ok(identity, ApiStatusCodes.Created));

        var tenantClient = new Mock<IUserTenantClient>();
        tenantClient
            .Setup(c => c.UpdateStatusAsync(
                7,
                It.Is<UpdateTenantStatusRequest>(r => r.FgsTenantStatusId == TenantStatusIds.ProvisioningFailed),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));

        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.AddCompanyBusinessTypesAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<AddCompanyBusinessTypesRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["Setup unavailable"], ApiStatusCodes.BadRequest));

        var handler = new CreateCompanySignupCommandHandler(
            userClient.Object,
            tenantClient.Object,
            setupClient.Object,
            NullLogger<CreateCompanySignupCommandHandler>.Instance);

        var response = await handler.Handle(ValidCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        response.Errors.Should().Contain(e => e.Contains("tenantId=7", StringComparison.Ordinal));
        tenantClient.Verify(
            c => c.UpdateStatusAsync(
                7,
                It.Is<UpdateTenantStatusRequest>(r => r.FgsTenantStatusId == TenantStatusIds.ProvisioningFailed),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static CreateCompanySignupCommand ValidCommand() =>
        new(
            Contact: new SignupContactDto("Admin", "+15551234567", "admin@example.com"),
            Company: new SignupCompanyDto(
                "Acme Co",
                "https://acme.example.com",
                new SignupAddressDto("1 Main", null, "Austin", "TX", "78701", Country: "US"),
                "11-50"),
            BusinessTypeIds: [1, 2],
            AuthenticationMethod: AuthenticationMethod.Password);
}

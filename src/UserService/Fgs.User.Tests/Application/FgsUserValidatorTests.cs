using Fgs.Security.Constants;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.Security;
using Fgs.Foundation.Time;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Application.Features.Users.Commands.InviteFgsUser;
using Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;
using Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Application.Features.Users.Validators;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class FgsUserValidatorTests
{
    [Fact]
    public async Task InviteValidator_WhenEmailMissing_HasValidationError()
    {
        var validator = CreateInviteValidator();
        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "", null, [1L])]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Email"));
    }

    [Fact]
    public async Task InviteValidator_WhenRoleMissing_HasValidationError()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var roleRead = CreateRoleReadMock();
        roleRead.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleDetailDto?)null);

        var validator = new InviteFgsUserCommandValidator(
            userRead.Object,
            roleRead.Object,
            CreateInvitationRead().Object,
            CreateEmailNormalizer().Object,
            CreateDateTime().Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "jane@example.com", null, [99L])]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("role", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task InviteValidator_WhenEmailExistsForTenantCompany_HasValidationError()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync("jane@example.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new InviteFgsUserCommandValidator(
            userRead.Object,
            CreateRoleReadMock().Object,
            CreateInvitationRead().Object,
            CreateEmailNormalizer().Object,
            CreateDateTime().Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "jane@example.com", null, [1L])]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "A user with this email already exists for this tenant and company.");
    }

    [Fact]
    public async Task InviteValidator_WhenTenantAdminAlreadyExists_HasValidationError()
    {
        var roleRead = CreateRoleReadMock();
        roleRead.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(5, FgsRoleCodes.TenantAdmin, "Tenant Admin", null, null, true, 1, true));
        roleRead.Setup(r => r.HasOtherActiveUserWithRoleCodeAsync(
                FgsRoleCodes.TenantAdmin, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new InviteFgsUserCommandValidator(
            CreateUserRead().Object,
            roleRead.Object,
            CreateInvitationRead().Object,
            CreateEmailNormalizer().Object,
            CreateDateTime().Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "jane@example.com", null, [5L])]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Only one tenant admin is allowed per company.");
    }

    [Fact]
    public async Task InviteValidator_WhenMultipleTenantAdminInvites_HasValidationError()
    {
        var roleRead = CreateRoleReadMock();
        roleRead.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(5, FgsRoleCodes.TenantAdmin, "Tenant Admin", null, null, true, 1, true));

        var validator = new InviteFgsUserCommandValidator(
            CreateUserRead().Object,
            roleRead.Object,
            CreateInvitationRead().Object,
            CreateEmailNormalizer().Object,
            CreateDateTime().Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand(
            [
                new FgsUserInviteDto("Jane", "jane@example.com", null, [5L]),
                new FgsUserInviteDto("John", "john@example.com", null, [5L])
            ]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Only one tenant admin invite is allowed in a single request.");
    }

    [Fact]
    public async Task UpdateValidator_WhenAssigningTenantAdminAndAnotherExists_HasValidationError()
    {
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var roleRead = CreateRoleReadMock();
        roleRead.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(5, FgsRoleCodes.TenantAdmin, "Tenant Admin", null, null, true, 1, true));
        roleRead.Setup(r => r.HasOtherActiveUserWithRoleCodeAsync(
                FgsRoleCodes.TenantAdmin, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateFgsUserCommandValidator(roleRead.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsUserCommand(userId, new FgsUserUpdateDto("Jane", null, [5L], true)));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Only one tenant admin is allowed per company.");
    }

    private static InviteFgsUserCommandValidator CreateInviteValidator() =>
        new(
            CreateUserRead().Object,
            CreateRoleReadMock().Object,
            CreateInvitationRead().Object,
            CreateEmailNormalizer().Object,
            CreateDateTime().Object);

    private static Mock<IFgsUserReadRepository> CreateUserRead()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return userRead;
    }

    [Fact]
    public async Task ResendInviteValidator_WhenUserAlreadyAccepted_Fails()
    {
        var userId = Guid.NewGuid();
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.HasAcceptedInvitationAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new ResendFgsUserInviteCommandValidator(userRead.Object);
        var result = await validator.ValidateAsync(new ResendFgsUserInviteCommand(userId));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("already accepted", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ResendInviteValidator_WhenUserPending_Succeeds()
    {
        var userId = Guid.NewGuid();
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.HasAcceptedInvitationAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new ResendFgsUserInviteCommandValidator(userRead.Object);
        var result = await validator.ValidateAsync(new ResendFgsUserInviteCommand(userId));

        result.IsValid.Should().BeTrue();
    }

    private static Mock<IFgsRoleReadRepository> CreateRoleReadMock()
    {
        var roleRead = new Mock<IFgsRoleReadRepository>();
        roleRead.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(1, "TECH", "Technician", null, null, false, 1, true));
        roleRead.Setup(r => r.HasOtherActiveUserWithRoleCodeAsync(
                It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return roleRead;
    }

    private static Mock<IInvitationReadQuery> CreateInvitationRead()
    {
        var invitationRead = new Mock<IInvitationReadQuery>();
        invitationRead.Setup(r => r.HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
                It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return invitationRead;
    }

    private static Mock<IEmailNormalizer> CreateEmailNormalizer()
    {
        var emailNormalizer = new Mock<IEmailNormalizer>();
        emailNormalizer.Setup(e => e.Normalize(It.IsAny<string>())).Returns<string>(s => s.Trim().ToLowerInvariant());
        return emailNormalizer;
    }

    private static Mock<IDateTimeProvider> CreateDateTime()
    {
        var dateTime = new Mock<IDateTimeProvider>();
        dateTime.SetupGet(d => d.UtcNow).Returns(DateTimeOffset.UtcNow);
        return dateTime;
    }
}

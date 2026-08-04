using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Application.Features.Users.Commands.InviteFgsUser;
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
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "", null, 1)]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Email"));
    }

    [Fact]
    public async Task InviteValidator_WhenRoleMissing_HasValidationError()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var roleRead = new Mock<IFgsRoleReadRepository>();
        roleRead.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleDetailDto?)null);

        var invitationRead = new Mock<IInvitationReadQuery>();
        invitationRead.Setup(r => r.HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
                It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var emailNormalizer = new Mock<IEmailNormalizer>();
        emailNormalizer.Setup(e => e.Normalize(It.IsAny<string>())).Returns<string>(s => s.Trim().ToLowerInvariant());

        var dateTime = new Mock<IDateTimeProvider>();
        dateTime.SetupGet(d => d.UtcNow).Returns(DateTimeOffset.UtcNow);

        var validator = new InviteFgsUserCommandValidator(
            userRead.Object,
            roleRead.Object,
            invitationRead.Object,
            emailNormalizer.Object,
            dateTime.Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "jane@example.com", null, 99)]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("role", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task InviteValidator_WhenEmailExistsForTenantCompany_HasValidationError()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync("jane@example.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var roleRead = new Mock<IFgsRoleReadRepository>();
        roleRead.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(1, "TECH", "Technician", null, null, false, 1, true));

        var invitationRead = new Mock<IInvitationReadQuery>();
        invitationRead.Setup(r => r.HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
                It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var emailNormalizer = new Mock<IEmailNormalizer>();
        emailNormalizer.Setup(e => e.Normalize(It.IsAny<string>())).Returns<string>(s => s.Trim().ToLowerInvariant());

        var dateTime = new Mock<IDateTimeProvider>();
        dateTime.SetupGet(d => d.UtcNow).Returns(DateTimeOffset.UtcNow);

        var validator = new InviteFgsUserCommandValidator(
            userRead.Object,
            roleRead.Object,
            invitationRead.Object,
            emailNormalizer.Object,
            dateTime.Object);

        var result = await validator.ValidateAsync(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane", "jane@example.com", null, 1)]));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "A user with this email already exists for this tenant and company.");
    }

    private static InviteFgsUserCommandValidator CreateInviteValidator()
    {
        var userRead = new Mock<IFgsUserReadRepository>();
        userRead.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var roleRead = new Mock<IFgsRoleReadRepository>();
        roleRead.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsRoleDetailDto(1, "TECH", "Technician", null, null, false, 1, true));

        var invitationRead = new Mock<IInvitationReadQuery>();
        invitationRead.Setup(r => r.HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
                It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var emailNormalizer = new Mock<IEmailNormalizer>();
        emailNormalizer.Setup(e => e.Normalize(It.IsAny<string>())).Returns<string>(s => s.Trim().ToLowerInvariant());

        var dateTime = new Mock<IDateTimeProvider>();
        dateTime.SetupGet(d => d.UtcNow).Returns(DateTimeOffset.UtcNow);

        return new InviteFgsUserCommandValidator(
            userRead.Object,
            roleRead.Object,
            invitationRead.Object,
            emailNormalizer.Object,
            dateTime.Object);
    }
}

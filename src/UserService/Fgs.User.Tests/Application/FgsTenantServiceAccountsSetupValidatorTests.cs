using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceAccountsSetups.Validators;

namespace Fgs.User.Tests.Application;

public sealed class FgsTenantServiceAccountsSetupValidatorTests
{
    [Fact]
    public async Task UpdateValidator_WhenAccountIdIsZero_HasValidationError()
    {
        var validator = new UpdateFgsTenantServiceAccountsSetupCommandValidator();
        var dto = ValidUpdateDto() with { BankAccountId = 0 };

        var result = await validator.ValidateAsync(new UpdateFgsTenantServiceAccountsSetupCommand(dto));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("BankAccountId"));
    }

    [Fact]
    public async Task UpdateValidator_WhenValid_Passes()
    {
        var validator = new UpdateFgsTenantServiceAccountsSetupCommandValidator();

        var result = await validator.ValidateAsync(new UpdateFgsTenantServiceAccountsSetupCommand(ValidUpdateDto()));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenAllAccountsNull_Passes()
    {
        var validator = new UpdateFgsTenantServiceAccountsSetupCommandValidator();
        var dto = new FgsTenantServiceAccountsSetupUpdateDto(
            null, null, null, null, null, null, null, null, null, null, true);

        var result = await validator.ValidateAsync(new UpdateFgsTenantServiceAccountsSetupCommand(dto));

        result.IsValid.Should().BeTrue();
    }

    private static FgsTenantServiceAccountsSetupUpdateDto ValidUpdateDto() =>
        new(
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            true);
}

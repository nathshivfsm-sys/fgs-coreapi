using Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Validators;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Tests.Application;

public sealed class FgsTenantServiceSetupValidatorTests
{
    [Fact]
    public async Task UpdateValidator_WhenBillHoursInvalid_HasValidationError()
    {
        var validator = new UpdateFgsTenantServiceSetupCommandValidator();
        var dto = ValidUpdateDto() with { BillHoursFromDispatchOrArrive = "OTHER" };

        var result = await validator.ValidateAsync(new UpdateFgsTenantServiceSetupCommand(dto));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("BillHoursFromDispatchOrArrive"));
    }

    [Fact]
    public async Task UpdateValidator_WhenValid_Passes()
    {
        var validator = new UpdateFgsTenantServiceSetupCommandValidator();

        var result = await validator.ValidateAsync(new UpdateFgsTenantServiceSetupCommand(ValidUpdateDto()));

        result.IsValid.Should().BeTrue();
    }

    private static FgsTenantServiceSetupUpdateDto ValidUpdateDto() =>
        new(
            TimeCardOption.None,
            null,
            false,
            false,
            false,
            false,
            false,
            false,
            null,
            null,
            null,
            null,
            null,
            "ARRIVE",
            false,
            false,
            100,
            100,
            100,
            100,
            null,
            null,
            null,
            null,
            null,
            true);
}

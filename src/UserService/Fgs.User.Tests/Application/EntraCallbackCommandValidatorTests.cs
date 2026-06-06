using Fgs.User.Application.Features.Auth.Commands.EntraCallback;
using FluentValidation.TestHelper;

namespace Fgs.User.Tests.Application;

public sealed class EntraCallbackCommandValidatorTests
{
    private readonly EntraCallbackCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenCodeEmpty_HasError()
    {
        var result = _validator.TestValidate(new EntraCallbackCommand(string.Empty, Guid.NewGuid().ToString()));
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WhenStateEmpty_HasError()
    {
        var result = _validator.TestValidate(new EntraCallbackCommand("code", string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void Validate_WhenValid_HasNoErrors()
    {
        var result = _validator.TestValidate(new EntraCallbackCommand("code", Guid.NewGuid().ToString()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}

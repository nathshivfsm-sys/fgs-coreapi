using Fgs.User.Application.Features.Auth.Queries.EntraCallback;
using FluentValidation.TestHelper; // TestValidate extensions ship with FluentValidation package

namespace Fgs.User.Tests.Application;

public sealed class EntraCallbackQueryValidatorTests
{
    private readonly EntraCallbackQueryValidator _validator = new();

    [Fact]
    public void Validate_WhenCodeEmpty_HasError()
    {
        var result = _validator.TestValidate(new EntraCallbackQuery(string.Empty, Guid.NewGuid().ToString()));
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WhenStateEmpty_HasError()
    {
        var result = _validator.TestValidate(new EntraCallbackQuery("code", string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void Validate_WhenValid_HasNoErrors()
    {
        var result = _validator.TestValidate(new EntraCallbackQuery("code", Guid.NewGuid().ToString()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}

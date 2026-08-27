using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
using Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using FluentValidation.TestHelper;

namespace Fgs.User.Tests.Application;

public sealed class AuthValidatorTests
{
    [Fact]
    public void StartLoginValidator_RejectsEmptyEmail()
    {
        var validator = new StartLoginCommandValidator();
        var result = validator.TestValidate(new StartLoginCommand(string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void StartLoginValidator_AcceptsValidEmail()
    {
        var validator = new StartLoginCommandValidator();
        var result = validator.TestValidate(new StartLoginCommand("user@test.com"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EntraLoginCallbackValidator_RejectsEmptyCode()
    {
        var validator = new EntraLoginCallbackCommandValidator();
        var result = validator.TestValidate(new EntraLoginCallbackCommand(string.Empty, "state"));
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void EntraLoginCallbackValidator_RejectsEmptyState()
    {
        var validator = new EntraLoginCallbackCommandValidator();
        var result = validator.TestValidate(new EntraLoginCallbackCommand("code", string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void EntraLoginCallbackValidator_AcceptsValidPayload()
    {
        var validator = new EntraLoginCallbackCommandValidator();
        var result = validator.TestValidate(new EntraLoginCallbackCommand("code", "state"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EntraApiConnectorValidator_RejectsMissingEmailAndObjectId()
    {
        var validator = new EntraApiConnectorCommandValidator();
        var result = validator.TestValidate(new EntraApiConnectorCommand(null, null));
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void EntraApiConnectorValidator_AcceptsEmail()
    {
        var validator = new EntraApiConnectorCommandValidator();
        var result = validator.TestValidate(new EntraApiConnectorCommand("user@test.com", null));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EntraApiConnectorValidator_AcceptsObjectId()
    {
        var validator = new EntraApiConnectorCommandValidator();
        var result = validator.TestValidate(new EntraApiConnectorCommand(null, "oid-123"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ExchangeLoginCodeValidator_RejectsEmptyCode()
    {
        var validator = new ExchangeLoginCodeCommandValidator();
        var result = validator.TestValidate(new ExchangeLoginCodeCommand(string.Empty, "state"));
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void ExchangeLoginCodeValidator_RejectsEmptyState()
    {
        var validator = new ExchangeLoginCodeCommandValidator();
        var result = validator.TestValidate(new ExchangeLoginCodeCommand("code", string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void ExchangeLoginCodeValidator_AcceptsValidPayload()
    {
        var validator = new ExchangeLoginCodeCommandValidator();
        var result = validator.TestValidate(new ExchangeLoginCodeCommand("code", "state"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void RefreshAuthTokenValidator_RejectsEmptyToken()
    {
        var validator = new RefreshAuthTokenCommandValidator();
        var result = validator.TestValidate(new RefreshAuthTokenCommand(string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }

    [Fact]
    public void RefreshAuthTokenValidator_AcceptsValidToken()
    {
        var validator = new RefreshAuthTokenCommandValidator();
        var result = validator.TestValidate(new RefreshAuthTokenCommand("refresh-token"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}

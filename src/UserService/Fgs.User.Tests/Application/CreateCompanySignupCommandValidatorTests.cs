using Fgs.User.Application.Signup;

namespace Fgs.User.Tests.Application;

public sealed class CreateCompanySignupCommandValidatorTests
{
    private readonly CreateCompanySignupCommandValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidCommand_Succeeds()
    {
        var command = CreateValidCommand();
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithNullPassword_Succeeds()
    {
        var command = CreateValidCommand() with { Password = null };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithWeakPassword_Fails()
    {
        var command = CreateValidCommand() with { Password = "short" };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithInvalidEmail_Fails()
    {
        var command = CreateValidCommand() with { Email = "not-an-email" };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
    }

    private static CreateCompanySignupCommand CreateValidCommand() =>
        new(
            TenantCode: "acme",
            TenantName: "Acme Corp",
            Email: "admin@acme.com",
            DisplayName: "Acme Admin",
            Website: "https://acme.com",
            Password: "Str0ng!Passw0rd",
            TimeZone: "America/Chicago",
            DefaultCurrency: "USD");
}

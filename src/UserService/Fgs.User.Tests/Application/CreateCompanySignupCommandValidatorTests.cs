using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Features.Signup.DTOs;

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
    public async Task Validate_WithInvalidEmail_Fails()
    {
        var command = CreateValidCommand() with
        {
            Contact = CreateValidCommand().Contact with { Email = "not-an-email" }
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
    }

    private static CreateCompanySignupCommand CreateValidCommand() =>
        new(
            Contact: new SignupContactDto(
                Name: "Acme Admin",
                PhoneNumber: "+1 555-0100",
                Email: "admin@acme.com"),
            Company: new SignupCompanyDto(
                Name: "Acme Corp",
                Website: "https://acme.com",
                Address: new SignupAddressDto(
                    AddressLine1: "123 Main St",
                    AddressLine2: null,
                    City: "Springfield",
                    State: "IL",
                    PostalCode: "62701",
                    Country: "US"),
                CompanySize: "2-5"),
            BusinessTypeId: 1);
}

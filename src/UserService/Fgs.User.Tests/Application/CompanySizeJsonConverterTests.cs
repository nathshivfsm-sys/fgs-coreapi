using System.Text.Json;
using Fgs.User.Application.Signup;
using Fgs.User.Application.Signup.Json;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Tests.Application;

public sealed class CompanySizeJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new CompanySizeJsonConverter() }
    };

    [Theory]
    [InlineData("TwoToFive", CompanySize.TwoToFive)]
    [InlineData("twoToFive", CompanySize.TwoToFive)]
    [InlineData("2-5 employees", CompanySize.TwoToFive)]
    [InlineData("Single Owner", CompanySize.SingleOwner)]
    public void Deserialize_AcceptsStringFormats(string companySize, CompanySize expected)
    {
        var json = $$"""
            {
              "name": "Acme",
              "address": {
                "addressLine1": "1 Main",
                "city": "Austin",
                "state": "TX",
                "postalCode": "78701"
              },
              "companySize": "{{companySize}}"
            }
            """;

        var dto = JsonSerializer.Deserialize<SignupCompanyDto>(json, Options);
        dto!.CompanySize.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_AcceptsNumericValue()
    {
        const string json = """
            {
              "name": "Acme",
              "address": {
                "addressLine1": "1 Main",
                "city": "Austin",
                "state": "TX",
                "postalCode": "78701"
              },
              "companySize": 3
            }
            """;

        var dto = JsonSerializer.Deserialize<SignupCompanyDto>(json, Options);
        dto!.CompanySize.Should().Be(CompanySize.SixToTen);
    }

    [Fact]
    public void Deserialize_FullSignupCommand_UsesRootBodyNotCommandWrapper()
    {
        const string json = """
            {
              "contact": {
                "name": "Jane",
                "phoneNumber": "+1 555-0100",
                "email": "jane@test.com"
              },
              "company": {
                "name": "Acme",
                "address": {
                  "addressLine1": "1 Main",
                  "city": "Austin",
                  "state": "TX",
                  "postalCode": "78701"
                },
                "companySize": "2-5 employees"
              },
              "businessTypeId": 1
            }
            """;

        var command = JsonSerializer.Deserialize<CreateCompanySignupCommand>(json, Options);
        command.Should().NotBeNull();
        command!.Company.CompanySize.Should().Be(CompanySize.TwoToFive);
    }
}

using Fgs.Foundation.Validation;

namespace Fgs.Foundation.Tests.Validation;

public sealed class EmailAddressValidationTests
{
    [Theory]
    [InlineData("fgs_user55@yopmail.com")]
    [InlineData("admin@acme.co.uk")]
    [InlineData("user.name+tag@example.org")]
    public void IsValid_WithWellFormedEmail_ReturnsTrue(string email)
    {
        EmailAddressValidation.IsValid(email).Should().BeTrue();
    }

    [Theory]
    [InlineData("fgs_user55@yopmail,com")]
    [InlineData("user@domain,com.uk")]
    [InlineData("not-an-email")]
    [InlineData("a@b")]
    [InlineData("user@domain..com")]
    [InlineData("Name <user@test.com>")]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WithMalformedEmail_ReturnsFalse(string email)
    {
        EmailAddressValidation.IsValid(email).Should().BeFalse();
    }
}

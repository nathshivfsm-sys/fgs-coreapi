using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Signup.DTOs;

namespace Fgs.User.Tests.Application;

public sealed class SignupLocationFactoryTests
{
    [Fact]
    public void CreateCompanyLocation_MapsStructuredAddressFields()
    {
        var tenantId = Guid.NewGuid();
        const long companyId = 1;
        var locationId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        var location = SignupLocationFactory.CreateCompanyLocation(
            locationId,
            tenantId,
            companyId,
            masterEntityTypeId: 2,
            new SignupAddressDto(
                AddressLine1: "123 Main St",
                AddressLine2: "Suite 200",
                City: "Springfield",
                State: "IL",
                PostalCode: "62701",
                Country: "US"),
            now);

        location.Id.Should().Be(locationId);
        location.AddressLine1.Should().Be("123 Main St");
        location.AddressLine2.Should().Be("Suite 200");
        location.City.Should().Be("Springfield");
        location.State.Should().Be("IL");
        location.PostalCode.Should().Be("62701");
        location.Country.Should().Be("US");
        location.FormattedAddress.Should().Be("123 Main St, Suite 200, Springfield, IL 62701, US");
    }
}

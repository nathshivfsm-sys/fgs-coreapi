using Fgs.User.Application.Common.Locations;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Tests.Application;

public sealed class LocationMapperTests
{
    [Fact]
    public void ToDetailDto_WhenNull_ReturnsNull()
    {
        LocationMapper.ToDetailDto(null).Should().BeNull();
    }

    [Fact]
    public void ToDetailDto_MapsAllFields()
    {
        var location = new FgsLocation
        {
            Id = Guid.NewGuid(),
            AddressLine1 = "123 Main",
            AddressLine2 = "Suite 1",
            City = "Austin",
            State = "TX",
            Country = "US",
            PostalCode = "78701",
            FormattedAddress = "123 Main, Austin, TX 78701",
            Latitude = 30.1m,
            Longitude = -97.1m,
            PlaceId = "place-1",
            IsActive = true
        };

        var dto = LocationMapper.ToDetailDto(location);

        dto!.AddressLine1.Should().Be("123 Main");
        dto.City.Should().Be("Austin");
        dto.Latitude.Should().Be(30.1m);
        dto.PlaceId.Should().Be("place-1");
    }

    [Fact]
    public void ApplyWriteDto_FormatsAddressWhenMissingFormattedValue()
    {
        var location = new FgsLocation { Id = Guid.NewGuid() };
        var now = DateTimeOffset.UtcNow;

        LocationMapper.ApplyWriteDto(
            location,
            new LocationWriteDto("123 Main St", null, null, null, "Austin", "TX", null, "US", "78701", null, null, null, null),
            now);

        location.AddressLine1.Should().Be("123 Main St");
        location.FormattedAddress.Should().Be("123 Main St, Austin, TX 78701, US");
        location.UpdatedOn.Should().Be(now);
    }

    [Fact]
    public void ApplyWriteDto_KeepsProvidedFormattedAddress()
    {
        var location = new FgsLocation { Id = Guid.NewGuid() };

        LocationMapper.ApplyWriteDto(
            location,
            new LocationWriteDto("123 Main St", null, null, null, "Austin", "TX", null, "US", "78701", "Custom Format", null, null, null),
            DateTimeOffset.UtcNow);

        location.FormattedAddress.Should().Be("Custom Format");
    }

    [Fact]
    public void ApplyWriteDto_ReturnsNullFormattedAddressWhenRequiredFieldsMissing()
    {
        var location = new FgsLocation { Id = Guid.NewGuid() };

        LocationMapper.ApplyWriteDto(
            location,
            new LocationWriteDto("123 Main St", null, null, null, null, "TX", null, "US", "78701", null, null, null, null),
            DateTimeOffset.UtcNow);

        location.FormattedAddress.Should().BeNull();
    }
}

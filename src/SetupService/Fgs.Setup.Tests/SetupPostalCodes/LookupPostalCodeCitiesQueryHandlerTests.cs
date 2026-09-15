using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using Fgs.Setup.Application.Features.SetupPostalCodes.Queries.LookupPostalCodeCities;
using Moq;

namespace Fgs.Setup.Tests.SetupPostalCodes;

public sealed class LookupPostalCodeCitiesQueryHandlerTests
{
    [Fact]
    public async Task LookupCities_PassesFiltersToRepository()
    {
        var expected = new List<PostalCodeCityLookupDto> { new("Austin"), new("Dallas") };
        var readRepository = new Mock<IFgsSetupPostalCodeReadRepository>();
        readRepository
            .Setup(r => r.LookupDistinctCitiesAsync("US", "TX", true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<PostalCodeCityLookupDto>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns(async (
                string _,
                Func<Task<IReadOnlyList<PostalCodeCityLookupDto>>> factory,
                TimeSpan? __,
                CancellationToken ___) => await factory());

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new LookupPostalCodeCitiesQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);

        var response = await handler.Handle(
            new LookupPostalCodeCitiesQuery("us", "tx"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(expected);
        readRepository.Verify(
            r => r.LookupDistinctCitiesAsync("US", "TX", true, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

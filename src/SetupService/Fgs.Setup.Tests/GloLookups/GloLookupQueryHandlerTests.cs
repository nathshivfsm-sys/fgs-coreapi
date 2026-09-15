using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloBusinessTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloCountries;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;
using Moq;

namespace Fgs.Setup.Tests.GloLookups;

public sealed class GloLookupQueryHandlerTests
{
    [Fact]
    public async Task LookupCountries_ReturnsCachedRepositoryResult()
    {
        var expected = new List<GloCountryLookupDto> { new("US", "United States", "USD") };
        var readRepository = new Mock<IGloCountryReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var cache = CreatePassthroughCache<IReadOnlyList<GloCountryLookupDto>>();
        var handler = new LookupGloCountriesQueryHandler(readRepository.Object, cache.Object);

        var response = await handler.Handle(new LookupGloCountriesQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(expected);
        readRepository.Verify(r => r.LookupAsync(true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LookupStateProvinces_PassesNormalizedCountryCode()
    {
        var expected = new List<GloStateProvinceLookupDto> { new(1, "US", "TX", "Texas") };
        var readRepository = new Mock<IGloStateProvinceReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync("US", true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var cache = CreatePassthroughCache<IReadOnlyList<GloStateProvinceLookupDto>>();
        var handler = new LookupGloStateProvincesQueryHandler(readRepository.Object, cache.Object);

        var response = await handler.Handle(
            new LookupGloStateProvincesQuery(" us "),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(expected);
        readRepository.Verify(r => r.LookupAsync("US", true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LookupBusinessTypes_ReturnsCachedRepositoryResult()
    {
        var expected = new List<GloBusinessTypeLookupDto> { new(1, "HVAC", "HVAC") };
        var readRepository = new Mock<IGloBusinessTypeReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var cache = CreatePassthroughCache<IReadOnlyList<GloBusinessTypeLookupDto>>();
        var handler = new LookupGloBusinessTypesQueryHandler(readRepository.Object, cache.Object);

        var response = await handler.Handle(new LookupGloBusinessTypesQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(expected);
        readRepository.Verify(r => r.LookupAsync(true, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Mock<ICacheService> CreatePassthroughCache<T>()
        where T : class
    {
        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<T>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns(async (string _, Func<Task<T>> factory, TimeSpan? _, CancellationToken __) =>
                await factory());
        return cache;
    }
}

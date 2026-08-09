using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.GetFgsSetupPricingMatrixById;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixGetByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenNotFound_Returns404()
    {
        var repository = new Mock<IFgsSetupPricingMatrixReadRepository>();
        repository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupPricingMatrixDetailDto?)null);

        var response = await CreateHandler(repository.Object, new Mock<ICacheService>().Object)
            .Handle(new(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Handle_WhenFound_ReturnsSlimHeaderAndCachesIt()
    {
        var detail = new FgsSetupPricingMatrixDetailDto(
            1, "STANDARD", "Standard Pricing", true, false, true, 1,
            new DateOnly(2026, 1, 1), null, true, true);
        var repository = new Mock<IFgsSetupPricingMatrixReadRepository>();
        repository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<FgsSetupPricingMatrixDetailDto>(
            It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupPricingMatrixDetailDto?)null);

        var response = await CreateHandler(repository.Object, cache.Object)
            .Handle(new(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(detail);
        cache.Verify(c => c.SetAsync(
            It.IsAny<string>(), detail, It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static GetFgsSetupPricingMatrixByIdQueryHandler CreateHandler(
        IFgsSetupPricingMatrixReadRepository repository, ICacheService cache) =>
        new(repository, cache, new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 10, CompanyId = 20 }
        });

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

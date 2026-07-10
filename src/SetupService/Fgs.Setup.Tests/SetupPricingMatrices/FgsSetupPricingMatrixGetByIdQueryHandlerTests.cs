using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.GetFgsSetupPricingMatrixById;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixGetByIdQueryHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task Handle_WhenNotFound_Returns404()
    {
        var readRepository = new Mock<IFgsSetupPricingMatrixReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupPricingMatrixDetailDto?)null);

        var handler = CreateHandler(readRepository.Object, new Mock<ICacheService>().Object);
        var response = await handler.Handle(new GetFgsSetupPricingMatrixByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Handle_WhenFound_ReturnsAggregateDetail()
    {
        var detail = new FgsSetupPricingMatrixDetailDto(
            1,
            "STANDARD",
            "Standard Pricing",
            true,
            false,
            true,
            1,
            DateOnly.FromDateTime(DateTime.UtcNow),
            null,
            true,
            true,
            [
                new FgsSetupPricingMatrixLaborLineDetailDto(
                    10,
                    1,
                    5,
                    85m,
                    null,
                    null,
                    null,
                    true,
                    [])
            ],
            [new FgsSetupPricingMatrixMaterialTierDetailDto(20, 0m, 100m, 25m, true)],
            []);

        var readRepository = new Mock<IFgsSetupPricingMatrixReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetAsync<FgsSetupPricingMatrixDetailDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupPricingMatrixDetailDto?)null);

        var handler = CreateHandler(readRepository.Object, cache.Object);
        var response = await handler.Handle(new GetFgsSetupPricingMatrixByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.LaborLines.Should().HaveCount(1);
        response.Data.MaterialTiers.Should().HaveCount(1);
        response.Data.OtherItems.Should().BeEmpty();
        cache.Verify(
            c => c.SetAsync(
                It.IsAny<string>(),
                detail,
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static GetFgsSetupPricingMatrixByIdQueryHandler CreateHandler(
        IFgsSetupPricingMatrixReadRepository readRepository,
        ICacheService cache) =>
        new(
            readRepository,
            cache,
            new TestTenantContextAccessor
            {
                Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
            });

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

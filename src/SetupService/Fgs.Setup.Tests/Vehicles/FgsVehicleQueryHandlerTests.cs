using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using Fgs.Setup.Application.Features.Vehicles.Queries.GetFgsVehicleById;
using Fgs.Setup.Application.Features.Vehicles.Queries.ListVehicles;
using Moq;

namespace Fgs.Setup.Tests.Vehicles;

public sealed class FgsVehicleQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsVehicleDetailDto(1, 10, 20, 1, "OwnershipType", "OwnershipCompany", 1, "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", null, 10.5m, "PurchasedFrom", null, "Notes value", true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsVehicleReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsVehicleByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVehicleByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsVehicleReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsVehicleDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsVehicleByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVehicleByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsVehicleReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsVehicleListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsVehicleSummaryDto>([], 1, 25, 0));

        var handler = new ListVehiclesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListVehiclesQuery(new SetupListQuery(), new FgsVehicleListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

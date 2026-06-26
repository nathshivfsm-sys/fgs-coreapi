using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using Fgs.Setup.Application.Features.VehicleMaintenances.Queries.GetFgsVehicleMaintenanceById;
using Fgs.Setup.Application.Features.VehicleMaintenances.Queries.ListVehicleMaintenances;
using Moq;

namespace Fgs.Setup.Tests.VehicleMaintenances;

public sealed class FgsVehicleMaintenanceQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsVehicleMaintenanceDetailDto(1, 1, 1, DateOnly.FromDateTime(DateTime.UtcNow), 60, "ServiceProvider", "InvoiceNumber", 10.5m, null, 60, true, "Description", "Notes value", true);

        var readRepository = new Mock<IFgsVehicleMaintenanceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsVehicleMaintenanceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVehicleMaintenanceByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsVehicleMaintenanceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsVehicleMaintenanceDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsVehicleMaintenanceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVehicleMaintenanceByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsVehicleMaintenanceReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsVehicleMaintenanceListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsVehicleMaintenanceSummaryDto>([], 1, 25, 0));

        var handler = new ListVehicleMaintenancesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListVehicleMaintenancesQuery(new SetupListQuery(), new FgsVehicleMaintenanceListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

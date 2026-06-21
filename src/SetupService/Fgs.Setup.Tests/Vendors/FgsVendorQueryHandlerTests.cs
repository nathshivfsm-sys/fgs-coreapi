using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using Fgs.Setup.Application.Features.Vendors.Queries.GetFgsVendorById;
using Fgs.Setup.Application.Features.Vendors.Queries.ListVendors;
using Moq;

namespace Fgs.Setup.Tests.Vendors;

public sealed class FgsVendorQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsVendorDetailDto(1, 10, 20, "TEST", "Name", "LegalName", "VendorType", null, "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes value", false, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsVendorByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsVendorByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsVendorDetailDto?)null);

        var handler = new GetFgsVendorByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsVendorByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsVendorListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsVendorSummaryDto>([], 1, 25, 0));

        var handler = new ListVendorsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListVendorsQuery(new SetupListQuery(), new FgsVendorListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

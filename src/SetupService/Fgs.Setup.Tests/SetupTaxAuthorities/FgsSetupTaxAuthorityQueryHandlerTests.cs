using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.GetFgsSetupTaxAuthorityById;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListSetupTaxAuthorities;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxAuthorities;

public sealed class FgsSetupTaxAuthorityQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupTaxAuthorityDetailDto(1, 10, 20, "TEST", "Name value", "TEST", false, "Description value", true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupTaxAuthorityByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxAuthorityByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupTaxAuthorityDetailDto?)null);

        var handler = new GetFgsSetupTaxAuthorityByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxAuthorityByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupTaxAuthorityListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupTaxAuthoritySummaryDto>([], 1, 25, 0));

        var handler = new ListSetupTaxAuthoritiesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupTaxAuthoritiesQuery(new SetupListQuery(), new FgsSetupTaxAuthorityListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

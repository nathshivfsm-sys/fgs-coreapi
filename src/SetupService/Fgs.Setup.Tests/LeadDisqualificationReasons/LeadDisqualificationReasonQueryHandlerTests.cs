using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.GetLeadDisqualificationReasonById;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListLeadDisqualificationReasons;
using Moq;

namespace Fgs.Setup.Tests.LeadDisqualificationReasons;

public sealed class LeadDisqualificationReasonQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new LeadDisqualificationReasonDetailDto(1, 10, 20, "TEST", "ReasonName value", "Description value", 1, false, true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<ILeadDisqualificationReasonReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetLeadDisqualificationReasonByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetLeadDisqualificationReasonByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ILeadDisqualificationReasonReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((LeadDisqualificationReasonDetailDto?)null);

        var handler = new GetLeadDisqualificationReasonByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetLeadDisqualificationReasonByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ILeadDisqualificationReasonReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<LeadDisqualificationReasonListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<LeadDisqualificationReasonSummaryDto>([], 1, 25, 0));

        var handler = new ListLeadDisqualificationReasonsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListLeadDisqualificationReasonsQuery(new SetupListQuery(), new LeadDisqualificationReasonListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Application.Features.JobTypes.Queries.GetJobTypeById;
using Fgs.Setup.Application.Features.JobTypes.Queries.ListJobTypes;
using Moq;

namespace Fgs.Setup.Tests.JobTypes;

public sealed class JobTypeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new JobTypeDetailDto(1, 10, 20, 1, null, "TEST", "TaskName value", "Description value", "UsedFor value", "Trade value", 60, "BusinessUnit value", 5, "BackgroundColor value", "TextColor value", true, true, 1, true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<IJobTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetJobTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IJobTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((JobTypeDetailDto?)null);

        var handler = new GetJobTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IJobTypeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<JobTypeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<JobTypeSummaryDto>([], 1, 25, 0));

        var handler = new ListJobTypesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListJobTypesQuery(new SetupListQuery(), new JobTypeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.GetFgsSetupTechSkillLevelById;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.ListSetupTechSkillLevels;
using Moq;

namespace Fgs.Setup.Tests.SetupTechSkillLevels;

public sealed class FgsSetupTechSkillLevelQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupTechSkillLevelDetailDto(1, 10, 20, "TEST", "Name value", "Description value", 60, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupTechSkillLevelReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupTechSkillLevelByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTechSkillLevelByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupTechSkillLevelReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupTechSkillLevelDetailDto?)null);

        var handler = new GetFgsSetupTechSkillLevelByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTechSkillLevelByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupTechSkillLevelReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupTechSkillLevelListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupTechSkillLevelSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupTechSkillLevelsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupTechSkillLevelsQuery(new SetupListQuery(), new FgsSetupTechSkillLevelListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

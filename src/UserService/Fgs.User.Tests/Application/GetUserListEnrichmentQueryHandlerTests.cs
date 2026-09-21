using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Queries.GetUserListEnrichment;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class GetUserListEnrichmentQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserIdsEmpty_ReturnsEmptyWithoutRepository()
    {
        var read = new Mock<IFgsUserReadRepository>();
        var handler = new GetUserListEnrichmentQueryHandler(read.Object);

        var response = await handler.Handle(new GetUserListEnrichmentQuery([]), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEmpty();
        read.Verify(
            r => r.GetListEnrichmentAsync(It.IsAny<IReadOnlyList<Guid>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIdsProvided_ReturnsRepositoryResult()
    {
        var userId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var enrichment = new FgsUserListEnrichmentDto(userId, 5, "Tech", DateTimeOffset.UtcNow);
        var read = new Mock<IFgsUserReadRepository>();
        read
            .Setup(r => r.GetListEnrichmentAsync(
                It.Is<IReadOnlyList<Guid>>(ids => ids.SequenceEqual(new[] { userId })),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FgsUserListEnrichmentDto> { enrichment });

        var handler = new GetUserListEnrichmentQueryHandler(read.Object);
        var response = await handler.Handle(new GetUserListEnrichmentQuery([userId]), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle().Which.UserId.Should().Be(userId);
        response.Data![0].RoleName.Should().Be("Tech");
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }
}

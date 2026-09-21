using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Queries.GetUserIdsByRoles;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class GetUserIdsByRolesQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenRoleIdsEmpty_ReturnsEmptyWithoutRepository()
    {
        var read = new Mock<IFgsUserReadRepository>();
        var handler = new GetUserIdsByRolesQueryHandler(read.Object);

        var response = await handler.Handle(new GetUserIdsByRolesQuery([]), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEmpty();
        read.Verify(
            r => r.GetIdsByRoleIdsAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoleIdsProvided_ReturnsRepositoryResult()
    {
        var userId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        var read = new Mock<IFgsUserReadRepository>();
        read
            .Setup(r => r.GetIdsByRoleIdsAsync(
                It.Is<IReadOnlyList<long>>(ids => ids.SequenceEqual(new long[] { 1L, 2L })),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid> { userId });

        var handler = new GetUserIdsByRolesQueryHandler(read.Object);
        var response = await handler.Handle(new GetUserIdsByRolesQuery([1, 2]), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle().Which.Should().Be(userId);
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }
}

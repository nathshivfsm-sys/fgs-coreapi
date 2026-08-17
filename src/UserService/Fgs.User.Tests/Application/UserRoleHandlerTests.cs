using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;
using Fgs.User.Application.Features.UserRoles.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class UserRoleHandlerTests
{
    private static readonly Guid UserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsUserRoleWriteService>();
        write.Setup(w => w.SyncAsync(It.IsAny<FgsUserRoleSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new FgsUserRoleDetailDto(1, UserId, 10, DateTimeOffset.UtcNow, "test"),
                new FgsUserRoleDetailDto(2, UserId, 11, DateTimeOffset.UtcNow, "test")
            ]);

        var handler = new SyncFgsUserRolesCommandHandler(
            write.Object,
            NullLogger<SyncFgsUserRolesCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsUserRolesCommand(new FgsUserRoleSyncDto(UserId, [10, 11])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListByUserIdHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.ListByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsUserRoleDetailDto(1, UserId, 10, DateTimeOffset.UtcNow, "test")]);

        var handler = new ListFgsUserRolesByUserIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsUserRolesByUserIdQuery(UserId), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsRoleId == 10);
    }

    [Fact]
    public async Task SyncValidator_RejectsEmptyUserId()
    {
        var validator = new SyncFgsUserRolesCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsUserRolesCommand(new FgsUserRoleSyncDto(Guid.Empty, [1])));

        result.IsValid.Should().BeFalse();
    }
}

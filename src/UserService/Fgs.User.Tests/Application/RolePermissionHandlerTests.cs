using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;
using Fgs.User.Application.Features.RolePermissions.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RolePermissionHandlerTests
{
    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRolePermissionWriteService>();
        var expected = new List<FgsRolePermissionDetailDto>
        {
            new(1, 10, 100, DateTimeOffset.UtcNow, "test"),
            new(2, 10, 101, DateTimeOffset.UtcNow, "test")
        };
        write.Setup(w => w.SyncAsync(It.IsAny<FgsRolePermissionSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new SyncFgsRolePermissionsCommandHandler(
            write.Object,
            NullLogger<SyncFgsRolePermissionsCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsRolePermissionsCommand(new FgsRolePermissionSyncDto(10, [100, 101])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        write.Verify(w => w.SyncAsync(
            It.Is<FgsRolePermissionSyncDto>(d => d.FgsRoleId == 10 && d.FgsPermissionIds.SequenceEqual(new long[] { 100, 101 })),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListByRoleIdHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.ListByRoleIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRolePermissionDetailDto(1, 10, 100, DateTimeOffset.UtcNow, "test")]);

        var handler = new ListFgsRolePermissionsByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRolePermissionsByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsPermissionId == 100);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidIds()
    {
        var validator = new SyncFgsRolePermissionsCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRolePermissionsCommand(new FgsRolePermissionSyncDto(0, [0])));

        result.IsValid.Should().BeFalse();
    }
}

using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;
using Fgs.User.Application.Features.RoleMenus.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RoleMenuHandlerTests
{
    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRoleMenuWriteService>();
        var expected = new List<FgsRoleMenuDetailDto>
        {
            new(1, 10, 100, 1, true, DateTimeOffset.UtcNow, "test"),
            new(2, 10, 101, 2, true, DateTimeOffset.UtcNow, "test")
        };
        write.Setup(w => w.SyncAsync(It.IsAny<FgsRoleMenuSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new SyncFgsRoleMenusCommandHandler(
            write.Object,
            NullLogger<SyncFgsRoleMenusCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsRoleMenusCommand(new FgsRoleMenuSyncDto(
                10,
                [new FgsRoleMenuSyncItemDto(100, 1), new FgsRoleMenuSyncItemDto(101, 2)])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        write.Verify(w => w.SyncAsync(
            It.Is<FgsRoleMenuSyncDto>(d =>
                d.RoleId == 10
                && d.Items.Count == 2
                && d.Items[0].MenuId == 100
                && d.Items[1].MenuId == 101),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListByRoleIdHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.ListByRoleIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleMenuDetailDto(1, 10, 100, 1, true, DateTimeOffset.UtcNow, "test")]);

        var handler = new ListFgsRoleMenusByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRoleMenusByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuId == 100 && x.RoleId == 10);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidIds()
    {
        var validator = new SyncFgsRoleMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRoleMenusCommand(new FgsRoleMenuSyncDto(
                0,
                [new FgsRoleMenuSyncItemDto(0, -1)])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SyncValidator_AcceptsValidPayload()
    {
        var validator = new SyncFgsRoleMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRoleMenusCommand(new FgsRoleMenuSyncDto(
                10,
                [new FgsRoleMenuSyncItemDto(100, 1, true)])));

        result.IsValid.Should().BeTrue();
    }
}

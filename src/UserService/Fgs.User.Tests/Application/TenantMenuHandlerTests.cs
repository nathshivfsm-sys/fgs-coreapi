using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Application.Features.TenantMenus.Queries.ListFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class TenantMenuHandlerTests
{
    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsTenantMenuWriteService>();
        var expected = new List<FgsTenantMenuDetailDto>
        {
            new(1, 100, 1, true, DateTimeOffset.UtcNow, "test"),
            new(2, 101, 2, false, DateTimeOffset.UtcNow, "test")
        };
        write.Setup(w => w.SyncAsync(It.IsAny<FgsTenantMenuSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new SyncFgsTenantMenusCommandHandler(
            write.Object,
            NullLogger<SyncFgsTenantMenusCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [new FgsTenantMenuSyncItemDto(100, 1), new FgsTenantMenuSyncItemDto(101, 2, false)])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        write.Verify(w => w.SyncAsync(
            It.Is<FgsTenantMenuSyncDto>(d =>
                d.Items.Count == 2
                && d.Items[0].MenuId == 100
                && d.Items[1].IsActive == false),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsTenantMenuDetailDto(1, 100, 1, true, DateTimeOffset.UtcNow, "test")]);

        var handler = new ListFgsTenantMenusQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsTenantMenusQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuId == 100);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidMenuIds()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [new FgsTenantMenuSyncItemDto(0, -1)])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SyncValidator_AcceptsEmptyList()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto([])));

        result.IsValid.Should().BeTrue();
    }
}

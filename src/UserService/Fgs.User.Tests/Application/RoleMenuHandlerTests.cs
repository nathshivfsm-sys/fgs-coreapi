using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.CreateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.PatchFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.UpdateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Application.Features.RoleMenus.Queries.GetFgsRoleMenuById;
using Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;
using Fgs.User.Application.Features.RoleMenus.Queries.LookupFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RoleMenuHandlerTests
{
    private static FgsRoleMenuDetailDto Detail(
        long id = 1,
        long roleId = 10,
        int menuId = 100,
        short displayOrder = 1,
        bool isActive = true) =>
        new(id, roleId, menuId, displayOrder, isActive, DateTimeOffset.UtcNow, "test");

    private static FgsRoleMenuCreateDto CreateDto(
        long roleId = 10,
        int menuId = 100,
        short displayOrder = 1) =>
        new(roleId, menuId, displayOrder);

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRoleMenuWriteService>();
        var expected = new List<FgsRoleMenuDetailDto>
        {
            Detail(1, 10, 100, 1),
            Detail(2, 10, 101, 2)
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
            .ReturnsAsync([Detail()]);

        var handler = new ListFgsRoleMenusByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRoleMenusByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuId == 100 && x.RoleId == 10);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsNotFound()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleMenuDetailDto?)null);

        var handler = new GetFgsRoleMenuByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleMenuByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsDetail()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new GetFgsRoleMenuByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleMenuByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.MenuId.Should().Be(100);
    }

    [Fact]
    public async Task LookupHandler_ReturnsItems()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.LookupAsync(10, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleMenuLookupDto(1, 10, 100, 1)]);

        var handler = new LookupFgsRoleMenusQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsRoleMenusQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuId == 100 && x.RoleId == 10);
    }

    [Fact]
    public async Task CreateHandler_ReturnsCreated()
    {
        var write = new Mock<IFgsRoleMenuWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsRoleMenuCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new CreateFgsRoleMenuCommandHandler(
            write.Object,
            NullLogger<CreateFgsRoleMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsRoleMenuCommand(CreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdated()
    {
        var write = new Mock<IFgsRoleMenuWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsRoleMenuUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(displayOrder: 5));

        var handler = new UpdateFgsRoleMenuCommandHandler(
            write.Object,
            NullLogger<UpdateFgsRoleMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsRoleMenuCommand(1, new FgsRoleMenuUpdateDto(10, 100, 5)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.DisplayOrder.Should().Be(5);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatched()
    {
        var write = new Mock<IFgsRoleMenuWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsRoleMenuPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(isActive: false));

        var handler = new PatchFgsRoleMenuCommandHandler(
            write.Object,
            NullLogger<PatchFgsRoleMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
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

    [Fact]
    public async Task LookupValidator_RejectsMissingRoleId()
    {
        var validator = new LookupFgsRoleMenusQueryValidator();
        var result = await validator.ValidateAsync(new LookupFgsRoleMenusQuery(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicateRoleMenu()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.ExistsByRoleMenuAsync(10, 100, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsRoleMenuCommand(CreateDto()));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidCreate()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.ExistsByRoleMenuAsync(It.IsAny<long>(), It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsRoleMenuCommand(CreateDto()));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        var validator = new UpdateFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRoleMenuCommand(0, new FgsRoleMenuUpdateDto(10, 100)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_AcceptsIsActiveOnly()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(IsActive: false)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidRoleId()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(RoleId: 0)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsNegativeDisplayOrder()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(DisplayOrder: -1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicateWhenMenuIdChanges()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleMenuAsync(10, 200, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(MenuId: 200)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_AllowsWhenAssignmentMissing()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleMenuDetailDto?)null);

        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(MenuId: 200)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsRoleIdChangeWhenUnique()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleMenuAsync(20, 100, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleMenuCommand(1, new FgsRoleMenuPatchDto(RoleId: 20)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsValidUpdate()
    {
        var read = new Mock<IFgsRoleMenuReadRepository>();
        read.Setup(r => r.ExistsByRoleMenuAsync(10, 100, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsRoleMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRoleMenuCommand(1, new FgsRoleMenuUpdateDto(10, 100, 2)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task LookupValidator_AcceptsValidRoleId()
    {
        var validator = new LookupFgsRoleMenusQueryValidator();
        var result = await validator.ValidateAsync(new LookupFgsRoleMenusQuery(10, false));

        result.IsValid.Should().BeTrue();
    }
}

using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Commands.CreateFgsTenantMenu;
using Fgs.User.Application.Features.TenantMenus.Commands.PatchFgsTenantMenu;
using Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Commands.UpdateFgsTenantMenu;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Application.Features.TenantMenus.Queries.GetFgsTenantMenuById;
using Fgs.User.Application.Features.TenantMenus.Queries.ListFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Queries.LookupFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class TenantMenuHandlerTests
{
    private static FgsTenantMenuDetailDto Detail(
        long id,
        int menuId,
        string code,
        string name,
        string menuType,
        short displayOrder = 1,
        bool isActive = true) =>
        new(
            id,
            menuId,
            code,
            name,
            null,
            null,
            menuType,
            null,
            null,
            displayOrder,
            isActive,
            DateTimeOffset.UtcNow,
            "test");

    private static FgsTenantMenuSyncItemDto SyncItem(
        int menuId,
        string code,
        string name,
        string menuType,
        short displayOrder = 1,
        bool isActive = true) =>
        new(menuId, code, name, menuType, DisplayOrder: displayOrder, IsActive: isActive);

    private static FgsTenantMenuCreateDto CreateDto(
        int menuId = 100,
        string code = "DASHBOARD",
        string name = "Dashboard",
        string menuType = "PAGE") =>
        new(menuId, code, name, menuType);

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsTenantMenuWriteService>();
        var expected = new List<FgsTenantMenuDetailDto>
        {
            Detail(1, 100, "DASHBOARD", "Dashboard", "PAGE", 1),
            Detail(2, 101, "SETTINGS", "Settings", "GROUP", 2, false)
        };
        write.Setup(w => w.SyncAsync(It.IsAny<FgsTenantMenuSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new SyncFgsTenantMenusCommandHandler(
            write.Object,
            NullLogger<SyncFgsTenantMenusCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [
                    SyncItem(100, "DASHBOARD", "Dashboard", "PAGE"),
                    SyncItem(101, "SETTINGS", "Settings", "GROUP", 2, false)
                ])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        write.Verify(w => w.SyncAsync(
            It.Is<FgsTenantMenuSyncDto>(d =>
                d.Items.Count == 2
                && d.Items[0].MenuId == 100
                && d.Items[0].MenuCode == "DASHBOARD"
                && d.Items[1].IsActive == false),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([Detail(1, 100, "DASHBOARD", "Dashboard", "PAGE")]);

        var handler = new ListFgsTenantMenusQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsTenantMenusQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuId == 100 && x.MenuCode == "DASHBOARD");
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsNotFound()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTenantMenuDetailDto?)null);

        var handler = new GetFgsTenantMenuByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantMenuByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsDetail()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(1, 100, "DASHBOARD", "Dashboard", "PAGE"));

        var handler = new GetFgsTenantMenuByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantMenuByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.MenuCode.Should().Be("DASHBOARD");
    }

    [Fact]
    public async Task LookupHandler_ReturnsItems()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsTenantMenuLookupDto(1, 100, "DASHBOARD", "Dashboard", 1)]);

        var handler = new LookupFgsTenantMenusQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsTenantMenusQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.MenuCode == "DASHBOARD");
    }

    [Fact]
    public async Task CreateHandler_ReturnsCreated()
    {
        var write = new Mock<IFgsTenantMenuWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsTenantMenuCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(1, 100, "DASHBOARD", "Dashboard", "PAGE"));

        var handler = new CreateFgsTenantMenuCommandHandler(
            write.Object,
            NullLogger<CreateFgsTenantMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsTenantMenuCommand(CreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdated()
    {
        var write = new Mock<IFgsTenantMenuWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsTenantMenuUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(1, 100, "DASHBOARD", "Home", "PAGE"));

        var handler = new UpdateFgsTenantMenuCommandHandler(
            write.Object,
            NullLogger<UpdateFgsTenantMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsTenantMenuCommand(1, new FgsTenantMenuUpdateDto(100, "DASHBOARD", "Home", "PAGE")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Home");
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatched()
    {
        var write = new Mock<IFgsTenantMenuWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsTenantMenuPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(1, 100, "DASHBOARD", "Dashboard", "PAGE", isActive: false));

        var handler = new PatchFgsTenantMenuCommandHandler(
            write.Object,
            NullLogger<PatchFgsTenantMenuCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsTenantMenuCommand(1, new FgsTenantMenuPatchDto(IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicateMenuId()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ExistsByMenuIdAsync(100, null, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        read.Setup(r => r.ExistsByMenuCodeAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsTenantMenuCommand(CreateDto()));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidCreate()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ExistsByMenuIdAsync(It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        read.Setup(r => r.ExistsByMenuCodeAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(new CreateFgsTenantMenuCommand(CreateDto()));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        var validator = new UpdateFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsTenantMenuCommand(0, new FgsTenantMenuUpdateDto(100, "DASHBOARD", "Dashboard", "PAGE")));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_AcceptsIsActiveOnly()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        var validator = new PatchFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsTenantMenuCommand(1, new FgsTenantMenuPatchDto(IsActive: false)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicateMenuCode()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ExistsByMenuCodeAsync("DASHBOARD", 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsTenantMenuCommand(1, new FgsTenantMenuPatchDto(MenuCode: "DASHBOARD")));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicateMenuId()
    {
        var read = new Mock<IFgsTenantMenuReadRepository>();
        read.Setup(r => r.ExistsByMenuIdAsync(100, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsTenantMenuCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsTenantMenuCommand(1, new FgsTenantMenuPatchDto(MenuId: 100)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidMenuIds()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [new FgsTenantMenuSyncItemDto(0, "", "", "", DisplayOrder: -1)])));

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

    [Fact]
    public async Task SyncValidator_AcceptsValidCatalogFields()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [
                    new FgsTenantMenuSyncItemDto(
                        100,
                        "DASHBOARD",
                        "Dashboard",
                        "PAGE",
                        Description: "Home",
                        ParentMenuId: 1,
                        Route: "/dashboard",
                        Icon: "home")
                ])));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task SyncValidator_RejectsBlankCatalogFields()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [new FgsTenantMenuSyncItemDto(1, " ", " ", " ")])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidParentMenuId()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [
                    new FgsTenantMenuSyncItemDto(
                        100,
                        "CHILD",
                        "Child",
                        "PAGE",
                        ParentMenuId: 0)
                ])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task SyncValidator_RejectsOversizedOptionalFields()
    {
        var validator = new SyncFgsTenantMenusCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsTenantMenusCommand(new FgsTenantMenuSyncDto(
                [
                    new FgsTenantMenuSyncItemDto(
                        100,
                        "DASHBOARD",
                        "Dashboard",
                        "PAGE",
                        Description: new string('d', 256),
                        Route: new string('r', 256),
                        Icon: new string('i', 101))
                ])));

        result.IsValid.Should().BeFalse();
    }
}

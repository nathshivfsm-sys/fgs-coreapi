using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Permissions.Commands.CreateFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.PatchFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.UpdateFgsPermission;
using Fgs.User.Application.Features.Permissions.Dtos;
using Fgs.User.Application.Features.Permissions.Queries.GetFgsPermissionById;
using Fgs.User.Application.Features.Permissions.Queries.ListFgsPermissions;
using Fgs.User.Application.Features.Permissions.Queries.LookupFgsPermissions;
using Fgs.User.Application.Features.Permissions.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class PermissionHandlerTests
{
    private static readonly FgsPermissionDetailDto Detail =
        new(1, "USERS.READ", "USERS", "USERS", "READ", "Read Users", "Desc", 1, true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedPermission()
    {
        var write = new Mock<IFgsPermissionWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsPermissionCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsPermissionCommandHandler(write.Object, NullLogger<CreateFgsPermissionCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsPermissionCommand(new FgsPermissionCreateDto("USERS.READ", "USERS", "USERS", "READ", "Read Users", "Desc")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.PermissionCode.Should().Be("USERS.READ");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedPermission()
    {
        var write = new Mock<IFgsPermissionWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsPermissionUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsPermissionCommandHandler(write.Object, NullLogger<UpdateFgsPermissionCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsPermissionCommand(1, new FgsPermissionUpdateDto("USERS.READ", "USERS", "USERS", "READ", "Read Users", "Desc", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedPermission()
    {
        var write = new Mock<IFgsPermissionWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsPermissionPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsPermissionCommandHandler(write.Object, NullLogger<PatchFgsPermissionCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsPermissionCommand(1, new FgsPermissionPatchDto(null, null, null, null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsPermission()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsPermissionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsPermissionByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.PermissionCode.Should().Be("USERS.READ");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsPermissionDetailDto?)null);

        var handler = new GetFgsPermissionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsPermissionByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsPermissionSummaryDto(1, "USERS.READ", "USERS", "USERS", "READ", "Read Users", "Desc", 1, true);
        var paged = new PagedResult<FgsPermissionSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsPermissionListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsPermissionsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsPermissionsQuery(new IdentityListQuery(), new FgsPermissionListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsPermissionLookupDto(1, "USERS.READ", "USERS", "USERS", "READ", "Read Users", 1)]);

        var handler = new LookupFgsPermissionsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsPermissionsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsLowercaseCode()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.ExistsByPermissionCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsPermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsPermissionCommand(new FgsPermissionCreateDto("users.read", "USERS", "USERS", "READ", "Read Users", null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        read.Setup(r => r.ExistsByPermissionCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsPermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsPermissionCommand(new FgsPermissionCreateDto("USERS.READ", "USERS", "USERS", "READ", "Read Users", null)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        var validator = new UpdateFgsPermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsPermissionCommand(0, new FgsPermissionUpdateDto("USERS.READ", "USERS", "USERS", "READ", "Read Users", null, 1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsPermissionReadRepository>();
        var validator = new PatchFgsPermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsPermissionCommand(0, new FgsPermissionPatchDto("USERS.READ", null, null, null, null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}

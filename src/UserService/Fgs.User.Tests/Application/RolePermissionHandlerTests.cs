using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.PatchFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.UpdateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Application.Features.RolePermissions.Queries.GetFgsRolePermissionById;
using Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;
using Fgs.User.Application.Features.RolePermissions.Queries.LookupFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RolePermissionHandlerTests
{
    private static FgsRolePermissionDetailDto Detail(long id = 1, long roleId = 10, long permissionId = 100) =>
        new(id, roleId, permissionId, DateTimeOffset.UtcNow, "test");

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRolePermissionWriteService>();
        var expected = new List<FgsRolePermissionDetailDto>
        {
            Detail(1, 10, 100),
            Detail(2, 10, 101)
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
            .ReturnsAsync([Detail()]);

        var handler = new ListFgsRolePermissionsByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRolePermissionsByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsPermissionId == 100);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsNotFound()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRolePermissionDetailDto?)null);

        var handler = new GetFgsRolePermissionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRolePermissionByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsDetail()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new GetFgsRolePermissionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRolePermissionByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsPermissionId.Should().Be(100);
    }

    [Fact]
    public async Task LookupHandler_ReturnsItems()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.LookupAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRolePermissionLookupDto(1, 10, 100)]);

        var handler = new LookupFgsRolePermissionsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsRolePermissionsQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsPermissionId == 100);
    }

    [Fact]
    public async Task CreateHandler_ReturnsCreated()
    {
        var write = new Mock<IFgsRolePermissionWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsRolePermissionCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new CreateFgsRolePermissionCommandHandler(
            write.Object,
            NullLogger<CreateFgsRolePermissionCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsRolePermissionCommand(new FgsRolePermissionCreateDto(10, 100)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdated()
    {
        var write = new Mock<IFgsRolePermissionWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsRolePermissionUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(permissionId: 200));

        var handler = new UpdateFgsRolePermissionCommandHandler(
            write.Object,
            NullLogger<UpdateFgsRolePermissionCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsRolePermissionCommand(1, new FgsRolePermissionUpdateDto(200)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsPermissionId.Should().Be(200);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatched()
    {
        var write = new Mock<IFgsRolePermissionWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsRolePermissionPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(permissionId: 201));

        var handler = new PatchFgsRolePermissionCommandHandler(
            write.Object,
            NullLogger<PatchFgsRolePermissionCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsRolePermissionCommand(1, new FgsRolePermissionPatchDto(201)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsPermissionId.Should().Be(201);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidIds()
    {
        var validator = new SyncFgsRolePermissionsCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRolePermissionsCommand(new FgsRolePermissionSyncDto(0, [0])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 100, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsRolePermissionCommand(new FgsRolePermissionCreateDto(10, 100)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidCreate()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 100, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsRolePermissionCommand(new FgsRolePermissionCreateDto(10, 100)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task LookupValidator_RejectsInvalidRoleId()
    {
        var validator = new LookupFgsRolePermissionsQueryValidator();
        var result = await validator.ValidateAsync(new LookupFgsRolePermissionsQuery(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 200, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRolePermissionCommand(1, new FgsRolePermissionUpdateDto(200)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsWhenMissing()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRolePermissionDetailDto?)null);

        var validator = new UpdateFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRolePermissionCommand(1, new FgsRolePermissionUpdateDto(200)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsUnique()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 200, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRolePermissionCommand(1, new FgsRolePermissionUpdateDto(200)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 200, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRolePermissionCommand(1, new FgsRolePermissionPatchDto(200)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_SkipsWhenPermissionIdOmitted()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        var validator = new PatchFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRolePermissionCommand(1, new FgsRolePermissionPatchDto()));

        result.IsValid.Should().BeTrue();
        read.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task PatchValidator_AcceptsWhenMissing()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRolePermissionDetailDto?)null);

        var validator = new PatchFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRolePermissionCommand(1, new FgsRolePermissionPatchDto(200)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsUnique()
    {
        var read = new Mock<IFgsRolePermissionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndPermissionIdAsync(10, 200, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsRolePermissionCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRolePermissionCommand(1, new FgsRolePermissionPatchDto(200)));

        result.IsValid.Should().BeTrue();
    }
}

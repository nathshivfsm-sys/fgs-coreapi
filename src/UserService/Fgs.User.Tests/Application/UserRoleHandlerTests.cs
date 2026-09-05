using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.PatchFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.UpdateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.UserRoles.Queries.GetFgsUserRoleById;
using Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;
using Fgs.User.Application.Features.UserRoles.Queries.LookupFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class UserRoleHandlerTests
{
    private static readonly Guid UserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static FgsUserRoleDetailDto Detail(long id = 1, long roleId = 10) =>
        new(id, UserId, roleId, DateTimeOffset.UtcNow, "test");

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsUserRoleWriteService>();
        write.Setup(w => w.SyncAsync(It.IsAny<FgsUserRoleSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                Detail(1, 10),
                Detail(2, 11)
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
            .ReturnsAsync([Detail()]);

        var handler = new ListFgsUserRolesByUserIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsUserRolesByUserIdQuery(UserId), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsRoleId == 10);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsNotFound()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsUserRoleDetailDto?)null);

        var handler = new GetFgsUserRoleByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsUserRoleByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsDetail()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new GetFgsUserRoleByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsUserRoleByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsRoleId.Should().Be(10);
    }

    [Fact]
    public async Task LookupHandler_ReturnsItems()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.LookupAsync(UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsUserRoleLookupDto(1, UserId, 10)]);

        var handler = new LookupFgsUserRolesQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsUserRolesQuery(UserId), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsRoleId == 10);
    }

    [Fact]
    public async Task CreateHandler_ReturnsCreated()
    {
        var write = new Mock<IFgsUserRoleWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsUserRoleCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new CreateFgsUserRoleCommandHandler(
            write.Object,
            NullLogger<CreateFgsUserRoleCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsUserRoleCommand(new FgsUserRoleCreateDto(UserId, 10)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdated()
    {
        var write = new Mock<IFgsUserRoleWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsUserRoleUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(roleId: 20));

        var handler = new UpdateFgsUserRoleCommandHandler(
            write.Object,
            NullLogger<UpdateFgsUserRoleCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsUserRoleCommand(1, new FgsUserRoleUpdateDto(20)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsRoleId.Should().Be(20);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatched()
    {
        var write = new Mock<IFgsUserRoleWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsUserRolePatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(roleId: 21));

        var handler = new PatchFgsUserRoleCommandHandler(
            write.Object,
            NullLogger<PatchFgsUserRoleCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsUserRoleCommand(1, new FgsUserRolePatchDto(21)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsRoleId.Should().Be(21);
    }

    [Fact]
    public async Task SyncValidator_RejectsEmptyUserId()
    {
        var validator = new SyncFgsUserRolesCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsUserRolesCommand(new FgsUserRoleSyncDto(Guid.Empty, [1])));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.ExistsByUserIdAndRoleIdAsync(UserId, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsUserRoleCommand(new FgsUserRoleCreateDto(UserId, 10)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByUserIdAndRoleIdAsync(UserId, 20, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsUserRoleCommand(1, new FgsUserRoleUpdateDto(20)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByUserIdAndRoleIdAsync(UserId, 20, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsUserRoleCommand(1, new FgsUserRolePatchDto(20)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_SkipsWhenRoleIdOmitted()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        var validator = new PatchFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsUserRoleCommand(1, new FgsUserRolePatchDto()));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsWhenMissing()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsUserRoleDetailDto?)null);

        var validator = new PatchFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsUserRoleCommand(1, new FgsUserRolePatchDto(20)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsUnique()
    {
        var read = new Mock<IFgsUserRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByUserIdAndRoleIdAsync(UserId, 20, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsUserRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsUserRoleCommand(1, new FgsUserRolePatchDto(20)));

        result.IsValid.Should().BeTrue();
    }
}

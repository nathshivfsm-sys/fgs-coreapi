using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Commands.PatchFgsUser;
using Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;
using Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Application.Features.Users.Queries.GetFgsUserById;
using Fgs.User.Application.Features.Users.Queries.ListFgsUsers;
using Fgs.User.Application.Features.Users.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class UserQueryHandlerTests
{
    private static readonly Guid UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    private static readonly FgsUserDetailDto Detail =
        new(UserId, "Test User", "user@test.com", null, 1, "Admin", "Accepted", true, true);

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsUser()
    {
        var read = new Mock<IFgsUserReadRepository>();
        read.Setup(r => r.GetByIdAsync(UserId, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsUserByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsUserByIdQuery(UserId), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsUserReadRepository>();
        read.Setup(r => r.GetByIdAsync(UserId, It.IsAny<CancellationToken>())).ReturnsAsync((FgsUserDetailDto?)null);

        var handler = new GetFgsUserByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsUserByIdQuery(UserId), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsUserSummaryDto(UserId, "Test User", "user@test.com", null, 1, "Admin", "Accepted", true);
        var paged = new PagedResult<FgsUserSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsUserReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsUserListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsUsersQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsUsersQuery(new IdentityListQuery(), new FgsUserListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedUser()
    {
        var write = new Mock<IFgsUserWriteService>();
        write.Setup(w => w.UpdateAsync(UserId, It.IsAny<FgsUserUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { DisplayName = "Updated User" });

        var handler = new UpdateFgsUserCommandHandler(write.Object, NullLogger<UpdateFgsUserCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsUserCommand(UserId, new FgsUserUpdateDto("Updated User", null, [1], true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.DisplayName.Should().Be("Updated User");
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedUser()
    {
        var write = new Mock<IFgsUserWriteService>();
        write.Setup(w => w.PatchAsync(UserId, It.IsAny<FgsUserPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsUserCommandHandler(write.Object, NullLogger<PatchFgsUserCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsUserCommand(UserId, new FgsUserPatchDto(null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task ResendInviteHandler_ReturnsUser()
    {
        var write = new Mock<IFgsUserWriteService>();
        write.Setup(w => w.ResendInviteAsync(UserId, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new ResendFgsUserInviteCommandHandler(
            write.Object,
            NullLogger<ResendFgsUserInviteCommandHandler>.Instance);
        var response = await handler.Handle(new ResendFgsUserInviteCommand(UserId), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(UserId);
    }

    [Fact]
    public async Task UpdateValidator_RejectsEmptyDisplayName()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        var validator = new UpdateFgsUserCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsUserCommand(UserId, new FgsUserUpdateDto(string.Empty, null, [1], true)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsEmptyDisplayNameWhenProvided()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        var validator = new PatchFgsUserCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsUserCommand(UserId, new FgsUserPatchDto(string.Empty, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}

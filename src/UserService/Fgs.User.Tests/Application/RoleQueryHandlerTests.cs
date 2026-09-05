using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Application.Features.Roles.Queries.GetFgsRoleById;
using Fgs.User.Application.Features.Roles.Queries.ListFgsRoles;
using Fgs.User.Application.Features.Roles.Queries.LookupFgsRoles;
using Fgs.User.Application.Features.Roles.Validators;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RoleQueryHandlerTests
{
    private static readonly FgsRoleDetailDto Detail =
        new(1, "DISPATCHER", "Dispatcher", "Schedules work", null, false, 1, true);

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsRole()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsRoleByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.RoleCode.Should().Be("DISPATCHER");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsRoleDetailDto?)null);

        var handler = new GetFgsRoleByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsRoleSummaryDto(1, "DISPATCHER", "Dispatcher", "Schedules work", null, false, 1, true);
        var paged = new PagedResult<FgsRoleSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsRoleListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsRolesQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsRolesQuery(new IdentityListQuery(), new FgsRoleListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleLookupDto(1, "DISPATCHER", "Dispatcher", false, 1)]);

        var handler = new LookupFgsRolesQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsRolesQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsLowercaseRoleCode()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.ExistsByRoleCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new Fgs.User.Application.Features.Roles.Commands.CreateFgsRole.CreateFgsRoleCommand(
                new FgsRoleCreateDto("dispatcher", "Dispatcher", null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        var validator = new UpdateFgsRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole.UpdateFgsRoleCommand(
                0, new FgsRoleUpdateDto("DISPATCHER", "Dispatcher", null, 1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.ExistsByRoleCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole.UpdateFgsRoleCommand(
                1, new FgsRoleUpdateDto("DISPATCHER", "Dispatcher", null, 1)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsLowercaseRoleCode()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        var validator = new PatchFgsRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new Fgs.User.Application.Features.Roles.Commands.PatchFgsRole.PatchFgsRoleCommand(
                1, new FgsRolePatchDto("dispatcher", null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsRoleReadRepository>();
        read.Setup(r => r.ExistsByRoleCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsRoleCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new Fgs.User.Application.Features.Roles.Commands.PatchFgsRole.PatchFgsRoleCommand(
                1, new FgsRolePatchDto("DISPATCHER", "Dispatcher", null, null, null)));

        result.IsValid.Should().BeTrue();
    }
}

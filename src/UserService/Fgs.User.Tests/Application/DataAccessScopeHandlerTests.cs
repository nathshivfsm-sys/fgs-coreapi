using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccessScopes.Commands.CreateFgsDataAccessScope;
using Fgs.User.Application.Features.DataAccessScopes.Commands.PatchFgsDataAccessScope;
using Fgs.User.Application.Features.DataAccessScopes.Commands.UpdateFgsDataAccessScope;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using Fgs.User.Application.Features.DataAccessScopes.Queries.GetFgsDataAccessScopeById;
using Fgs.User.Application.Features.DataAccessScopes.Queries.ListFgsDataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class DataAccessScopeHandlerTests
{
    private static readonly FgsDataAccessScopeDetailDto Detail =
        new(1, 10, "LOCATION", "IN", "1,2,3", 1);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedScope()
    {
        var write = new Mock<IFgsDataAccessScopeWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsDataAccessScopeCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsDataAccessScopeCommandHandler(
            write.Object,
            NullLogger<CreateFgsDataAccessScopeCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsDataAccessScopeCommand(new FgsDataAccessScopeCreateDto(10, "LOCATION", "IN", "1,2,3")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.ScopeType.Should().Be("LOCATION");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedScope()
    {
        var write = new Mock<IFgsDataAccessScopeWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsDataAccessScopeUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsDataAccessScopeCommandHandler(
            write.Object,
            NullLogger<UpdateFgsDataAccessScopeCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsDataAccessScopeCommand(1, new FgsDataAccessScopeUpdateDto("LOCATION", "IN", "1,2,3", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedScope()
    {
        var write = new Mock<IFgsDataAccessScopeWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsDataAccessScopePatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { ScopeValue = "4,5" });

        var handler = new PatchFgsDataAccessScopeCommandHandler(
            write.Object,
            NullLogger<PatchFgsDataAccessScopeCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsDataAccessScopeCommand(1, new FgsDataAccessScopePatchDto(null, null, "4,5", null)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.ScopeValue.Should().Be("4,5");
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsScope()
    {
        var read = new Mock<IFgsDataAccessScopeReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsDataAccessScopeByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsDataAccessScopeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.ScopeType.Should().Be("LOCATION");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsDataAccessScopeReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsDataAccessScopeDetailDto?)null);

        var handler = new GetFgsDataAccessScopeByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsDataAccessScopeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsDataAccessScopeSummaryDto(1, 10, "LOCATION", "IN", "1,2,3", 1);
        var paged = new PagedResult<FgsDataAccessScopeSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsDataAccessScopeReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsDataAccessScopeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsDataAccessScopesQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsDataAccessScopesQuery(new IdentityListQuery(), new FgsDataAccessScopeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsInvalidDataAccessId()
    {
        var validator = new CreateFgsDataAccessScopeCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsDataAccessScopeCommand(new FgsDataAccessScopeCreateDto(0, "LOCATION", "IN")));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var validator = new CreateFgsDataAccessScopeCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsDataAccessScopeCommand(new FgsDataAccessScopeCreateDto(10, "LOCATION", "IN", "1,2,3")));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var validator = new UpdateFgsDataAccessScopeCommandValidator();
        var result = await validator.ValidateAsync(
            new UpdateFgsDataAccessScopeCommand(0, new FgsDataAccessScopeUpdateDto("LOCATION", "IN", "1,2,3", 1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var validator = new PatchFgsDataAccessScopeCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchFgsDataAccessScopeCommand(0, new FgsDataAccessScopePatchDto("LOCATION", null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}

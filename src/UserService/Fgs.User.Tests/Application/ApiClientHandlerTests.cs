using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiClients.Commands.CreateFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.PatchFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.UpdateFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Dtos;
using Fgs.User.Application.Features.ApiClients.Queries.GetFgsApiClientById;
using Fgs.User.Application.Features.ApiClients.Queries.ListFgsApiClients;
using Fgs.User.Application.Features.ApiClients.Queries.LookupFgsApiClients;
using Fgs.User.Application.Features.ApiClients.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ApiClientHandlerTests
{
    private static readonly FgsApiClientDetailDto Detail =
        new(1, Guid.Parse("11111111-1111-1111-1111-111111111111"), "TestApp", "Desc", "Contact", "c@test.com", 60, true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedClient()
    {
        var write = new Mock<IFgsApiClientWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsApiClientCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsApiClientCommandHandler(write.Object, NullLogger<CreateFgsApiClientCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsApiClientCommand(new FgsApiClientCreateDto("TestApp", "Desc", "Contact", "c@test.com")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.ApplicationName.Should().Be("TestApp");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedClient()
    {
        var write = new Mock<IFgsApiClientWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsApiClientUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsApiClientCommandHandler(write.Object, NullLogger<UpdateFgsApiClientCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsApiClientCommand(1, new FgsApiClientUpdateDto("TestApp", "Desc", "Contact", "c@test.com", 100)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedClient()
    {
        var write = new Mock<IFgsApiClientWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsApiClientPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsApiClientCommandHandler(write.Object, NullLogger<PatchFgsApiClientCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsApiClientCommand(1, new FgsApiClientPatchDto(null, null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsClient()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsApiClientByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiClientByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.ApplicationName.Should().Be("TestApp");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsApiClientDetailDto?)null);

        var handler = new GetFgsApiClientByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiClientByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsApiClientSummaryDto(1, Detail.ClientId, "TestApp", "Desc", "Contact", "c@test.com", 60, true);
        var paged = new PagedResult<FgsApiClientSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsApiClientListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsApiClientsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsApiClientsQuery(new IdentityListQuery(), new FgsApiClientListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsApiClientLookupDto(1, Detail.ClientId, "TestApp")]);

        var handler = new LookupFgsApiClientsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsApiClientsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicateName()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.ExistsByApplicationNameAsync("TestApp", null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var validator = new CreateFgsApiClientCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiClientCommand(new FgsApiClientCreateDto("TestApp", null, null, null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        read.Setup(r => r.ExistsByApplicationNameAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsApiClientCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiClientCommand(new FgsApiClientCreateDto("TestApp", null, null, null)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        var validator = new UpdateFgsApiClientCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsApiClientCommand(0, new FgsApiClientUpdateDto("TestApp", null, null, null, 60)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsApiClientReadRepository>();
        var validator = new PatchFgsApiClientCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsApiClientCommand(0, new FgsApiClientPatchDto("TestApp", null, null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}

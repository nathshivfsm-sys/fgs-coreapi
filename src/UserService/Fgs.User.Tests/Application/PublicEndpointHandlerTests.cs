using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.PatchFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.UpdateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Application.Features.PublicEndpoints.Queries.GetFgsPublicEndpointById;
using Fgs.User.Application.Features.PublicEndpoints.Queries.ListFgsPublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Queries.LookupFgsPublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class PublicEndpointHandlerTests
{
    private static readonly FgsPublicEndpointDetailDto Detail =
        new(1, "API", "PROD", "https://api.example.com", "Prod API", true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedEndpoint()
    {
        var write = new Mock<IFgsPublicEndpointWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsPublicEndpointCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsPublicEndpointCommandHandler(
            write.Object,
            NullLogger<CreateFgsPublicEndpointCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsPublicEndpointCommand(new FgsPublicEndpointCreateDto("API", "PROD", "https://api.example.com", "Prod API")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.EndpointType.Should().Be("API");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedEndpoint()
    {
        var write = new Mock<IFgsPublicEndpointWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsPublicEndpointUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsPublicEndpointCommandHandler(
            write.Object,
            NullLogger<UpdateFgsPublicEndpointCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsPublicEndpointCommand(1, new FgsPublicEndpointUpdateDto("API", "PROD", "https://api.example.com", "Prod API")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedEndpoint()
    {
        var write = new Mock<IFgsPublicEndpointWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsPublicEndpointPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsPublicEndpointCommandHandler(
            write.Object,
            NullLogger<PatchFgsPublicEndpointCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsPublicEndpointCommand(1, new FgsPublicEndpointPatchDto(null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsEndpoint()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsPublicEndpointByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsPublicEndpointByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.BaseUrl.Should().Be("https://api.example.com");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsPublicEndpointDetailDto?)null);

        var handler = new GetFgsPublicEndpointByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsPublicEndpointByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsPublicEndpointSummaryDto(1, "API", "PROD", "https://api.example.com", "Prod API", true);
        var paged = new PagedResult<FgsPublicEndpointSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsPublicEndpointListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsPublicEndpointsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsPublicEndpointsQuery(new IdentityListQuery(), new FgsPublicEndpointListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsPublicEndpointLookupDto(1, "API", "PROD", "https://api.example.com", "Prod API")]);

        var handler = new LookupFgsPublicEndpointsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsPublicEndpointsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        var validator = new UpdateFgsPublicEndpointCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsPublicEndpointCommand(0, new FgsPublicEndpointUpdateDto("API", "PROD", "https://api.example.com", null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsPublicEndpointReadRepository>();
        var validator = new PatchFgsPublicEndpointCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsPublicEndpointCommand(0, new FgsPublicEndpointPatchDto("API", null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}

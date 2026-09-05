using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Commands.CreateFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.PatchFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.RevokeFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using Fgs.User.Application.Features.ApiSecrets.Queries.GetFgsApiSecretById;
using Fgs.User.Application.Features.ApiSecrets.Queries.ListFgsApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Validators;
using Fgs.User.Application.Features.ApiClients.Dtos;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ApiSecretHandlerTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    private static readonly FgsApiSecretDetailDto Detail =
        new(1, 10, "Primary", Now.AddDays(30), null, null, null, true, Now, "test");

    [Fact]
    public async Task CreateHandler_ReturnsCreatedSecret()
    {
        var createResult = new FgsApiSecretCreateResultDto(1, 10, "Primary", "secret-value", Now.AddDays(30), true, Now, "test");
        var write = new Mock<IFgsApiSecretWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsApiSecretCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createResult);

        var handler = new CreateFgsApiSecretCommandHandler(write.Object, NullLogger<CreateFgsApiSecretCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsApiSecretCommand(new FgsApiSecretCreateDto(10, "Primary", Now.AddDays(30))),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Secret.Should().Be("secret-value");
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedSecret()
    {
        var write = new Mock<IFgsApiSecretWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsApiSecretPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsApiSecretCommandHandler(write.Object, NullLogger<PatchFgsApiSecretCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsApiSecretCommand(1, new FgsApiSecretPatchDto(null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task RevokeHandler_ReturnsRevokedSecret()
    {
        var write = new Mock<IFgsApiSecretWriteService>();
        write.Setup(w => w.RevokeAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { RevokedOn = Now, RevokedBy = "admin" });

        var handler = new RevokeFgsApiSecretCommandHandler(write.Object, NullLogger<RevokeFgsApiSecretCommandHandler>.Instance);
        var response = await handler.Handle(new RevokeFgsApiSecretCommand(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.RevokedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsSecret()
    {
        var read = new Mock<IFgsApiSecretReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsApiSecretByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiSecretByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Primary");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsApiSecretReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsApiSecretDetailDto?)null);

        var handler = new GetFgsApiSecretByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiSecretByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsApiSecretSummaryDto(1, 10, "Primary", Now.AddDays(30), null, null, true, Now, "test");
        var paged = new PagedResult<FgsApiSecretSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsApiSecretReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsApiSecretListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsApiSecretsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsApiSecretsQuery(new IdentityListQuery(), new FgsApiSecretListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsMissingClient()
    {
        var secretRead = new Mock<IFgsApiSecretReadRepository>();
        var clientRead = new Mock<IFgsApiClientReadRepository>();
        clientRead.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync((FgsApiClientDetailDto?)null);

        var validator = new CreateFgsApiSecretCommandValidator(secretRead.Object, clientRead.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiSecretCommand(new FgsApiSecretCreateDto(10, "Primary", null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var secretRead = new Mock<IFgsApiSecretReadRepository>();
        secretRead.Setup(r => r.ExistsByNameAsync(10, "Primary", null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var clientRead = new Mock<IFgsApiClientReadRepository>();
        clientRead.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsApiClientDetailDto(10, Guid.NewGuid(), "App", null, null, null, 60, true));

        var validator = new CreateFgsApiSecretCommandValidator(secretRead.Object, clientRead.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiSecretCommand(new FgsApiSecretCreateDto(10, "Primary", null)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsApiSecretReadRepository>();
        var validator = new PatchFgsApiSecretCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsApiSecretCommand(0, new FgsApiSecretPatchDto("Primary", null, null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task RevokeValidator_RejectsInvalidId()
    {
        var validator = new RevokeFgsApiSecretCommandValidator();
        var result = await validator.ValidateAsync(new RevokeFgsApiSecretCommand(0));

        result.IsValid.Should().BeFalse();
    }
}

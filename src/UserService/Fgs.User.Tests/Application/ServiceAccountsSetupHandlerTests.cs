using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetupById;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.ListFgsTenantServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.LookupFgsTenantServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ServiceAccountsSetupHandlerTests
{
    private static readonly FgsTenantServiceAccountsSetupDetailDto Detail =
        new(10, 1, 100, 101, 102, null, null, null, null, null, null, null, true);

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedSetup()
    {
        var write = new Mock<IFgsTenantServiceAccountsSetupWriteService>();
        write.Setup(w => w.UpdateAsync(It.IsAny<FgsTenantServiceAccountsSetupUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsTenantServiceAccountsSetupCommandHandler(
            write.Object,
            NullLogger<UpdateFgsTenantServiceAccountsSetupCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsTenantServiceAccountsSetupCommand(new FgsTenantServiceAccountsSetupUpdateDto(100, 101, 102, null, null, null, null, null, null, null, true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.BankAccountId.Should().Be(100);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedSetup()
    {
        var write = new Mock<IFgsTenantServiceAccountsSetupWriteService>();
        write.Setup(w => w.PatchAsync(It.IsAny<FgsTenantServiceAccountsSetupPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsTenantServiceAccountsSetupCommandHandler(
            write.Object,
            NullLogger<PatchFgsTenantServiceAccountsSetupCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsTenantServiceAccountsSetupCommand(new FgsTenantServiceAccountsSetupPatchDto(IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsValidPayload()
    {
        var validator = new UpdateFgsTenantServiceAccountsSetupCommandValidator();
        var result = await validator.ValidateAsync(
            new UpdateFgsTenantServiceAccountsSetupCommand(new FgsTenantServiceAccountsSetupUpdateDto(100, null, null, null, null, null, null, null, null, null, true)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task GetHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTenantServiceAccountsSetupDetailDto?)null);

        var handler = new GetFgsTenantServiceAccountsSetupQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceAccountsSetupQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetHandler_WhenFound_ReturnsSetup()
    {
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsTenantServiceAccountsSetupQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceAccountsSetupQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TenantId.Should().Be(10);
    }

    [Fact]
    public async Task GetByIdHandler_WhenCompanyMatches_ReturnsSetup()
    {
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsTenantServiceAccountsSetupByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceAccountsSetupByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.CompanyId.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdHandler_WhenCompanyDoesNotMatch_ReturnsNotFound()
    {
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsTenantServiceAccountsSetupByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceAccountsSetupByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedSetup()
    {
        var summary = new FgsTenantServiceAccountsSetupSummaryDto(
            10, 1, 100, 101, 102, null, null, null, null, null, null, null, true);
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsTenantServiceAccountsSetupSummaryDto>([summary], 1, 25, 1));

        var handler = new ListFgsTenantServiceAccountsSetupsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsTenantServiceAccountsSetupsQuery(new IdentityListQuery()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TotalCount.Should().Be(1);
        response.Data.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookupRows()
    {
        var read = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FgsTenantServiceAccountsSetupLookupDto> { new(1, true) });

        var handler = new LookupFgsTenantServiceAccountsSetupsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsTenantServiceAccountsSetupsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
        response.Data![0].CompanyId.Should().Be(1);
    }

    [Fact]
    public async Task PatchValidator_AcceptsValidPayload()
    {
        var validator = new PatchFgsTenantServiceAccountsSetupCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchFgsTenantServiceAccountsSetupCommand(new FgsTenantServiceAccountsSetupPatchDto(BankAccountId: 100)));

        result.IsValid.Should().BeTrue();
    }
}

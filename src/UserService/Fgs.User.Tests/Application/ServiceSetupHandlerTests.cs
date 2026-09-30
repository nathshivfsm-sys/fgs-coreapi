using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetupById;
using Fgs.User.Application.Features.ServiceSetups.Queries.ListFgsTenantServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Queries.LookupFgsTenantServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Validators;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ServiceSetupHandlerTests
{
    private static readonly FgsTenantServiceSetupDetailDto Detail = new(
        10, 1, TimeCardOption.None, null, false, false, false, false, false, false,
        null, null, null, null, null, "ARRIVE", false, false, 100, 100, 100, 100,
        null, null, null, null, null, EstimateRevisionCreationModes.OnDemand, true);

    [Fact]
    public async Task GetHandler_WhenFound_ReturnsSetup()
    {
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsTenantServiceSetupQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceSetupQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TenantId.Should().Be(10);
    }

    [Fact]
    public async Task GetByIdHandler_WhenCompanyMatches_ReturnsSetup()
    {
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsTenantServiceSetupByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceSetupByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.CompanyId.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTenantServiceSetupDetailDto?)null);

        var handler = new GetFgsTenantServiceSetupByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceSetupByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedSetup()
    {
        var summary = new FgsTenantServiceSetupSummaryDto(
            10, 1, TimeCardOption.None, null, false, false, false, false, false, false,
            null, null, null, null, null, "ARRIVE", false, false, 100, 100, 100, 100,
            null, null, null, null, null, EstimateRevisionCreationModes.OnDemand, true);
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsTenantServiceSetupSummaryDto>([summary], 1, 25, 1));

        var handler = new ListFgsTenantServiceSetupsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsTenantServiceSetupsQuery(new IdentityListQuery()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookupRows()
    {
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FgsTenantServiceSetupLookupDto> { new(1, true) });

        var handler = new LookupFgsTenantServiceSetupsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsTenantServiceSetupsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data![0].IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsTenantServiceSetupReadRepository>();
        read.Setup(r => r.GetCurrentAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTenantServiceSetupDetailDto?)null);

        var handler = new GetFgsTenantServiceSetupQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsTenantServiceSetupQuery(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedSetup()
    {
        var write = new Mock<IFgsTenantServiceSetupWriteService>();
        write.Setup(w => w.UpdateAsync(It.IsAny<FgsTenantServiceSetupUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsTenantServiceSetupCommandHandler(
            write.Object,
            NullLogger<UpdateFgsTenantServiceSetupCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsTenantServiceSetupCommand(ValidUpdateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.BillHoursFromDispatchOrArrive.Should().Be("ARRIVE");
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedSetup()
    {
        var write = new Mock<IFgsTenantServiceSetupWriteService>();
        write.Setup(w => w.PatchAsync(It.IsAny<FgsTenantServiceSetupPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { EnableCustomerPortal = true });

        var handler = new PatchFgsTenantServiceSetupCommandHandler(
            write.Object,
            NullLogger<PatchFgsTenantServiceSetupCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsTenantServiceSetupCommand(new FgsTenantServiceSetupPatchDto(EnableCustomerPortal: true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.EnableCustomerPortal.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsValidPayload()
    {
        var validator = new PatchFgsTenantServiceSetupCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchFgsTenantServiceSetupCommand(new FgsTenantServiceSetupPatchDto(BillHoursFromDispatchOrArrive: "DISPATCH")));

        result.IsValid.Should().BeTrue();
    }

    private static FgsTenantServiceSetupUpdateDto ValidUpdateDto() =>
        new(
            TimeCardOption.None,
            null,
            false,
            false,
            false,
            false,
            false,
            false,
            null,
            null,
            null,
            null,
            null,
            "ARRIVE",
            false,
            false,
            100,
            100,
            100,
            100,
            null,
            null,
            null,
            null,
            null,
            EstimateRevisionCreationModes.OnDemand,
            true);
}

using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetFgsSetupCommunicationTemplateById;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListCommunicationTemplates;
using Moq;

namespace Fgs.Setup.Tests.CommunicationTemplates;

public sealed class FgsSetupCommunicationTemplateQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupCommunicationTemplateDetailDto(1, 10L, 20L, "Email", "TemplateType value", "Code value", "Name value", "Subject value", "Body value", true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupCommunicationTemplateReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsSetupCommunicationTemplateByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSetupCommunicationTemplateByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupCommunicationTemplateReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupCommunicationTemplateDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsSetupCommunicationTemplateByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSetupCommunicationTemplateByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupCommunicationTemplateReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupCommunicationTemplateListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupCommunicationTemplateSummaryDto>([], 1, 25, 0));

        var handler = new ListCommunicationTemplatesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListCommunicationTemplatesQuery(new SetupListQuery(), new FgsSetupCommunicationTemplateListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}

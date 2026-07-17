using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.LookupResolutionCodes;
using Moq;

namespace Fgs.Setup.Tests.ResolutionCodes;

public sealed class ResolutionCodeLookupQueryHandlerTests
{
    [Fact]
    public async Task Lookup_PassesMobileVisibilityFilterToRepository()
    {
        var readRepository = new Mock<IResolutionCodeReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ResolutionCodeLookupDto> { new(1, "DONE", "Done") });

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<ResolutionCodeLookupDto>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns(async (string _, Func<Task<IReadOnlyList<ResolutionCodeLookupDto>>> factory, TimeSpan? _, CancellationToken __) =>
                (IReadOnlyList<ResolutionCodeLookupDto>?)await factory());

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new LookupResolutionCodesQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);

        var response = await handler.Handle(
            new LookupResolutionCodesQuery(true, true),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        readRepository.Verify(r => r.LookupAsync(true, true, It.IsAny<CancellationToken>()), Times.Once);
    }
}

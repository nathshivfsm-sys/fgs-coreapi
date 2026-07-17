using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.LookupSetupPaymentMethods;
using Moq;

namespace Fgs.Setup.Tests.SetupPaymentMethods;

public sealed class FgsSetupPaymentMethodLookupQueryHandlerTests
{
    [Fact]
    public async Task Lookup_PassesVisibilityFiltersToRepository()
    {
        var readRepository = new Mock<IFgsSetupPaymentMethodReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, true, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FgsSetupPaymentMethodLookupDto> { new(1, "Card", 1) });

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns(async (string _, Func<Task<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>> factory, TimeSpan? _, CancellationToken __) =>
                (IReadOnlyList<FgsSetupPaymentMethodLookupDto>?)await factory());

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new LookupSetupPaymentMethodsQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);

        var response = await handler.Handle(
            new LookupSetupPaymentMethodsQuery(true, true, false),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        readRepository.Verify(r => r.LookupAsync(true, true, false, It.IsAny<CancellationToken>()), Times.Once);
    }
}

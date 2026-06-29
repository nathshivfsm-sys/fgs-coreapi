using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using Fgs.Setup.Application.Features.SetupTimeSlots.Queries.LookupSetupTimeSlots;
using Moq;

namespace Fgs.Setup.Tests.SetupTimeSlots;

public sealed class FgsSetupTimeSlotLookupQueryHandlerTests
{
    [Fact]
    public async Task Lookup_PassesVisibilityFiltersToRepository()
    {
        var readRepository = new Mock<IFgsSetupTimeSlotReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, true, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FgsSetupTimeSlotLookupDto> { new(1, "MORNING", "Morning") });

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<FgsSetupTimeSlotLookupDto>?>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns((string _, Func<Task<IReadOnlyList<FgsSetupTimeSlotLookupDto>?>> factory, TimeSpan? _, CancellationToken __) => factory());

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new LookupSetupTimeSlotsQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);

        var response = await handler.Handle(
            new LookupSetupTimeSlotsQuery(true, true, true),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        readRepository.Verify(r => r.LookupAsync(true, true, true, It.IsAny<CancellationToken>()), Times.Once);
    }
}

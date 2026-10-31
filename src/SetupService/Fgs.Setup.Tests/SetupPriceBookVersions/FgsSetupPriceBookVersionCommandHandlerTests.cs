using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Time;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Entities.SetupPriceBookVersions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupPriceBookVersions;

public sealed class FgsSetupPriceBookVersionCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesVersionAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var cache = new Mock<ICacheService>();
        var handler = new CreateFgsSetupPriceBookVersionCommandHandler(
            CreateWriteService(context),
            cache.Object,
            CreateTenantAccessor(),
            NullLogger<CreateFgsSetupPriceBookVersionCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupPriceBookVersionCommand(
                new FgsSetupPriceBookVersionCreateDto("2026-Q1", new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 31))),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Version.Should().Be("2026-Q1");
        response.Data.IsActive.Should().BeTrue();
        context.FgsSetupPriceBookVersions.Should().ContainSingle();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "pricebookversion"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_UpdatesFieldsAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(
            new FgsSetupPriceBookVersionCreateDto("2026-Q1", new DateOnly(2026, 1, 1)));
        var cache = new Mock<ICacheService>();
        var handler = new UpdateFgsSetupPriceBookVersionCommandHandler(
            service,
            cache.Object,
            CreateTenantAccessor(),
            NullLogger<UpdateFgsSetupPriceBookVersionCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsSetupPriceBookVersionCommand(
                created.Id,
                new FgsSetupPriceBookVersionUpdateDto("2026-H1", new DateOnly(2026, 1, 1), new DateOnly(2026, 6, 30))),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Version.Should().Be("2026-H1");
        response.Data.EffectiveTo.Should().Be(new DateOnly(2026, 6, 30));
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "pricebookversion"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHandler_SoftDeactivatesAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(
            new FgsSetupPriceBookVersionCreateDto("2026-Q2", new DateOnly(2026, 4, 1)));
        var cache = new Mock<ICacheService>();
        var handler = new PatchFgsSetupPriceBookVersionCommandHandler(
            service,
            cache.Object,
            CreateTenantAccessor(),
            NullLogger<PatchFgsSetupPriceBookVersionCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsSetupPriceBookVersionCommand(
                created.Id,
                new FgsSetupPriceBookVersionPatchDto(IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "pricebookversion"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static FgsSetupPriceBookVersionWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            CreateTenantAccessor(),
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new FgsSetupPriceBookVersionWriteService(context, unitOfWork, auditHelper);
    }

    private static ITenantContextAccessor CreateTenantAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext
            {
                TenantId = TenantId,
                CompanyId = CompanyId
            }
        };

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, CreateTenantAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Entities.SetupPricingMatrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;
    private const int LaborRateTypeId = 1;

    [Fact]
    public async Task CreateHandler_WithTierLabor_ReturnsCreatedAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        await SeedLaborRateTypeAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsSetupPricingMatrixCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupPricingMatrixCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupPricingMatrixCommand(
                new FgsSetupPricingMatrixCreateDto(
                    "TIERED",
                    "Tiered labor matrix",
                    false,
                    true,
                    false,
                    null,
                    null,
                    null,
                    true,
                    [
                        new FgsSetupPricingMatrixLaborLineDto(
                            null,
                            LaborRateTypeId,
                            null,
                            null,
                            null,
                            null,
                            null,
                            [new FgsSetupPricingMatrixLaborTierItemDto(null, 1, 60, 50m, null)])
                    ],
                    null,
                    null)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Code.Should().Be("TIERED");
        (await context.FgsSetupPricingMatrixLaborTiers.CountAsync(t => t.IsActive)).Should().Be(1);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "pricingmatrix"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_WhenSwitchingMarkupBranch_DeactivatesOppositeChildren()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsSetupPricingMatrixCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupPricingMatrixCommandHandler>.Instance);
        var updateHandler = new UpdateFgsSetupPricingMatrixCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsSetupPricingMatrixCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSetupPricingMatrixCommand(
                new FgsSetupPricingMatrixCreateDto(
                    "MARKUP",
                    "Markup matrix",
                    false,
                    false,
                    false,
                    1,
                    null,
                    null,
                    true,
                    null,
                    [new FgsSetupPricingMatrixMaterialTierDto(null, 0m, null, 10m)],
                    null)),
            CancellationToken.None);

        var response = await updateHandler.Handle(
            new UpdateFgsSetupPricingMatrixCommand(
                created.Data!.Id,
                new FgsSetupPricingMatrixUpdateDto(
                    "MARKUP",
                    "Markup matrix",
                    false,
                    false,
                    false,
                    1,
                    null,
                    null,
                    true,
                    null,
                    null,
                    [new FgsSetupPricingMatrixOtherItemDto(null, "OT", "Other", 15m, null)])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        context.FgsSetupPricingMatrixMaterialTiers.Any(t => t.IsActive).Should().BeFalse();
        context.FgsSetupPricingMatrixOthers.Count(o => o.IsActive).Should().Be(1);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsSetupPricingMatrixWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = CreateTenantContextAccessor();
        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);

        var readRepository = new Mock<IFgsSetupPricingMatrixReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns<long, CancellationToken>((id, _) =>
            {
                var matrix = context.FgsSetupPricingMatrices.AsNoTracking().First(m => m.Id == id);
                return Task.FromResult<FgsSetupPricingMatrixDetailDto?>(new FgsSetupPricingMatrixDetailDto(
                    matrix.Id,
                    matrix.Code,
                    matrix.Name,
                    matrix.IsDefault,
                    matrix.IsLaborTierStructure,
                    matrix.IsLaborRateBySkillLevel,
                    (short)matrix.PriceAdjustmentTypeId,
                    matrix.EffectiveFrom,
                    matrix.EffectiveTo,
                    matrix.IsMobileVisible,
                    matrix.IsActive,
                    [],
                    [],
                    []));
            });

        return new FgsSetupPricingMatrixWriteService(
            context,
            unitOfWork,
            auditHelper,
            readRepository.Object);
    }

    private static async Task SeedLaborRateTypeAsync(FgsSetupDbContext context)
    {
        context.FgsSetupLaborRateTypes.Add(new Domain.Entities.FgsSetupLaborRateType
        {
            Id = LaborRateTypeId,
            Name = "Standard",
            SortOrder = 1,
            TenantId = TenantId,
            CompanyId = CompanyId,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedBy = "test",
            UpdatedBy = "test"
        });
        await context.SaveChangesAsync();
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = CreateTenantContextAccessor();
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, accessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

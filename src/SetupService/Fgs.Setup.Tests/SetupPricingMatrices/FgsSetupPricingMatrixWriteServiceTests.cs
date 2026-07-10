using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Entities.SetupPricingMatrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixWriteServiceTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;
    private const int LaborRateTypeId = 1;
    private const long TechSkillLevelId = 100;

    [Fact]
    public async Task CreateAsync_WithFlatLabor_CreatesLaborWithoutTiers()
    {
        await using var context = await CreateContextAsync();
        await SeedLaborRateTypeAsync(context);
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(
            BuildCreateDto(
                isLaborTierStructure: false,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null,
                        LaborRateTypeId,
                        null,
                        75m,
                        1.5m,
                        2m,
                        10m,
                        null)
                ]),
            CancellationToken.None);

        var labor = await context.FgsSetupPricingMatrixLabors.SingleAsync();
        labor.BaseRate.Should().Be(75m);
        labor.OvertimeMultiplier.Should().Be(1.5m);
        context.FgsSetupPricingMatrixLaborTiers.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_WithTierLabor_CreatesLaborTiers()
    {
        await using var context = await CreateContextAsync();
        await SeedLaborRateTypeAsync(context);
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(
            BuildCreateDto(
                isLaborTierStructure: true,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null,
                        LaborRateTypeId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        [
                            new FgsSetupPricingMatrixLaborTierItemDto(null, 1, 60, 50m, null),
                            new FgsSetupPricingMatrixLaborTierItemDto(null, 2, 120, 45m, null)
                        ])
                ]),
            CancellationToken.None);

        var labor = await context.FgsSetupPricingMatrixLabors.SingleAsync();
        labor.BaseRate.Should().Be(0);
        var tiers = await context.FgsSetupPricingMatrixLaborTiers.Where(t => t.IsActive).ToListAsync();
        tiers.Should().HaveCount(2);
        tiers.Select(t => t.SequenceOrder).Should().BeEquivalentTo(new short[] { 1, 2 });
    }

    [Fact]
    public async Task CreateAsync_WithMaterialTiers_PersistsMaterialTiersOnly()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(
            BuildCreateDto(
                materialTiers:
                [
                    new FgsSetupPricingMatrixMaterialTierDto(null, 0m, 100m, 15m),
                    new FgsSetupPricingMatrixMaterialTierDto(null, 100m, null, 10m)
                ],
                priceAdjustmentTypeId: 1),
            CancellationToken.None);

        context.FgsSetupPricingMatrixMaterialTiers.Count(t => t.IsActive).Should().Be(2);
        context.FgsSetupPricingMatrixOthers.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_WithOtherItems_PersistsOtherItemsOnly()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        await writeService.CreateAsync(
            BuildCreateDto(
                otherItems:
                [
                    new FgsSetupPricingMatrixOtherItemDto(null, "NI", "Non-Inventory markup", 20m, null)
                ],
                priceAdjustmentTypeId: 1),
            CancellationToken.None);

        context.FgsSetupPricingMatrixOthers.Count(o => o.IsActive).Should().Be(1);
        context.FgsSetupPricingMatrixMaterialTiers.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_FlipFromMaterialToOther_DeactivatesMaterialTiers()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        var created = await writeService.CreateAsync(
            BuildCreateDto(
                materialTiers: [new FgsSetupPricingMatrixMaterialTierDto(null, 0m, null, 10m)],
                priceAdjustmentTypeId: 1),
            CancellationToken.None);

        await writeService.UpdateAsync(
            created.Id,
            BuildUpdateDto(
                otherItems: [new FgsSetupPricingMatrixOtherItemDto(null, "OT", "Other", 15m, null)],
                priceAdjustmentTypeId: 1),
            CancellationToken.None);

        context.FgsSetupPricingMatrixMaterialTiers.Any(t => t.IsActive).Should().BeFalse();
        context.FgsSetupPricingMatrixOthers.Count(o => o.IsActive).Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_FlipFromFlatToTierLabor_DeactivatesFlatRatesAndCreatesTiers()
    {
        await using var context = await CreateContextAsync();
        await SeedLaborRateTypeAsync(context);
        var writeService = CreateWriteService(context);

        var created = await writeService.CreateAsync(
            BuildCreateDto(
                isLaborTierStructure: false,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, LaborRateTypeId, null, 80m, 1.5m, 2m, null, null)
                ]),
            CancellationToken.None);

        var laborId = await context.FgsSetupPricingMatrixLabors.Select(l => l.Id).SingleAsync();

        await writeService.UpdateAsync(
            created.Id,
            BuildUpdateDto(
                isLaborTierStructure: true,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        laborId,
                        LaborRateTypeId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        [new FgsSetupPricingMatrixLaborTierItemDto(null, 1, 60, 55m, null)])
                ]),
            CancellationToken.None);

        var labor = await context.FgsSetupPricingMatrixLabors.SingleAsync();
        labor.BaseRate.Should().Be(0);
        labor.OvertimeMultiplier.Should().BeNull();
        context.FgsSetupPricingMatrixLaborTiers.Count(t => t.IsActive).Should().Be(1);
    }

    [Fact]
    public async Task PatchAsync_WhenDisablingSkillLevel_ClearsTechSkillLevelIds()
    {
        await using var context = await CreateContextAsync();
        await SeedLaborRateTypeAsync(context);
        await SeedTechSkillLevelAsync(context);
        var writeService = CreateWriteService(context);

        var created = await writeService.CreateAsync(
            BuildCreateDto(
                isLaborRateBySkillLevel: true,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, LaborRateTypeId, TechSkillLevelId, 90m, null, null, null, null)
                ]),
            CancellationToken.None);

        await writeService.PatchAsync(
            created.Id,
            new FgsSetupPricingMatrixPatchDto(
                null, null, null, null, false, null, null, null, null, null),
            CancellationToken.None);

        var labor = await context.FgsSetupPricingMatrixLabors.SingleAsync();
        labor.TechSkillLevelId.Should().BeNull();
    }

    private static FgsSetupPricingMatrixCreateDto BuildCreateDto(
        bool isLaborTierStructure = false,
        bool isLaborRateBySkillLevel = false,
        short? priceAdjustmentTypeId = null,
        IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? laborLines = null,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers = null,
        IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems = null) =>
        new(
            "MATRIX1",
            "Test pricing matrix",
            false,
            isLaborTierStructure,
            isLaborRateBySkillLevel,
            priceAdjustmentTypeId,
            null,
            null,
            true,
            laborLines,
            materialTiers,
            otherItems);

    private static FgsSetupPricingMatrixUpdateDto BuildUpdateDto(
        bool isLaborTierStructure = false,
        bool isLaborRateBySkillLevel = false,
        short? priceAdjustmentTypeId = null,
        IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? laborLines = null,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers = null,
        IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems = null) =>
        new(
            "MATRIX1",
            "Test pricing matrix",
            false,
            isLaborTierStructure,
            isLaborRateBySkillLevel,
            priceAdjustmentTypeId,
            null,
            null,
            true,
            laborLines,
            materialTiers,
            otherItems);

    private static FgsSetupPricingMatrixWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

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
        context.FgsSetupLaborRateTypes.Add(new FgsSetupLaborRateType
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

    private static async Task SeedTechSkillLevelAsync(FgsSetupDbContext context)
    {
        context.FgsSetupTechSkillLevels.Add(new FgsSetupTechSkillLevel
        {
            Id = TechSkillLevelId,
            Code = "JOURNEY",
            Name = "Journeyman",
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
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

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

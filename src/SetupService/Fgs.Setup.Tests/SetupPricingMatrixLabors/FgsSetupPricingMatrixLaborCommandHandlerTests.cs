using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.CreateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.DeleteFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrixLabors;

public sealed class FgsSetupPricingMatrixLaborCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesLaborUnderMatrixAndInvalidatesCache()
    {
        await using var context = await CreateContextAsync();
        var matrixId = await SeedReferencesAsync(context);
        var cache = new Mock<ICacheService>();
        var handler = new CreateFgsSetupPricingMatrixLaborCommandHandler(
            CreateWriteService(context), cache.Object, CreateTenantAccessor(),
            NullLogger<CreateFgsSetupPricingMatrixLaborCommandHandler>.Instance);

        var response = await handler.Handle(new(
            new FgsSetupPricingMatrixLaborCreateDto(matrixId, 1, null, 75m, 1.5m, 2m, 10m)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.PricingMatrixId.Should().Be(matrixId);
        context.FgsSetupPricingMatrixLabors.Should().ContainSingle(x => x.IsActive);
        cache.Verify(c => c.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(TenantId, CompanyId, "pricingmatrixlabor"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletesLabor()
    {
        await using var context = await CreateContextAsync();
        var matrixId = await SeedReferencesAsync(context);
        var service = CreateWriteService(context);
        var created = await service.CreateAsync(new(matrixId, 1, null, 75m, null, null, null));
        var handler = new DeleteFgsSetupPricingMatrixLaborCommandHandler(
            service, new Mock<ICacheService>().Object, CreateTenantAccessor(),
            NullLogger<DeleteFgsSetupPricingMatrixLaborCommandHandler>.Instance);

        var response = await handler.Handle(new(created.Id), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
        (await context.FgsSetupPricingMatrixLabors.FindAsync(created.Id))!.IsActive.Should().BeFalse();
    }

    private static async Task<long> SeedReferencesAsync(FgsSetupDbContext context)
    {
        var matrix = new FgsSetupPricingMatrix
        {
            Code = "MATRIX", Name = "Matrix", IsLaborTierStructure = false,
            IsLaborRateBySkillLevel = false, PriceAdjustmentTypeId = PriceAdjustmentType.MarkupPercent,
            EffectiveFrom = new DateOnly(2026, 1, 1), TenantId = TenantId, CompanyId = CompanyId,
            IsActive = true, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow,
            CreatedBy = "test", UpdatedBy = "test"
        };
        context.FgsSetupPricingMatrices.Add(matrix);
        context.FgsSetupLaborRateTypes.Add(new FgsSetupLaborRateType
        {
            Id = 1, Name = "Standard", SortOrder = 1, TenantId = TenantId, CompanyId = CompanyId,
            IsActive = true, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow,
            CreatedBy = "test", UpdatedBy = "test"
        });
        await context.SaveChangesAsync();
        return matrix.Id;
    }

    private static FgsSetupPricingMatrixLaborWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(x => x.TenantId).Returns(TenantId);
        user.SetupGet(x => x.CompanyId).Returns(CompanyId);
        user.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var audit = new SetupEntityAuditHelper(user.Object, CreateTenantAccessor(), new DateTimeProvider());
        return new(context, new EfUnitOfWork<FgsSetupDbContext>(context), audit);
    }

    private static ITenantContextAccessor CreateTenantAccessor() => new TestTenantContextAccessor
    {
        Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
    };

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new FgsSetupDbContext(options, CreateTenantAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

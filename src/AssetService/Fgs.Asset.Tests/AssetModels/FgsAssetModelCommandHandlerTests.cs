using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.AssetModels;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Asset.Tests.AssetModels;

public sealed class FgsAssetModelCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        await SeedCatalogAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsAssetModelCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor);

        var response = await handler.Handle(
            new CreateFgsAssetModelCommand(new FgsAssetModelCreateDto(1, 1, "58MCA", "Carrier Infinity Model")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PatchHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        await SeedCatalogAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsAssetModelCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor);
        var patchHandler = new PatchFgsAssetModelCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor);

        var created = await createHandler.Handle(
            new CreateFgsAssetModelCommand(new FgsAssetModelCreateDto(1, 1, "58MCA", "Carrier Infinity Model")),
            CancellationToken.None);

        var response = await patchHandler.Handle(
            new PatchFgsAssetModelCommand(created.Data!.Id, new FgsAssetModelPatchDto(null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static async Task SeedCatalogAsync(FgsAssetDbContext context)
    {
        context.FgsAssetTypes.Add(new FgsAssetType
        {
            Id = 1,
            TenantId = TenantId,
            CompanyId = CompanyId,
            Code = "HVAC",
            Name = "HVAC",
            IsActive = true
        });
        context.FgsAssetManufacturers.Add(new FgsAssetManufacturer
        {
            Id = 1,
            TenantId = TenantId,
            CompanyId = CompanyId,
            Code = "CARRIER",
            Name = "Carrier",
            IsActive = true
        });
        await context.SaveChangesAsync();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor { Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId } };

    private static FgsAssetModelWriteService CreateWriteService(FgsAssetDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var auditHelper = new AssetEntityAuditHelper(
            userContext.Object,
            CreateTenantContextAccessor(),
            new DateTimeProvider());
        return new FgsAssetModelWriteService(context, new EfUnitOfWork<FgsAssetDbContext>(context), auditHelper);
    }

    private static async Task<FgsAssetDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsAssetDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new FgsAssetDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

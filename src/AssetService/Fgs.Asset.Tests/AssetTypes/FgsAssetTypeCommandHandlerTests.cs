using Fgs.Asset.Application.Features.AssetTypes.Commands.CreateFgsAssetType;
using Fgs.Asset.Application.Features.AssetTypes.Commands.PatchFgsAssetType;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Asset.Infrastructure.AssetTypes;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Asset.Tests.AssetTypes;

public sealed class FgsAssetTypeCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var handler = new CreateFgsAssetTypeCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantAccessor(),
            NullLogger<CreateFgsAssetTypeCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsAssetTypeCommand(new FgsAssetTypeCreateDto("CODE01", "Test Asset Type", null)),
            CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PatchHandler_SoftDeletesViaIsActive()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantAccessor();
        var created = await new CreateFgsAssetTypeCommandHandler(
            writeService, cache.Object, tenantAccessor, NullLogger<CreateFgsAssetTypeCommandHandler>.Instance)
            .Handle(new CreateFgsAssetTypeCommand(new FgsAssetTypeCreateDto("CODE01", "Test", null)), CancellationToken.None);
        var response = await new PatchFgsAssetTypeCommandHandler(
            writeService, cache.Object, tenantAccessor, NullLogger<PatchFgsAssetTypeCommandHandler>.Instance)
            .Handle(new PatchFgsAssetTypeCommand(created.Data!.Id, new FgsAssetTypePatchDto(null, null, null, false)), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantAccessor() =>
        new TestTenantContextAccessor { Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId } };

    private static FgsAssetTypeWriteService CreateWriteService(FgsAssetDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var tenantAccessor = CreateTenantAccessor();
        var auditHelper = new AssetEntityAuditHelper(userContext.Object, tenantAccessor, new DateTimeProvider());
        return new FgsAssetTypeWriteService(context, new EfUnitOfWork<FgsAssetDbContext>(context), auditHelper);
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

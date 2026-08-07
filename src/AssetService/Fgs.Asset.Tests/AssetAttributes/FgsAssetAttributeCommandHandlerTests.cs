using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Infrastructure.AssetAttributes;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Fgs.MultiTenancy.Persistence;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeCommandHandlerTests
{
  [Fact] public async Task CreateHandler_PersistsRecord() { var opts = new DbContextOptionsBuilder<FgsAssetDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options; await using var ctx = new FgsAssetDbContext(opts, new DesignTimeTenantContextAccessor()); await ctx.Database.EnsureCreatedAsync(); var uc = new Mock<IFgsUserContext>(); uc.SetupGet(x => x.TenantId).Returns(10L); uc.SetupGet(x => x.CompanyId).Returns(20L); uc.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111")); var ta = new TestTenant { Current = new TenantContext { TenantId = 10, CompanyId = 20 } }; var ws = new FgsAssetAttributeWriteService(ctx, new EfUnitOfWork<FgsAssetDbContext>(ctx), new AssetEntityAuditHelper(uc.Object, ta, new DateTimeProvider())); var h = new CreateFgsAssetAttributeCommandHandler(ws, new Mock<ICacheService>().Object, ta); var res = await h.Handle(new CreateFgsAssetAttributeCommand(new FgsAssetAttributeCreateDto(1, "CODE", "Name", "TEXT", null, null, null, null, null, null, false, true, 0)), CancellationToken.None); res.Success.Should().BeTrue(); }
  private sealed class TestTenant : ITenantContextAccessor { public ITenantContext? Current { get; set; } }
}

using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.DeleteFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.SetupTaxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxes;

public sealed class FgsSetupTaxCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "taxes"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateHandler_WithTaxDetails_InsertsLines()
    {
        await using var context = await CreateContextAsync();
        var authority = await SeedTaxAuthorityAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto(
                "TAX2",
                "With lines",
                false,
                null,
                null,
                true,
                null,
                [
                    new FgsSetupTaxLineUpsertDto(null, authority.Id, new DateOnly(2026, 1, 1), null)
                ])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TaxDetails.Should().HaveCount(1);
        response.Data.TaxDetails[0].FgsSetupTaxAuthorityId.Should().Be(authority.Id);
        response.Data.TaxDetails[0].IsActive.Should().BeTrue();

        var persisted = await context.FgsSetupTaxDetails.CountAsync(d => d.FgsSetupTaxId == response.Data.Id && d.IsActive);
        persisted.Should().Be(1);
    }

    [Fact]
    public async Task UpdateHandler_OmitsActiveLine_SoftDeactivates()
    {
        await using var context = await CreateContextAsync();
        var authority = await SeedTaxAuthorityAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);
        var updateHandler = new UpdateFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsSetupTaxCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto(
                "TAX3",
                "Sync lines",
                false,
                null,
                null,
                true,
                null,
                [
                    new FgsSetupTaxLineUpsertDto(null, authority.Id, new DateOnly(2026, 1, 1), null)
                ])),
            CancellationToken.None);
        created.Success.Should().BeTrue();
        var lineId = created.Data!.TaxDetails[0].Id;

        var updated = await updateHandler.Handle(
            new UpdateFgsSetupTaxCommand(
                created.Data.Id,
                new FgsSetupTaxUpdateDto("TAX3", "Sync lines", false, null, null, true, null, [])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.TaxDetails.Should().ContainSingle(d => d.Id == lineId && !d.IsActive);

        var line = await context.FgsSetupTaxDetails
            .IgnoreQueryFilters()
            .SingleAsync(d => d.Id == lineId);
        line.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsSetupTaxCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsSetupTaxCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsSetupTaxCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsSetupTaxCommand(new FgsSetupTaxCreateDto("TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value")),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsSetupTaxCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsSetupTaxWriteService CreateWriteService(FgsSetupDbContext context)
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
        var readRepository = new EfTaxReadRepositoryStub(context);
        return new FgsSetupTaxWriteService(context, unitOfWork, auditHelper, readRepository);
    }

    private static async Task<FgsSetupTaxAuthority> SeedTaxAuthorityAsync(FgsSetupDbContext context)
    {
        var authority = new FgsSetupTaxAuthority
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            Code = "AUTH1",
            Name = "Authority 1",
            TaxPercent = 8.25m,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "11111111-1111-1111-1111-111111111111",
            UpdatedOn = DateTimeOffset.UtcNow,
            UpdatedBy = "11111111-1111-1111-1111-111111111111"
        };
        context.FgsSetupTaxAuthorities.Add(authority);
        await context.SaveChangesAsync();
        return authority;
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

    /// <summary>
    /// In-memory stand-in for Dapper read repository used by write-service reload.
    /// </summary>
    private sealed class EfTaxReadRepositoryStub(FgsSetupDbContext context) : IFgsSetupTaxReadRepository
    {
        public async Task<FgsSetupTaxDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var tax = await context.FgsSetupTaxes
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(t => t.TaxDetails)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (tax is null)
            {
                return null;
            }

            var authorityIds = tax.TaxDetails.Select(d => d.FgsSetupTaxAuthorityId).Distinct().ToList();
            var authorities = await context.FgsSetupTaxAuthorities
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => authorityIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id, cancellationToken);

            var lines = tax.TaxDetails
                .OrderBy(d => d.Id)
                .Select(d =>
                {
                    authorities.TryGetValue(d.FgsSetupTaxAuthorityId, out var authority);
                    return new FgsSetupTaxLineDetailDto(
                        d.Id,
                        d.FgsSetupTaxAuthorityId,
                        authority?.Code ?? string.Empty,
                        authority?.Name ?? string.Empty,
                        authority?.TaxPercent ?? 0m,
                        d.EffectiveFromDate,
                        d.EffectiveToDate,
                        d.IsActive);
                })
                .ToList();

            return new FgsSetupTaxDetailDto(
                tax.Id,
                tax.TaxCode,
                tax.Name,
                tax.ShowTaxDetail,
                tax.Description,
                lines.Where(l => l.IsActive).Sum(l => l.TaxPercent),
                tax.IsActive,
                lines);
        }

        public Task<Fgs.Foundation.Paging.PagedResult<FgsSetupTaxSummaryDto>> ListAsync(
            Fgs.Setup.Application.Common.SetupCrud.SetupListQuery query,
            FgsSetupTaxListFilters filters,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<FgsSetupTaxLookupDto>> LookupAsync(
            bool activeOnly = true,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<bool> ExistsByTaxCodeAsync(
            string taxCode,
            long? excludeId = null,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }
}

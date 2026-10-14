using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Entities.GridPreferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class FgsUserGridPreferenceStoreTests
{
    private const long TenantId = 10;

    private const long CompanyId = 20;

    [Fact]
    public async Task GetUpsertAndDelete_AreScopedToTheSignedInUser()
    {
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var otherUserId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        await using var context = await CreateContextAsync();
        context.FgsUsers.AddRange(CreateUser(userId), CreateUser(otherUserId));
        await context.SaveChangesAsync();

        var store = CreateStore(context, userId, "Ada Lovelace");
        var otherStore = CreateStore(context, otherUserId, "Other Person");
        const string selected = """{ "columns": [ { "field": "name", "width": 180, "visible": true } ] }""";
        const string stored = """{"columns":[{"field":"name","width":180,"visible":true}]}""";
        var missing = await store.GetAsync("WorkOrderList");
        var saved = await store.UpsertAsync(" WorkOrderList ", selected);
        var otherUser = await otherStore.GetAsync("WorkOrderList");
        var loaded = await store.GetAsync("WorkOrderList");
        var updated = await store.UpsertAsync("WorkOrderList", "{}");
        var row = await context.FgsUserGridPreferences
            .IgnoreQueryFilters()
            .SingleAsync(preference => preference.GridCode == "WorkOrderList" && preference.UserId == userId);
        await store.DeleteAsync("MissingGrid");
        await store.DeleteAsync("WorkOrderList");
        var afterDelete = await store.GetAsync("WorkOrderList");

        missing.GridConfiguration.GetRawText().Should().Be("{}");
        saved.GridCode.Should().Be("WorkOrderList");
        saved.GridConfiguration.GetRawText().Should().Be(stored);
        otherUser.GridConfiguration.GetRawText().Should().Be("{}");
        loaded.GridConfiguration.GetRawText().Should().Be(stored);
        updated.GridConfiguration.GetRawText().Should().Be("{}");
        row.CreatedBy.Should().Be(userId.ToString("D"));
        row.CreatedBy.Should().NotBe("Ada Lovelace");
        row.GridConfiguration.Should().Be("{}");
        row.UpdatedBy.Should().Be(userId.ToString("D"));
        afterDelete.GridConfiguration.GetRawText().Should().Be("{}");
        (await context.FgsUserGridPreferences.IgnoreQueryFilters().CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Upsert_WhenInsertHitsUniqueViolation_UpdatesTheExistingRow()
    {
        var userId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        await using var context = await CreateContextAsync();
        context.FgsUsers.Add(CreateUser(userId));
        await context.SaveChangesAsync();

        var unitOfWork = new UniqueViolationThenSave(context);
        var store = CreateStore(context, userId, "Ada Lovelace", unitOfWork);

        const string selected = """{"columns":[{"field":"email","visible":true}]}""";
        var saved = await store.UpsertAsync("EstimateList", selected);

        saved.GridConfiguration.GetRawText().Should().Be(selected);
        var rows = await context.FgsUserGridPreferences.IgnoreQueryFilters().ToListAsync();
        rows.Should().ContainSingle();
        rows[0].GridConfiguration.Should().Be(selected);
        rows[0].UpdatedBy.Should().Be(userId.ToString("D"));
        unitOfWork.SaveCalls.Should().Be(2);
    }

    [Fact]
    public async Task Get_WhenCallerIsNotSignedIn_Throws()
    {
        await using var context = await CreateContextAsync();
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(false);
        var store = new FgsUserGridPreferenceStore(
            context,
            new PassThroughUnitOfWork(context),
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);

        var act = () => store.GetAsync("WorkOrderList");

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    private static FgsUserGridPreferenceStore CreateStore(
        FgsUserDbContext context,
        Guid userId,
        string displayName,
        IUnitOfWork? unitOfWork = null)
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(true);
        user.SetupGet(c => c.UserId).Returns(userId);
        user.SetupGet(c => c.DisplayName).Returns(displayName);
        user.SetupGet(c => c.Email).Returns("ada@example.com");
        return new FgsUserGridPreferenceStore(
            context,
            unitOfWork ?? new PassThroughUnitOfWork(context),
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);
    }

    private static FgsUser CreateUser(Guid id) =>
        new()
        {
            Id = id,
            TenantId = TenantId,
            CompanyId = CompanyId,
            Email = $"{id:N}@example.com",
            DisplayName = "User",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var context = new FgsUserDbContext(
            new DbContextOptionsBuilder<FgsUserDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .Options,
            new TestTenantContextAccessor
            {
                Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
            });
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class PassThroughUnitOfWork(FgsUserDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            context.SaveChangesAsync(cancellationToken);

        public Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default) =>
            operation(cancellationToken);

        public Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> operation,
            CancellationToken cancellationToken = default) =>
            operation(cancellationToken);

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class =>
            throw new NotSupportedException();
    }

    private sealed class UniqueViolationThenSave(FgsUserDbContext context) : IUnitOfWork
    {
        public int SaveCalls { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCalls++;
            if (SaveCalls == 1)
            {
                var added = context.ChangeTracker.Entries<FgsUserGridPreference>()
                    .Single(entry => entry.State == EntityState.Added);
                var pending = added.Entity;
                added.State = EntityState.Detached;
                context.FgsUserGridPreferences.Add(new FgsUserGridPreference
                {
                    TenantId = pending.TenantId,
                    CompanyId = pending.CompanyId,
                    UserId = pending.UserId,
                    GridCode = pending.GridCode,
                    GridConfiguration = """{"columns":["old"]}""",
                    CreatedOn = DateTimeOffset.UtcNow,
                    CreatedBy = pending.CreatedBy
                });
                await context.SaveChangesAsync(cancellationToken);
                throw new DbUpdateException(
                    "duplicate key value violates unique constraint (23505)",
                    new Exception("23505"),
                    new List<EntityEntry> { added });
            }

            return await context.SaveChangesAsync(cancellationToken);
        }

        public Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default) =>
            operation(cancellationToken);

        public Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> operation,
            CancellationToken cancellationToken = default) =>
            operation(cancellationToken);

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class =>
            throw new NotSupportedException();
    }
}

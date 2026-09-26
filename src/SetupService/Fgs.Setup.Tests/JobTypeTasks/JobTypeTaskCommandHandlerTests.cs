using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.DeleteJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.JobTypeTasks;

public sealed class JobTypeTaskCommandHandlerTests
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
        var handler = new CreateJobTypeTaskCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.Name.Should().Be("Repair");
        response.Data.TaskName.Should().Be("Repair");
        response.Data.SkillLevelId.Should().BeNull();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "jobtypetask"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateJobTypeTaskCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);
        var deleteHandler = new DeleteJobTypeTaskCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteJobTypeTaskCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteJobTypeTaskCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task CreateHandler_WhenTaskNameOmitted_ComposesCategoryNamePlusSubCategory()
    {
        await using var context = await CreateContextAsync();
        await SeedCategoryAsync(context, jobTypeCategoryId: 1, categoryName: "HVAC Repair");
        var writeService = CreateWriteService(context);
        var handler = new CreateJobTypeTaskCommandHandler(
            writeService,
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Compressor", 5, 10.5m, 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Compressor");
        response.Data.TaskName.Should().Be("HVAC Repair Compressor");
        response.Data.CategoryName.Should().Be("HVAC Repair");
    }

    [Fact]
    public async Task CreateHandler_WhenTaskNameProvided_UsesOverride()
    {
        await using var context = await CreateContextAsync();
        await SeedCategoryAsync(context, jobTypeCategoryId: 1, categoryName: "HVAC Repair");
        var writeService = CreateWriteService(context);
        var handler = new CreateJobTypeTaskCommandHandler(
            writeService,
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeTaskCommand(
                new JobTypeTaskCreateDto(1, 1, "Compressor", 5, 10.5m, 1, TaskName: "Custom Task")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.TaskName.Should().Be("Custom Task");
    }

    [Fact]
    public async Task CreateHandler_WhenIsActiveFalse_CreatesInactiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateJobTypeTaskCommandHandler(
            writeService,
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeTaskCommand(
                new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1, IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static async Task SeedCategoryAsync(
        FgsSetupDbContext context,
        long jobTypeCategoryId,
        string categoryName)
    {
        await context.FgsJobCategories.AddAsync(new FgsJobCategory
        {
            Id = 10,
            TenantId = TenantId,
            CompanyId = CompanyId,
            CategoryCode = "HVAC",
            Name = categoryName,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.FgsJobTypeCategories.AddAsync(new FgsJobTypeCategory
        {
            Id = jobTypeCategoryId,
            TenantId = TenantId,
            CompanyId = CompanyId,
            JobTypeId = 1,
            JobCategoryId = 10,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static JobTypeTaskWriteService CreateWriteService(FgsSetupDbContext context)
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
        return new JobTypeTaskWriteService(context, unitOfWork, auditHelper);
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

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
        response.Data.TaskName.Should().Be("HVAC Repair");
        response.Data.CategoryName.Should().Be("HVAC");
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
    public async Task CreateHandler_WhenTaskNameOmitted_ComposesCategoryAndName()
    {
        await using var context = await CreateContextAsync();
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
        response.Data.TaskName.Should().Be("HVAC Compressor");
        response.Data.CategoryName.Should().Be("HVAC");
    }

    [Fact]
    public async Task CreateHandler_WhenTaskNameProvided_UsesOverride()
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

    [Fact]
    public async Task CreateHandler_WhenDisplayOrderOmitted_UsesNextInCategorySequence()
    {
        await using var context = await CreateContextAsync();
        context.FgsJobCategories.Add(new FgsJobCategory
        {
            Id = 2,
            CategoryCode = "PLMB",
            Name = "Plumbing",
            TenantId = TenantId,
            CompanyId = CompanyId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();

        var handler = new CreateJobTypeTaskCommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateJobTypeTaskCommandHandler>.Instance);

        var first = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "First", 5, 1m, 4)),
            CancellationToken.None);
        var inactive = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Inactive", 5, 1m, 7, IsActive: false)),
            CancellationToken.None);
        var second = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Second", 5, 1m)),
            CancellationToken.None);
        var otherCategory = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(2, 1, "Other", 5, 1m)),
            CancellationToken.None);
        var explicitOrder = await handler.Handle(
            new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Explicit", 5, 1m, 3)),
            CancellationToken.None);

        first.Data!.DisplayOrder.Should().Be(4);
        inactive.Data!.DisplayOrder.Should().Be(7);
        second.Data!.DisplayOrder.Should().Be(8);
        otherCategory.Data!.DisplayOrder.Should().Be(1);
        explicitOrder.Data!.DisplayOrder.Should().Be(3);
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
        context.FgsJobCategories.Add(new FgsJobCategory
        {
            Id = 1,
            CategoryCode = "HVAC",
            Name = "HVAC",
            TenantId = TenantId,
            CompanyId = CompanyId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        });
        await context.SaveChangesAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.DeleteJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Persistence.JobTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.JobTypes;

public sealed class JobTypeCommandHandlerTests
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
        var handler = new CreateJobTypeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateJobTypeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeCommand(new JobTypeCreateDto("TEST", "Name", 5, "BusinessUnit", true, true, 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.ShowToFieldTech.Should().BeTrue();
        response.Data.ShowOnCustomerPortal.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "jobtype"),
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
        var createHandler = new CreateJobTypeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateJobTypeCommandHandler>.Instance);
        var deleteHandler = new DeleteJobTypeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteJobTypeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateJobTypeCommand(new JobTypeCreateDto("TEST", "Name", 5, "BusinessUnit", true, true, 1)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteJobTypeCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task CreateHandler_WritesJobTypeAndSubCategories()
    {
        await using var context = await CreateContextAsync();
        var firstTaskId = await SeedJobTypeTaskAsync(context, "Repair");
        var secondTaskId = await SeedJobTypeTaskAsync(context, "Install");
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var handler = new CreateJobTypeCommandHandler(
            writeService,
            cache.Object,
            CreateTenantContextAccessor(),
            NullLogger<CreateJobTypeCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeCommand(
                new JobTypeCreateDto(
                    "SVC",
                    "Service Call",
                    1,
                    "Field Services",
                    true,
                    false,
                    1,
                    [
                        new JobTypeSubCategoryWriteDto(firstTaskId, 1),
                        new JobTypeSubCategoryWriteDto(secondTaskId, 2)
                    ])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.ShowToFieldTech.Should().BeTrue();
        response.Data.ShowOnCustomerPortal.Should().BeFalse();
        response.Data.SubCategories.Should().HaveCount(2);
        response.Data.SubCategories.Select(c => c.JobTypeTaskId).Should().BeEquivalentTo([firstTaskId, secondTaskId]);
        response.Data.SubCategories.Should().OnlyContain(c => c.CategoryId == 1);
        response.Data.SubCategories.Select(c => c.Name).Should().BeEquivalentTo(["Repair", "Install"]);
        context.FgsJobTypeCategories.Should().HaveCount(2);
        context.FgsJobTypeCategories.Should().OnlyContain(c => c.JobTypeId == response.Data.Id);
    }

    [Fact]
    public async Task UpdateHandler_SyncsSubCategoriesWithoutDeletingRemovedMappings()
    {
        await using var context = await CreateContextAsync();
        var keptTaskId = await SeedJobTypeTaskAsync(context, "Repair");
        var removedTaskId = await SeedJobTypeTaskAsync(context, "Install");
        var addedTaskId = await SeedJobTypeTaskAsync(context, "Inspect");
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateJobTypeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateJobTypeCommandHandler>.Instance);
        var updateHandler = new UpdateJobTypeCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateJobTypeCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateJobTypeCommand(
                new JobTypeCreateDto(
                    "SVC",
                    "Service Call",
                    1,
                    "Field Services",
                    true,
                    true,
                    1,
                    [
                        new JobTypeSubCategoryWriteDto(keptTaskId, 1),
                        new JobTypeSubCategoryWriteDto(removedTaskId, 2)
                    ])),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await updateHandler.Handle(
            new UpdateJobTypeCommand(
                created.Data!.Id,
                new JobTypeUpdateDto(
                    "SVC",
                    "Service Call Updated",
                    1,
                    "Field Services",
                    false,
                    true,
                    2,
                    [
                        new JobTypeSubCategoryWriteDto(keptTaskId, 3),
                        new JobTypeSubCategoryWriteDto(addedTaskId, 4)
                    ])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Service Call Updated");
        response.Data.ShowToFieldTech.Should().BeFalse();
        response.Data.ShowOnCustomerPortal.Should().BeTrue();
        response.Data.SubCategories.Should().HaveCount(3);
        response.Data.SubCategories.Select(c => c.JobTypeTaskId).Should().BeEquivalentTo([keptTaskId, addedTaskId, removedTaskId]);
        context.FgsJobTypeCategories.IgnoreQueryFilters().Should().HaveCount(3);
        context.FgsJobTypeCategories.IgnoreQueryFilters().Single(c => c.JobTypeTaskId == keptTaskId).DisplayOrder.Should().Be(3);
        context.FgsJobTypeCategories.IgnoreQueryFilters().Single(c => c.JobTypeTaskId == addedTaskId).IsActive.Should().BeTrue();
        context.FgsJobTypeCategories.IgnoreQueryFilters().Single(c => c.JobTypeTaskId == removedTaskId).IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static JobTypeWriteService CreateWriteService(FgsSetupDbContext context)
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
        return new JobTypeWriteService(context, unitOfWork, auditHelper);
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

    private static async Task<long> SeedJobTypeTaskAsync(FgsSetupDbContext context, string name)
    {
        var task = new FgsJobTypeTask
        {
            JobCategoryId = 1,
            TradeId = 1,
            Name = name,
            TaskName = name,
            TenantId = TenantId,
            CompanyId = CompanyId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };

        context.FgsJobTypeTasks.Add(task);
        await context.SaveChangesAsync();
        return task.Id;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}

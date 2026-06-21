using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.CreateJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.DeleteJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.JobTypeSubCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.JobTypeSubCategories;

public sealed class JobTypeSubCategoryCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesWithAuditFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateJobTypeSubCategoryCommandHandler(
            writeService,
            NullLogger<CreateJobTypeSubCategoryCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateJobTypeSubCategoryCommand(new JobTypeSubCategoryCreateDto("TEST", "Name value", "Description value", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.TenantId.Should().Be(TenantId);
        response.Data.CompanyId.Should().Be(CompanyId);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateJobTypeSubCategoryCommandHandler(
            writeService,
            NullLogger<CreateJobTypeSubCategoryCommandHandler>.Instance);
        var deleteHandler = new DeleteJobTypeSubCategoryCommandHandler(
            writeService,
            NullLogger<DeleteJobTypeSubCategoryCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateJobTypeSubCategoryCommand(new JobTypeSubCategoryCreateDto("TEST", "Name value", "Description value", 1)),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteJobTypeSubCategoryCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static JobTypeSubCategoryWriteService CreateWriteService(FgsSetupDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }
        };

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new JobTypeSubCategoryWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }
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

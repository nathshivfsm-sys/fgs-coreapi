using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Persistence.Implementations;
using Fgs.User.Application.Features.Tenants.Commands.PatchTenant;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.Tenants.Validators;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class TenantCommandHandlerTests
{
    [Fact]
    public async Task UpdateHandler_UpdatesTenant()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        var cache = new Mock<ICacheService>();

        var handler = new UpdateTenantCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            cache.Object,
            UnauthenticatedContext().Object);

        var response = await handler.Handle(
            new UpdateTenantCommand(tenant.Id, new TenantUpdateDto(
                "Updated Tenant",
                "Updated Legal",
                "info@tenant.com",
                "+15550199",
                "https://tenant.com",
                "USD",
                1,
                true)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Updated Tenant");
        cache.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_WhenTenantMissing_ReturnsNotFound()
    {
        await using var context = await CreateContextAsync();
        var handler = new UpdateTenantCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

        var response = await handler.Handle(
            new UpdateTenantCommand(99, new TenantUpdateDto("Name", null, null, null, null, null, null, true)),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task PatchHandler_UpdatesTenantFields()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        var cache = new Mock<ICacheService>();

        var handler = new PatchTenantCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            cache.Object,
            UnauthenticatedContext().Object);

        var response = await handler.Handle(
            new PatchTenantCommand(tenant.Id, new TenantPatchDto(
                Name: "Patched Tenant",
                PhoneNumber: "+15550199",
                Website: "https://patched.com",
                DefaultCurrency: "EUR",
                DefaultLanguageId: 2,
                IsActive: false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Patched Tenant");
        response.Data.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task PatchHandler_WhenTenantMissing_ReturnsNotFound()
    {
        await using var context = await CreateContextAsync();
        var handler = new PatchTenantCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            Mock.Of<ICacheService>(),
            UnauthenticatedContext().Object);

        var response = await handler.Handle(
            new PatchTenantCommand(99, new TenantPatchDto(Name: "Missing")),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateStorageBucketHandler_UpdatesBucket()
    {
        await using var context = await CreateContextAsync();
        var tenant = await SeedTenantAsync(context);
        var cache = new Mock<ICacheService>();

        var handler = new UpdateTenantStorageBucketCommandHandler(
            TestUserRepositories.Write<FgsTenant>(context),
            new EfUnitOfWork<FgsUserDbContext>(context),
            cache.Object);

        var response = await handler.Handle(
            new UpdateTenantStorageBucketCommand(tenant.Id, new UpdateTenantStorageBucketRequest("new-bucket")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        var updated = await context.FgsTenants.FindAsync(tenant.Id);
        updated!.StorageBucketName.Should().Be("new-bucket");
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidEmail()
    {
        var validator = new PatchTenantCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchTenantCommand(1, new TenantPatchDto(Email: "bad-email")));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_AcceptsValidPayload()
    {
        var validator = new PatchTenantCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchTenantCommand(1, new TenantPatchDto(Name: "Tenant", Email: "a@b.com")));

        result.IsValid.Should().BeTrue();
    }

    private static async Task<FgsUserDbContext> CreateContextAsync() =>
        await TestDbContextFactory.CreateAndInitializeAsync();

    private static async Task<FgsTenant> SeedTenantAsync(FgsUserDbContext context)
    {
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = "TENANT",
            Name = "Tenant",
            FgsTenantStatusId = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();
        return tenant;
    }

    private static Mock<Fgs.Security.Abstractions.IFgsUserContext> UnauthenticatedContext()
    {
        var userContext = new Mock<Fgs.Security.Abstractions.IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        userContext.SetupGet(c => c.DisplayName).Returns("test");
        return userContext;
    }
}

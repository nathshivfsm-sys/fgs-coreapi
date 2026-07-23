using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.Contracts.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.Persistence.Implementations;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Application;

public sealed class SignupUniquenessValidatorTests
{
    [Fact]
    public async Task ValidateAsync_WhenEmailExistsInAnotherTenant_ReturnsConflict()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 99, CompanyId = 1 }
        };

        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        await SeedTenantCompanyAsync(context, 1, 1);
        await SeedTenantCompanyAsync(context, 99, 1);
        context.FgsUsers.Add(CreateUser(1, 1, "existing@test.com"));
        await context.SaveChangesAsync();

        var validator = CreateValidator(context);
        var errors = await validator.ValidateAsync(
            CreateCommand("existing@test.com"),
            CancellationToken.None);

        errors.Should().ContainSingle()
            .Which.Should().Be(SignupErrorMessages.EmailAlreadyUsed);
    }

    [Fact]
    public async Task ValidateAsync_WhenPendingInvitationExistsInAnotherTenant_ReturnsConflict()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 99, CompanyId = 1 }
        };

        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        await SeedTenantCompanyAsync(context, 1, 1);
        await SeedTenantCompanyAsync(context, 99, 1);

        var userId = Guid.NewGuid();
        context.FgsUsers.Add(CreateUser(1, 1, "invite@test.com", userId));
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TenantId = 1,
            Email = "invite@test.com",
            TokenHash = "hash",
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(7)
        });
        await context.SaveChangesAsync();

        var validator = CreateValidator(context);
        var errors = await validator.ValidateAsync(
            CreateCommand("invite@test.com"),
            CancellationToken.None);

        errors.Should().ContainSingle()
            .Which.Should().Be(SignupErrorMessages.EmailAlreadyUsed);
    }

    private static SignupUniquenessValidator CreateValidator(FgsUserDbContext context)
    {
        IUnitOfWork unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);
        return new SignupUniquenessValidator(
            unitOfWork,
            new EmailNormalizer(),
            new DateTimeProvider(),
            TestUserRepositories.InvitationRead(context));
    }

    private static CreateCompanySignupCommand CreateCommand(string email) =>
        new(
            Contact: new SignupContactDto("Admin User", "+15551234567", email),
            Company: new SignupCompanyDto(
                "Acme Field Services",
                "https://acme.example.com",
                new SignupAddressDto("100 Main St", null, "Austin", "TX", "78701", null, "US", null),
                "11-50"),
            BusinessTypeIds: [1]);

    private static async Task SeedTenantCompanyAsync(
        FgsUserDbContext context,
        long tenantId,
        long companyId)
    {
        context.FgsTenants.Add(new FgsTenant
        {
            Id = tenantId,
            TenantGuid = Guid.NewGuid(),
            TenantCode = $"tenant-{tenantId}",
            Name = $"Tenant {tenantId}"
        });

        var companyGuid = Guid.NewGuid();
        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenantId,
            CompanyNumber = companyId,
            CompanyGuid = companyGuid,
            Code = $"company-{companyId}",
            Name = $"Company {companyId}"
        });
        context.FgsTenantCompanyCaches.Add(new FgsTenantCompanyCache
        {
            TenantId = tenantId,
            CompanyId = companyId,
            CompanyGuid = companyGuid,
            CompanyCode = $"company-{companyId}",
            CompanyName = $"Company {companyId}",
            IsActive = true,
            UpdatedOn = DateTimeOffset.UtcNow
        });

        await context.SaveChangesAsync();
    }

    private static FgsUser CreateUser(long tenantId, long companyId, string email, Guid? userId = null) =>
        new()
        {
            Id = userId ?? Guid.NewGuid(),
            TenantId = tenantId,
            CompanyId = companyId,
            Email = email,
            DisplayName = email,
            IsActive = true
        };
}

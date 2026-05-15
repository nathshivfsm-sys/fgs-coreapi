using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Persistence;
using Fgs.User.Infrastructure.Security;
using Fgs.User.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Tests.Application;

public sealed class CreateCompanySignupCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithDuplicateTenantCode_ReturnsConflict()
    {
        var (handler, _) = await CreateHandlerAsync();
        var command = ValidCommand($"dup-{Guid.NewGuid():N}"[..10]);

        await handler.Handle(command, CancellationToken.None);
        var second = await handler.Handle(command, CancellationToken.None);

        second.Success.Should().BeFalse();
        second.StatusCode.Should().Be(ApiStatusCodes.Conflict);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesTenantCompanyUserInvitationAndOutbox()
    {
        var (handler, context) = await CreateHandlerAsync();
        var tc = $"tenant-{Guid.NewGuid():N}"[..12];
        var command = ValidCommand(tc);
        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().NotBeNull();

        (await context.FgsTenants.CountAsync()).Should().Be(1);
        (await context.FgsTenantCompanies.CountAsync()).Should().Be(1);
        (await context.FgsUsers.CountAsync()).Should().Be(1);
        (await context.FgsInvitations.CountAsync()).Should().Be(1);
        (await context.FgsOutboxMessages.CountAsync()).Should().Be(1);

        var tenant = await context.FgsTenants.SingleAsync();
        tenant.TenantCode.Should().Be(tc);
        tenant.Website.Should().Be("https://example.com");

        var company = await context.FgsTenantCompanies.SingleAsync();
        company.Code.Should().Be(tc);
        company.Name.Should().Be(command.TenantName);
        company.Website.Should().Be("https://example.com");

        var user = await context.FgsUsers.SingleAsync();
        user.PasswordHash.Should().NotBeNullOrEmpty();
        user.Role.ToString().Should().Be("Admin");
    }

    [Fact]
    public async Task Handle_WithNoPassword_LeavesPasswordHashNull()
    {
        var (handler, context) = await CreateHandlerAsync();
        var tc = $"tenant-{Guid.NewGuid():N}"[..12];
        var command = ValidCommand(tc) with { Password = null };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Success.Should().BeTrue();
        var user = await context.FgsUsers.SingleAsync();
        user.PasswordHash.Should().BeNull();
    }

    private static CreateCompanySignupCommand ValidCommand(string? tenantCode = null) =>
        new(
            TenantCode: tenantCode ?? $"tenant-{Guid.NewGuid():N}"[..12],
            TenantName: "Test Tenant",
            Email: $"admin-{Guid.NewGuid():N}@test.com",
            DisplayName: "Admin User",
            Website: "https://example.com",
            Password: "Str0ng!Passw0rd",
            TimeZone: "UTC",
            DefaultCurrency: "USD");

    private static async Task<(CreateCompanySignupCommandHandler Handler, FgsUserDbContext Context)> CreateHandlerAsync()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();

        IUnitOfWork unitOfWork = new UnitOfWork(context);
        IDateTimeProvider dateTime = new DateTimeProvider();
        IOutboxWriter outboxWriter = new OutboxWriter(context, dateTime);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Invitation:ExpiryDays"] = "7",
                ["Invitation:InviteBaseUrl"] = "https://localhost/api/invite/start"
            })
            .Build();

        var handler = new CreateCompanySignupCommandHandler(
            unitOfWork,
            new PasswordHasherService(),
            new EmailNormalizer(),
            new InvitationTokenService(),
            outboxWriter,
            dateTime,
            configuration);

        return (handler, context);
    }
}

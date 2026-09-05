using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Foundation.Paging;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.Foundation.Time;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Common;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.Users.Commands.InviteFgsUser;
using Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;
using Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Application.Invitations;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Entities.UserRoles;
using Fgs.User.Infrastructure.Entities.Users;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class InviteFgsUserCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task InviteHandler_CreatesUserRoleInvitationAndUserInvitedOutbox()
    {
        await using var context = await CreateContextAsync();
        var roleId = await SeedRoleAsync(context);
        var (writeService, _) = CreateWriteService(context, roleId);

        var handler = new InviteFgsUserCommandHandler(
            writeService,
            NullLogger<InviteFgsUserCommandHandler>.Instance);

        var response = await handler.Handle(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane Doe", "jane@example.com", null, [roleId])]),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data.Should().HaveCount(1);
        response.Data![0].Email.Should().Be("jane@example.com");
        response.Data[0].DisplayName.Should().Be("Jane Doe");
        response.Data[0].RoleId.Should().Be(roleId);

        (await context.FgsUsers.CountAsync()).Should().Be(1);
        (await context.FgsUserRoles.CountAsync()).Should().Be(1);
        (await context.FgsInvitations.CountAsync()).Should().Be(1);
        (await context.TenantOutboxMessages.CountAsync()).Should().Be(1);

        var user = await context.FgsUsers.SingleAsync();
        user.AuthenticationMethod.Should().Be(AuthenticationMethod.PasswordOrEmailOtp);

        var outbox = await context.TenantOutboxMessages.SingleAsync();
        outbox.RoutingKey.Should().Be(IntegrationEventRoutingKeys.UserInvited);
        var evt = JsonSerializer.Deserialize<UserInvitedEvent>(outbox.Payload);
        evt.Should().NotBeNull();
        evt!.Email.Should().Be("jane@example.com");
        evt.InviteUrl.Should().Contain("token=");
        evt.CompanyName.Should().Be("Acme Plumbing");
        evt.DisplayName.Should().Be("Jane Doe");
    }

    [Fact]
    public async Task InviteHandler_Batch_CreatesMultipleUsersAndAppliesAuthenticationMethod()
    {
        await using var context = await CreateContextAsync();
        var roleId = await SeedRoleAsync(context);
        var (writeService, _) = CreateWriteService(context, roleId);

        var handler = new InviteFgsUserCommandHandler(
            writeService,
            NullLogger<InviteFgsUserCommandHandler>.Instance);

        var response = await handler.Handle(
            new InviteFgsUserCommand(
            [
                new FgsUserInviteDto("Jane Doe", "jane@example.com", null, [roleId], AuthenticationMethod.Password),
                new FgsUserInviteDto("John Doe", "john@example.com", null, [roleId], AuthenticationMethod.EmailOtp)
            ]),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
        (await context.FgsUsers.CountAsync()).Should().Be(2);
        (await context.FgsInvitations.CountAsync()).Should().Be(2);
        (await context.TenantOutboxMessages.CountAsync()).Should().Be(2);

        var jane = await context.FgsUsers.SingleAsync(u => u.Email == "jane@example.com");
        jane.AuthenticationMethod.Should().Be(AuthenticationMethod.Password);
        var john = await context.FgsUsers.SingleAsync(u => u.Email == "john@example.com");
        john.AuthenticationMethod.Should().Be(AuthenticationMethod.EmailOtp);
    }

    [Fact]
    public async Task UpdateHandler_ReplacesRole()
    {
        await using var context = await CreateContextAsync();
        var roleId = await SeedRoleAsync(context, "TECH", "Technician");
        var otherRoleId = await SeedRoleAsync(context, "DISP", "Dispatcher");
        var (writeService, readRepository) = CreateWriteService(context, roleId, otherRoleId);

        var inviteHandler = new InviteFgsUserCommandHandler(
            writeService,
            NullLogger<InviteFgsUserCommandHandler>.Instance);
        var created = await inviteHandler.Handle(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane Doe", "jane@example.com", null, [roleId])]),
            CancellationToken.None);

        var updateHandler = new UpdateFgsUserCommandHandler(
            writeService,
            NullLogger<UpdateFgsUserCommandHandler>.Instance);
        var updated = await updateHandler.Handle(
            new UpdateFgsUserCommand(created.Data![0].Id, new FgsUserUpdateDto("Jane Updated", null, [otherRoleId], true)),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.DisplayName.Should().Be("Jane Updated");
        updated.Data.RoleId.Should().Be(otherRoleId);
        (await context.FgsUserRoles.CountAsync()).Should().Be(1);

        var detail = await readRepository.GetByIdAsync(created.Data[0].Id);
        detail!.RoleName.Should().Be("Dispatcher");
    }

    [Fact]
    public async Task ResendHandler_SupersedesPendingInvitation()
    {
        await using var context = await CreateContextAsync();
        var roleId = await SeedRoleAsync(context);
        var (writeService, _) = CreateWriteService(context, roleId);

        var inviteHandler = new InviteFgsUserCommandHandler(
            writeService,
            NullLogger<InviteFgsUserCommandHandler>.Instance);
        var created = await inviteHandler.Handle(
            new InviteFgsUserCommand([new FgsUserInviteDto("Jane Doe", "jane@example.com", null, [roleId])]),
            CancellationToken.None);

        var firstInviteId = (await context.FgsInvitations.SingleAsync()).Id;

        var resendHandler = new ResendFgsUserInviteCommandHandler(
            writeService,
            NullLogger<ResendFgsUserInviteCommandHandler>.Instance);
        var resent = await resendHandler.Handle(
            new ResendFgsUserInviteCommand(created.Data![0].Id),
            CancellationToken.None);

        resent.Success.Should().BeTrue();
        (await context.FgsInvitations.CountAsync()).Should().Be(2);
        var expired = await context.FgsInvitations.SingleAsync(i => i.Id == firstInviteId);
        expired.Status.Should().Be(InvitationStatus.Expired);
        (await context.FgsInvitations.CountAsync(i => i.Status == InvitationStatus.Pending)).Should().Be(1);
        (await context.TenantOutboxMessages.CountAsync()).Should().Be(2);
    }

    private static (IFgsUserWriteService Write, IFgsUserReadRepository Read) CreateWriteService(
        FgsUserDbContext context,
        params long[] knownRoleIds)
    {
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.Email).Returns("admin@example.com");
        userContext.SetupGet(x => x.DisplayName).Returns("Admin");
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        IDateTimeProvider dateTime = new DateTimeProvider();
        var unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);
        var outboxWriter = new OutboxWriter(context, dateTime, Options.Create(new OutboxOptions()));
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.Invitation.ExpiryDays] = "7",
                [ConfigurationKeys.Invitation.InviteBaseUrl] = "https://localhost/api/v1/invite/start"
            })
            .Build();

        IUserInvitationIssuer issuer = new UserInvitationIssuer(
            unitOfWork,
            new InvitationTokenService(),
            outboxWriter,
            dateTime,
            configuration);

        var roleRead = new Mock<IFgsRoleReadRepository>();
        foreach (var roleId in knownRoleIds.Distinct())
        {
            var id = roleId;
            roleRead.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FgsRoleDetailDto(id, $"R{id}", $"Role {id}", null, null, false, 1, true));
        }

        // Prefer real role names from DB when present
        roleRead.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns<long, CancellationToken>(async (id, ct) =>
            {
                var role = await context.FgsRoles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
                return role is null
                    ? null
                    : new FgsRoleDetailDto(role.Id, role.RoleCode, role.Name, role.Description, role.ParentRoleId, role.IsBuiltIn, role.DisplayOrder, role.IsActive);
            });

        var userRoleWrite = new FgsUserRoleWriteService(context, unitOfWork, tenantAccessor, userContext.Object);
        var readRepository = new InMemoryFgsUserReadRepository(context, TenantId, CompanyId);
        var writeService = new FgsUserWriteService(
            context,
            unitOfWork,
            tenantAccessor,
            userContext.Object,
            readRepository,
            userRoleWrite,
            issuer);

        return (writeService, readRepository);
    }

    private static async Task<long> SeedRoleAsync(
        FgsUserDbContext context,
        string code = "TECH",
        string name = "Technician")
    {
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = code,
            Name = name,
            IsBuiltIn = false,
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };
        context.FgsRoles.Add(role);
        await context.SaveChangesAsync();
        return role.Id;
    }

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        context.FgsTenantCompanyCaches.Add(new FgsTenantCompanyCache
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            CompanyGuid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            CompanyCode = "acme",
            CompanyName = "Acme Plumbing",
            IsActive = true,
            UpdatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        return context;
    }

    /// <summary>
    /// In-memory stand-in for Dapper read repository used by write-service detail return.
    /// </summary>
    private sealed class InMemoryFgsUserReadRepository(FgsUserDbContext context, long tenantId, long companyId)
        : IFgsUserReadRepository
    {
        public async Task<FgsUserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await context.FgsUsers.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId && u.CompanyId == companyId && !u.IsDeleted, cancellationToken);
            if (user is null)
            {
                return null;
            }

            var role = await (
                from ur in context.FgsUserRoles.AsNoTracking()
                join r in context.FgsRoles.AsNoTracking() on ur.FgsRoleId equals r.Id
                where ur.UserId == id
                orderby ur.Id
                select new { ur.FgsRoleId, r.Name }).FirstOrDefaultAsync(cancellationToken);

            var invitation = await context.FgsInvitations.AsNoTracking()
                .Where(i => i.UserId == id)
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => i.Status.ToString())
                .FirstOrDefaultAsync(cancellationToken);

            var accepted = await context.FgsInvitations.AsNoTracking()
                .AnyAsync(i => i.UserId == id && i.Status == InvitationStatus.Accepted, cancellationToken);

            return new FgsUserDetailDto(
                user.Id,
                user.DisplayName,
                user.Email,
                user.PhoneNumber,
                role?.FgsRoleId,
                role?.Name,
                invitation,
                user.IsActive,
                accepted);
        }

        public Task<PagedResult<FgsUserSummaryDto>> ListAsync(
            IdentityListQuery query,
            FgsUserListFilters filters,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<bool> ExistsByEmailAsync(
            string email,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default) =>
            context.FgsUsers.AnyAsync(
                u => u.TenantId == tenantId
                     && u.CompanyId == companyId
                     && !u.IsDeleted
                     && u.Email.ToLower() == email.Trim().ToLower()
                     && (!excludeId.HasValue || u.Id != excludeId.Value),
                cancellationToken);

        public Task<bool> HasAcceptedInvitationAsync(Guid userId, CancellationToken cancellationToken = default) =>
            context.FgsInvitations.AnyAsync(
                i => i.UserId == userId && i.Status == InvitationStatus.Accepted,
                cancellationToken);
    }
}

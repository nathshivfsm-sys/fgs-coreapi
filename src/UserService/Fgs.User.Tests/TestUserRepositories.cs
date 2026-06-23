using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.User.Tests;

internal static class TestUserRepositories
{
    public static IUserWriteRepository<TEntity> Write<TEntity>(FgsUserDbContext context)
        where TEntity : class =>
        new UserEfWriteRepository<TEntity>(context);

    public static IUserReadRepository<FgsUser> ReadUsers(FgsUserDbContext context)
    {
        var mock = new Mock<IUserReadRepository<FgsUser>>();

        mock
            .Setup(r => r.FirstOrDefaultAsync(
                It.Is<string>(w => w.Contains("EntraObjectId", StringComparison.Ordinal)),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, object, CancellationToken>(async (_, parameters, cancellationToken) =>
            {
                var entraObjectId = parameters.GetType().GetProperty("entraObjectId")?.GetValue(parameters) as string;
                return await context.FgsUsers.FirstOrDefaultAsync(
                    u => u.EntraObjectId == entraObjectId && u.IsActive && !u.IsDeleted,
                    cancellationToken);
            });

        mock
            .Setup(r => r.FirstOrDefaultAsync(
                It.Is<string>(w => w.Contains("\"Email\"", StringComparison.Ordinal)),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, object, CancellationToken>(async (_, parameters, cancellationToken) =>
            {
                var email = parameters.GetType().GetProperty("email")?.GetValue(parameters) as string;
                return await context.FgsUsers.FirstOrDefaultAsync(
                    u => u.Email == email && u.IsActive && !u.IsDeleted,
                    cancellationToken);
            });

        return mock.Object;
    }

    public static IUserReadRepository<FgsTenant> ReadTenants(FgsUserDbContext context)
    {
        var writeRepository = Write<FgsTenant>(context);
        var mock = new Mock<IUserReadRepository<FgsTenant>>();

        mock
            .Setup(r => r.AnyAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns<string, object, CancellationToken>(async (_, parameters, cancellationToken) =>
            {
                var tenantCode = parameters.GetType().GetProperty("tenantCode")?.GetValue(parameters) as string;
                return await writeRepository.AnyAsync(t => t.TenantCode == tenantCode, cancellationToken);
            });

        return mock.Object;
    }

    public static IInvitationReadQuery InvitationRead(FgsUserDbContext context, IEmailNormalizer? emailNormalizer = null)
    {
        var normalizer = emailNormalizer ?? new EmailNormalizer();
        var mock = new Mock<IInvitationReadQuery>();

        mock
            .Setup(q => q.HasValidInvitationForUserAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .Returns<Guid, CancellationToken>(async (userId, cancellationToken) =>
            {
                return await context.FgsInvitations.AnyAsync(
                    i => i.UserId == userId
                         && (i.Status == InvitationStatus.Pending || i.Status == InvitationStatus.Accepted),
                    cancellationToken);
            });

        mock
            .Setup(q => q.HasPendingInvitationForNormalizedEmailAsync(
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, DateTimeOffset, CancellationToken>(async (normalizedEmail, nowUtc, cancellationToken) =>
            {
                var invitations = await context.FgsInvitations
                    .Where(i => i.Status == InvitationStatus.Pending && i.ExpiresAtUtc > nowUtc)
                    .Select(i => i.Email)
                    .ToListAsync(cancellationToken);

                return invitations.Any(email => normalizer.Normalize(email) == normalizedEmail);
            });

        return mock.Object;
    }

    public static IUserRoleCodesReadQuery RoleCodesRead(FgsUserDbContext context, IReadOnlyList<string>? defaultRoles = null)
    {
        var mock = new Mock<IUserRoleCodesReadQuery>();

        mock
            .Setup(q => q.GetRoleCodesForUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns<Guid, CancellationToken>(async (userId, cancellationToken) =>
            {
                if (defaultRoles is not null)
                {
                    return defaultRoles;
                }

                return await (
                    from userRole in context.FgsUserRoles
                    join role in context.FgsRoles on userRole.FgsRoleId equals role.Id
                    where userRole.UserId == userId && userRole.FgsRoleId != null
                    select role.RoleCode).ToListAsync(cancellationToken);
            });

        return mock.Object;
    }
}

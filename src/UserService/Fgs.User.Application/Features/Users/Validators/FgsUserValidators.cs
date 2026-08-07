using Fgs.Security.Constants;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Commands.InviteFgsUser;
using Fgs.User.Application.Features.Users.Commands.PatchFgsUser;
using Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;
using Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;
using FluentValidation;

namespace Fgs.User.Application.Features.Users.Validators;

public sealed class InviteFgsUserCommandValidator : AbstractValidator<InviteFgsUserCommand>
{
    public InviteFgsUserCommandValidator(
        IFgsUserReadRepository userReadRepository,
        IFgsRoleReadRepository roleReadRepository,
        IInvitationReadQuery invitationReadQuery,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime)
    {
        RuleFor(x => x.Invites).NotEmpty().WithMessage("At least one invite is required.");

        RuleFor(x => x.Invites)
            .Must(invites =>
            {
                var emails = invites
                    .Select(i => i.Email?.Trim().ToLowerInvariant())
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .ToList();
                return emails.Count == emails.Distinct().Count();
            })
            .WithMessage("Duplicate email addresses are not allowed in the same invite request.")
            .When(x => x.Invites is { Count: > 0 });

        RuleFor(x => x.Invites)
            .MustAsync(async (invites, cancellationToken) =>
            {
                var tenantAdminInviteCount = 0;
                foreach (var invite in invites)
                {
                    if (invite.RoleId <= 0)
                    {
                        continue;
                    }

                    var role = await roleReadRepository.GetByIdAsync(invite.RoleId, cancellationToken);
                    if (role is not null
                        && string.Equals(role.RoleCode, FgsRoleCodes.TenantAdmin, StringComparison.OrdinalIgnoreCase))
                    {
                        tenantAdminInviteCount++;
                    }
                }

                return tenantAdminInviteCount <= 1;
            })
            .WithMessage("Only one tenant admin invite is allowed in a single request.")
            .When(x => x.Invites is { Count: > 0 });

        RuleForEach(x => x.Invites).ChildRules(invite =>
        {
            invite.RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(200);
            invite.RuleFor(x => x.Email).NotEmpty().MaximumLength(300).EmailAddress();
            invite.RuleFor(x => x.PhoneNumber).MaximumLength(20);
            invite.RuleFor(x => x.AuthenticationMethod).IsInEnum();
            invite.RuleFor(x => x.Email).MustAsync(async (_, email, cancellationToken) =>
                    !await userReadRepository.ExistsByEmailAsync(email, null, cancellationToken))
                .WithMessage("A user with this email already exists for this tenant and company.");
            invite.RuleFor(x => x.Email).MustAsync(async (_, email, cancellationToken) =>
                    !await invitationReadQuery.HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
                        emailNormalizer.Normalize(email),
                        dateTime.UtcNow,
                        cancellationToken))
                .WithMessage("A pending invitation already exists for this email in this tenant and company.");
            invite.RuleFor(x => x.RoleId).GreaterThan(0);
            invite.RuleFor(x => x.RoleId).MustAsync(async (_, roleId, cancellationToken) =>
                    await roleReadRepository.GetByIdAsync(roleId, cancellationToken) is not null)
                .WithMessage("The specified role was not found.");
            invite.RuleFor(x => x.RoleId).MustAsync(async (_, roleId, cancellationToken) =>
                {
                    var role = await roleReadRepository.GetByIdAsync(roleId, cancellationToken);
                    if (role is null
                        || !string.Equals(role.RoleCode, FgsRoleCodes.TenantAdmin, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return !await roleReadRepository.HasOtherActiveUserWithRoleCodeAsync(
                        FgsRoleCodes.TenantAdmin,
                        excludeUserId: null,
                        cancellationToken);
                })
                .WithMessage("Only one tenant admin is allowed per company.");
        });
    }
}

public sealed class UpdateFgsUserCommandValidator : AbstractValidator<UpdateFgsUserCommand>
{
    public UpdateFgsUserCommandValidator(IFgsRoleReadRepository roleReadRepository)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Dto.RoleId).GreaterThan(0);
        RuleFor(x => x.Dto.RoleId).MustAsync(async (_, roleId, cancellationToken) =>
                await roleReadRepository.GetByIdAsync(roleId, cancellationToken) is not null)
            .WithMessage("The specified role was not found.");
        RuleFor(x => x).MustAsync(async (command, cancellationToken) =>
            {
                var role = await roleReadRepository.GetByIdAsync(command.Dto.RoleId, cancellationToken);
                if (role is null
                    || !string.Equals(role.RoleCode, FgsRoleCodes.TenantAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return !await roleReadRepository.HasOtherActiveUserWithRoleCodeAsync(
                    FgsRoleCodes.TenantAdmin,
                    excludeUserId: command.Id,
                    cancellationToken);
            })
            .WithMessage("Only one tenant admin is allowed per company.");
    }
}

public sealed class PatchFgsUserCommandValidator : AbstractValidator<PatchFgsUserCommand>
{
    public PatchFgsUserCommandValidator(IFgsRoleReadRepository roleReadRepository)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200).When(x => x.Dto.DisplayName is not null);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(20).When(x => x.Dto.PhoneNumber is not null);
        RuleFor(x => x.Dto.RoleId).GreaterThan(0).When(x => x.Dto.RoleId.HasValue);
        RuleFor(x => x.Dto.RoleId).MustAsync(async (_, roleId, cancellationToken) =>
                await roleReadRepository.GetByIdAsync(roleId!.Value, cancellationToken) is not null)
            .WithMessage("The specified role was not found.")
            .When(x => x.Dto.RoleId.HasValue);
        RuleFor(x => x).MustAsync(async (command, cancellationToken) =>
            {
                if (!command.Dto.RoleId.HasValue)
                {
                    return true;
                }

                var role = await roleReadRepository.GetByIdAsync(command.Dto.RoleId.Value, cancellationToken);
                if (role is null
                    || !string.Equals(role.RoleCode, FgsRoleCodes.TenantAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return !await roleReadRepository.HasOtherActiveUserWithRoleCodeAsync(
                    FgsRoleCodes.TenantAdmin,
                    excludeUserId: command.Id,
                    cancellationToken);
            })
            .WithMessage("Only one tenant admin is allowed per company.")
            .When(x => x.Dto.RoleId.HasValue);
    }
}

public sealed class ResendFgsUserInviteCommandValidator : AbstractValidator<ResendFgsUserInviteCommand>
{
    public ResendFgsUserInviteCommandValidator(IFgsUserReadRepository userReadRepository)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Id).MustAsync(async (id, cancellationToken) =>
                !await userReadRepository.HasAcceptedInvitationAsync(id, cancellationToken))
            .WithMessage("Cannot resend invite for a user who has already accepted.");
    }
}

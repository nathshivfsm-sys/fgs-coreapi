using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.PatchFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using Fgs.User.Application.Features.UserRoles.Commands.UpdateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Queries.LookupFgsUserRoles;
using FluentValidation;

namespace Fgs.User.Application.Features.UserRoles.Validators;

public sealed class SyncFgsUserRolesCommandValidator : AbstractValidator<SyncFgsUserRolesCommand>
{
    public SyncFgsUserRolesCommandValidator()
    {
        RuleFor(x => x.Dto.UserId).NotEmpty();
        RuleFor(x => x.Dto.FgsRoleIds).NotNull();
        RuleForEach(x => x.Dto.FgsRoleIds).GreaterThan(0);
    }
}

public sealed class CreateFgsUserRoleCommandValidator : AbstractValidator<CreateFgsUserRoleCommand>
{
    public CreateFgsUserRoleCommandValidator(IFgsUserRoleReadRepository readRepository)
    {
        RuleFor(x => x.Dto.UserId).NotEmpty();
        RuleFor(x => x.Dto.FgsRoleId)
            .GreaterThan(0)
            .MustAsync(async (command, roleId, cancellationToken) =>
                !await readRepository.ExistsByUserIdAndRoleIdAsync(
                    command.Dto.UserId,
                    roleId,
                    null,
                    cancellationToken))
            .WithMessage("A user-role assignment with this UserId and FgsRoleId already exists.");
    }
}

public sealed class UpdateFgsUserRoleCommandValidator : AbstractValidator<UpdateFgsUserRoleCommand>
{
    public UpdateFgsUserRoleCommandValidator(IFgsUserRoleReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsRoleId)
            .GreaterThan(0)
            .MustAsync(async (command, roleId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByUserIdAndRoleIdAsync(
                    existing.UserId,
                    roleId,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A user-role assignment with this UserId and FgsRoleId already exists.");
    }
}

public sealed class PatchFgsUserRoleCommandValidator : AbstractValidator<PatchFgsUserRoleCommand>
{
    public PatchFgsUserRoleCommandValidator(IFgsUserRoleReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsRoleId)
            .GreaterThan(0)
            .MustAsync(async (command, roleId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByUserIdAndRoleIdAsync(
                    existing.UserId,
                    roleId!.Value,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A user-role assignment with this UserId and FgsRoleId already exists.")
            .When(x => x.Dto.FgsRoleId.HasValue);
    }
}

public sealed class LookupFgsUserRolesQueryValidator : AbstractValidator<LookupFgsUserRolesQuery>
{
    public LookupFgsUserRolesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

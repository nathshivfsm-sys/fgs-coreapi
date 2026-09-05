using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.PatchFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Commands.UpdateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Queries.LookupFgsRolePermissions;
using FluentValidation;

namespace Fgs.User.Application.Features.RolePermissions.Validators;

public sealed class SyncFgsRolePermissionsCommandValidator : AbstractValidator<SyncFgsRolePermissionsCommand>
{
    public SyncFgsRolePermissionsCommandValidator()
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsPermissionIds).NotNull();
        RuleForEach(x => x.Dto.FgsPermissionIds).GreaterThan(0);
    }
}

public sealed class CreateFgsRolePermissionCommandValidator : AbstractValidator<CreateFgsRolePermissionCommand>
{
    public CreateFgsRolePermissionCommandValidator(IFgsRolePermissionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsPermissionId)
            .GreaterThan(0)
            .MustAsync(async (command, permissionId, cancellationToken) =>
                !await readRepository.ExistsByRoleIdAndPermissionIdAsync(
                    command.Dto.FgsRoleId,
                    permissionId,
                    null,
                    cancellationToken))
            .WithMessage("A role-permission assignment with this FgsRoleId and FgsPermissionId already exists.");
    }
}

public sealed class UpdateFgsRolePermissionCommandValidator : AbstractValidator<UpdateFgsRolePermissionCommand>
{
    public UpdateFgsRolePermissionCommandValidator(IFgsRolePermissionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsPermissionId)
            .GreaterThan(0)
            .MustAsync(async (command, permissionId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByRoleIdAndPermissionIdAsync(
                    existing.FgsRoleId,
                    permissionId,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A role-permission assignment with this FgsRoleId and FgsPermissionId already exists.");
    }
}

public sealed class PatchFgsRolePermissionCommandValidator : AbstractValidator<PatchFgsRolePermissionCommand>
{
    public PatchFgsRolePermissionCommandValidator(IFgsRolePermissionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsPermissionId)
            .GreaterThan(0)
            .MustAsync(async (command, permissionId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByRoleIdAndPermissionIdAsync(
                    existing.FgsRoleId,
                    permissionId!.Value,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A role-permission assignment with this FgsRoleId and FgsPermissionId already exists.")
            .When(x => x.Dto.FgsPermissionId.HasValue);
    }
}

public sealed class LookupFgsRolePermissionsQueryValidator : AbstractValidator<LookupFgsRolePermissionsQuery>
{
    public LookupFgsRolePermissionsQueryValidator()
    {
        RuleFor(x => x.FgsRoleId).GreaterThan(0);
    }
}

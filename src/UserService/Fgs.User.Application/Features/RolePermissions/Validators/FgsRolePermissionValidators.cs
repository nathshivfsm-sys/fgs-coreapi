using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
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

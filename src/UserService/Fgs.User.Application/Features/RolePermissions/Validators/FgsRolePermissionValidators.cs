using Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;
using Fgs.User.Application.Features.RolePermissions.Commands.DeleteFgsRolePermission;
using FluentValidation;

namespace Fgs.User.Application.Features.RolePermissions.Validators;

public sealed class CreateFgsRolePermissionCommandValidator : AbstractValidator<CreateFgsRolePermissionCommand>
{
    public CreateFgsRolePermissionCommandValidator()
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsPermissionId).GreaterThan(0);
    }
}

public sealed class DeleteFgsRolePermissionCommandValidator : AbstractValidator<DeleteFgsRolePermissionCommand>
{
    public DeleteFgsRolePermissionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

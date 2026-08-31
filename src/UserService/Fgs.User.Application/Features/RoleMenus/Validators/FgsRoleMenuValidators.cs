using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using FluentValidation;

namespace Fgs.User.Application.Features.RoleMenus.Validators;

public sealed class SyncFgsRoleMenusCommandValidator : AbstractValidator<SyncFgsRoleMenusCommand>
{
    public SyncFgsRoleMenusCommandValidator()
    {
        RuleFor(x => x.Dto.RoleId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotNull();
        RuleForEach(x => x.Dto.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.MenuId).GreaterThan(0);
            item.RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

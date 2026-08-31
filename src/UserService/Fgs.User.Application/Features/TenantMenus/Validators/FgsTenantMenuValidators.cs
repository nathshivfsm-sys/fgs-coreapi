using Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;
using FluentValidation;

namespace Fgs.User.Application.Features.TenantMenus.Validators;

public sealed class SyncFgsTenantMenusCommandValidator : AbstractValidator<SyncFgsTenantMenusCommand>
{
    public SyncFgsTenantMenusCommandValidator()
    {
        RuleFor(x => x.Dto.Items).NotNull();
        RuleForEach(x => x.Dto.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.MenuId).GreaterThan(0);
            item.RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

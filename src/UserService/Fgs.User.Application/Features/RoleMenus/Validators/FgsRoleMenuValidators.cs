using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.CreateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.PatchFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;
using Fgs.User.Application.Features.RoleMenus.Commands.UpdateFgsRoleMenu;
using Fgs.User.Application.Features.RoleMenus.Queries.LookupFgsRoleMenus;
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

public sealed class LookupFgsRoleMenusQueryValidator : AbstractValidator<LookupFgsRoleMenusQuery>
{
    public LookupFgsRoleMenusQueryValidator()
    {
        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}

public sealed class CreateFgsRoleMenuCommandValidator : AbstractValidator<CreateFgsRoleMenuCommand>
{
    public CreateFgsRoleMenuCommandValidator(IFgsRoleMenuReadRepository readRepository)
    {
        RuleFor(x => x.Dto.RoleId).GreaterThan(0);

        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .MustAsync(async (command, menuId, cancellationToken) =>
                !await readRepository.ExistsByRoleMenuAsync(command.Dto.RoleId, menuId, null, cancellationToken))
            .WithMessage("A role menu with this RoleId and MenuId already exists.");

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
    }
}

public sealed class UpdateFgsRoleMenuCommandValidator : AbstractValidator<UpdateFgsRoleMenuCommand>
{
    public UpdateFgsRoleMenuCommandValidator(IFgsRoleMenuReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.RoleId).GreaterThan(0);

        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .MustAsync(async (command, menuId, cancellationToken) =>
                !await readRepository.ExistsByRoleMenuAsync(command.Dto.RoleId, menuId, command.Id, cancellationToken))
            .WithMessage("A role menu with this RoleId and MenuId already exists.");

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
    }
}

public sealed class PatchFgsRoleMenuCommandValidator : AbstractValidator<PatchFgsRoleMenuCommand>
{
    public PatchFgsRoleMenuCommandValidator(IFgsRoleMenuReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.RoleId)
            .GreaterThan(0)
            .When(x => x.Dto.RoleId.HasValue);

        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .When(x => x.Dto.MenuId.HasValue);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThanOrEqualTo((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                var roleId = command.Dto.RoleId ?? existing.RoleId;
                var menuId = command.Dto.MenuId ?? existing.MenuId;
                return !await readRepository.ExistsByRoleMenuAsync(roleId, menuId, command.Id, cancellationToken);
            })
            .WithMessage("A role menu with this RoleId and MenuId already exists.")
            .When(x => x.Dto.RoleId.HasValue || x.Dto.MenuId.HasValue);
    }
}

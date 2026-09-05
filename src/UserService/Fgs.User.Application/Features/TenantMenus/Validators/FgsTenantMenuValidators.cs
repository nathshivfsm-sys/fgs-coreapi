using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Commands.CreateFgsTenantMenu;
using Fgs.User.Application.Features.TenantMenus.Commands.PatchFgsTenantMenu;
using Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;
using Fgs.User.Application.Features.TenantMenus.Commands.UpdateFgsTenantMenu;
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
            item.RuleFor(x => x.MenuCode).NotEmpty().MaximumLength(50);
            item.RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            item.RuleFor(x => x.MenuType).NotEmpty().MaximumLength(20);
            item.RuleFor(x => x.Description).MaximumLength(255).When(x => x.Description is not null);
            item.RuleFor(x => x.ParentMenuId).GreaterThan(0).When(x => x.ParentMenuId.HasValue);
            item.RuleFor(x => x.Route).MaximumLength(255).When(x => x.Route is not null);
            item.RuleFor(x => x.Icon).MaximumLength(100).When(x => x.Icon is not null);
            item.RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)0);
        });
    }
}

public sealed class CreateFgsTenantMenuCommandValidator : AbstractValidator<CreateFgsTenantMenuCommand>
{
    public CreateFgsTenantMenuCommandValidator(IFgsTenantMenuReadRepository readRepository)
    {
        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .MustAsync(async (command, menuId, cancellationToken) =>
                !await readRepository.ExistsByMenuIdAsync(menuId, null, cancellationToken))
            .WithMessage("A tenant menu with this MenuId already exists.");

        RuleFor(x => x.Dto.MenuCode)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (command, menuCode, cancellationToken) =>
                !await readRepository.ExistsByMenuCodeAsync(menuCode, null, cancellationToken))
            .WithMessage("A tenant menu with this MenuCode already exists.");

        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.MenuType).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.ParentMenuId).GreaterThan(0).When(x => x.Dto.ParentMenuId.HasValue);
        RuleFor(x => x.Dto.Route).MaximumLength(255).When(x => x.Dto.Route is not null);
        RuleFor(x => x.Dto.Icon).MaximumLength(100).When(x => x.Dto.Icon is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
    }
}

public sealed class UpdateFgsTenantMenuCommandValidator : AbstractValidator<UpdateFgsTenantMenuCommand>
{
    public UpdateFgsTenantMenuCommandValidator(IFgsTenantMenuReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .MustAsync(async (command, menuId, cancellationToken) =>
                !await readRepository.ExistsByMenuIdAsync(menuId, command.Id, cancellationToken))
            .WithMessage("A tenant menu with this MenuId already exists.");

        RuleFor(x => x.Dto.MenuCode)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (command, menuCode, cancellationToken) =>
                !await readRepository.ExistsByMenuCodeAsync(menuCode, command.Id, cancellationToken))
            .WithMessage("A tenant menu with this MenuCode already exists.");

        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.MenuType).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.ParentMenuId).GreaterThan(0).When(x => x.Dto.ParentMenuId.HasValue);
        RuleFor(x => x.Dto.Route).MaximumLength(255).When(x => x.Dto.Route is not null);
        RuleFor(x => x.Dto.Icon).MaximumLength(100).When(x => x.Dto.Icon is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);
    }
}

public sealed class PatchFgsTenantMenuCommandValidator : AbstractValidator<PatchFgsTenantMenuCommand>
{
    public PatchFgsTenantMenuCommandValidator(IFgsTenantMenuReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.MenuId)
            .GreaterThan(0)
            .MustAsync(async (command, menuId, cancellationToken) =>
                !await readRepository.ExistsByMenuIdAsync(menuId!.Value, command.Id, cancellationToken))
            .WithMessage("A tenant menu with this MenuId already exists.")
            .When(x => x.Dto.MenuId.HasValue);

        RuleFor(x => x.Dto.MenuCode)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (command, menuCode, cancellationToken) =>
                !await readRepository.ExistsByMenuCodeAsync(menuCode!, command.Id, cancellationToken))
            .WithMessage("A tenant menu with this MenuCode already exists.")
            .When(x => x.Dto.MenuCode is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.MenuType)
            .NotEmpty()
            .MaximumLength(20)
            .When(x => x.Dto.MenuType is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.ParentMenuId)
            .GreaterThan(0)
            .When(x => x.Dto.ParentMenuId.HasValue);

        RuleFor(x => x.Dto.Route)
            .MaximumLength(255)
            .When(x => x.Dto.Route is not null);

        RuleFor(x => x.Dto.Icon)
            .MaximumLength(100)
            .When(x => x.Dto.Icon is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThanOrEqualTo((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}

using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Commands.CreateFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.PatchFgsPermission;
using Fgs.User.Application.Features.Permissions.Commands.UpdateFgsPermission;
using FluentValidation;

namespace Fgs.User.Application.Features.Permissions.Validators;

public sealed class CreateFgsPermissionCommandValidator : AbstractValidator<CreateFgsPermissionCommand>
{
    public CreateFgsPermissionCommandValidator(IFgsPermissionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.PermissionCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("PermissionCode must be uppercase.")
            .MustAsync(async (command, permissionCode, cancellationToken) =>
                !await readRepository.ExistsByPermissionCodeAsync(permissionCode, null, cancellationToken))
            .WithMessage("A permission with this permission code already exists.");

        RuleFor(x => x.Dto.Module)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Resource)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Action)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class UpdateFgsPermissionCommandValidator : AbstractValidator<UpdateFgsPermissionCommand>
{
    public UpdateFgsPermissionCommandValidator(IFgsPermissionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.PermissionCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("PermissionCode must be uppercase.")
            .MustAsync(async (command, permissionCode, cancellationToken) =>
                !await readRepository.ExistsByPermissionCodeAsync(permissionCode, command.Id, cancellationToken))
            .WithMessage("A permission with this permission code already exists.");

        RuleFor(x => x.Dto.Module)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Resource)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Action)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class PatchFgsPermissionCommandValidator : AbstractValidator<PatchFgsPermissionCommand>
{
    public PatchFgsPermissionCommandValidator(IFgsPermissionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.PermissionCode)
            .NotEmpty()
            .MaximumLength(100)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("PermissionCode must be uppercase.")
            .MustAsync(async (command, permissionCode, cancellationToken) =>
                !await readRepository.ExistsByPermissionCodeAsync(permissionCode!, command.Id, cancellationToken))
            .WithMessage("A permission with this permission code already exists.")
            .When(x => x.Dto.PermissionCode is not null);

        RuleFor(x => x.Dto.Module)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.Module is not null);

        RuleFor(x => x.Dto.Resource)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.Resource is not null);

        RuleFor(x => x.Dto.Action)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.Action is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}

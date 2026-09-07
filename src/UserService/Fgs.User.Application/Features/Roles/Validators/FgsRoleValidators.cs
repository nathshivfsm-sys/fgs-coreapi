using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Commands.CloneFgsRole;
using Fgs.User.Application.Features.Roles.Commands.CreateFgsRole;
using Fgs.User.Application.Features.Roles.Commands.PatchFgsRole;
using Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole;
using FluentValidation;

namespace Fgs.User.Application.Features.Roles.Validators;

public sealed class CreateFgsRoleCommandValidator : AbstractValidator<CreateFgsRoleCommand>
{
    public CreateFgsRoleCommandValidator(IFgsRoleReadRepository readRepository)
    {
        RuleFor(x => x.Dto.RoleCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("RoleCode must be uppercase.")
            .MustAsync(async (command, roleCode, cancellationToken) =>
                !await readRepository.ExistsByRoleCodeAsync(roleCode, null, cancellationToken))
            .WithMessage("A role with this role code already exists.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class CloneFgsRoleCommandValidator : AbstractValidator<CloneFgsRoleCommand>
{
    public CloneFgsRoleCommandValidator(IFgsRoleReadRepository readRepository)
    {
        RuleFor(x => x.SourceRoleId).GreaterThan(0);

        RuleFor(x => x.Dto.RoleCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("RoleCode must be uppercase.")
            .MustAsync(async (command, roleCode, cancellationToken) =>
                !await readRepository.ExistsByRoleCodeAsync(roleCode, null, cancellationToken))
            .WithMessage("A role with this role code already exists.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);

        RuleForEach(x => x.Dto.FgsPermissionIds)
            .GreaterThan(0)
            .When(x => x.Dto.FgsPermissionIds is not null);
    }
}

public sealed class UpdateFgsRoleCommandValidator : AbstractValidator<UpdateFgsRoleCommand>
{
    public UpdateFgsRoleCommandValidator(IFgsRoleReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.RoleCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("RoleCode must be uppercase.")
            .MustAsync(async (command, roleCode, cancellationToken) =>
                !await readRepository.ExistsByRoleCodeAsync(roleCode, command.Id, cancellationToken))
            .WithMessage("A role with this role code already exists.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class PatchFgsRoleCommandValidator : AbstractValidator<PatchFgsRoleCommand>
{
    public PatchFgsRoleCommandValidator(IFgsRoleReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.RoleCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("RoleCode must be uppercase.")
            .MustAsync(async (command, roleCode, cancellationToken) =>
                !await readRepository.ExistsByRoleCodeAsync(roleCode!, command.Id, cancellationToken))
            .WithMessage("A role with this role code already exists.")
            .When(x => x.Dto.RoleCode is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("A role with this name already exists.")
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}

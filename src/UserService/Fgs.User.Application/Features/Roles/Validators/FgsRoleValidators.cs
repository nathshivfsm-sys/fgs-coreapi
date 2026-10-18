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
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
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
            .CustomAsync(async (name, context, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                var command = context.InstanceToValidate;
                var effectiveName = await ResolveEffectiveCloneNameAsync(
                    readRepository,
                    command.SourceRoleId,
                    name,
                    cancellationToken);

                if (effectiveName.Length > MaxRoleNameLength)
                {
                    context.AddFailure("Name must be 100 characters or fewer.");
                }

                if (await readRepository.ExistsByNameAsync(effectiveName, null, cancellationToken))
                {
                    context.AddFailure("A role with this name already exists.");
                }
            });

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

    private const int MaxRoleNameLength = 100;

    private static async Task<string> ResolveEffectiveCloneNameAsync(
        IFgsRoleReadRepository readRepository,
        long sourceRoleId,
        string submittedName,
        CancellationToken cancellationToken)
    {
        var source = await readRepository.GetByIdAsync(sourceRoleId, cancellationToken);
        if (source is null)
        {
            return submittedName.Trim();
        }

        return ResolveCloneRoleName(submittedName, source.Name);
    }

    private static string ResolveCloneRoleName(string submittedName, string sourceName)
    {
        var trimmedSubmitted = submittedName.Trim();
        var trimmedSource = sourceName.Trim();
        if (string.Equals(trimmedSubmitted, trimmedSource, StringComparison.OrdinalIgnoreCase))
        {
            return "Copy of " + trimmedSource;
        }

        return trimmedSubmitted;
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

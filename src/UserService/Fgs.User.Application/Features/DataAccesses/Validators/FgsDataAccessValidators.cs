using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Commands.CreateFgsDataAccess;
using Fgs.User.Application.Features.DataAccesses.Commands.PatchFgsDataAccess;
using Fgs.User.Application.Features.DataAccesses.Commands.UpdateFgsDataAccess;
using FluentValidation;

namespace Fgs.User.Application.Features.DataAccesses.Validators;

public sealed class CreateFgsDataAccessCommandValidator : AbstractValidator<CreateFgsDataAccessCommand>
{
    public CreateFgsDataAccessCommandValidator(IFgsDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Dto.DataAccessCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("DataAccessCode must be uppercase.")
            .MustAsync(async (command, dataAccessCode, cancellationToken) =>
                !await readRepository.ExistsByDataAccessCodeAsync(dataAccessCode, null, cancellationToken))
            .WithMessage("A data access profile with this data access code already exists.");

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

public sealed class UpdateFgsDataAccessCommandValidator : AbstractValidator<UpdateFgsDataAccessCommand>
{
    public UpdateFgsDataAccessCommandValidator(IFgsDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.DataAccessCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("DataAccessCode must be uppercase.")
            .MustAsync(async (command, dataAccessCode, cancellationToken) =>
                !await readRepository.ExistsByDataAccessCodeAsync(dataAccessCode, command.Id, cancellationToken))
            .WithMessage("A data access profile with this data access code already exists.");

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

public sealed class PatchFgsDataAccessCommandValidator : AbstractValidator<PatchFgsDataAccessCommand>
{
    public PatchFgsDataAccessCommandValidator(IFgsDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.DataAccessCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("DataAccessCode must be uppercase.")
            .MustAsync(async (command, dataAccessCode, cancellationToken) =>
                !await readRepository.ExistsByDataAccessCodeAsync(dataAccessCode!, command.Id, cancellationToken))
            .WithMessage("A data access profile with this data access code already exists.")
            .When(x => x.Dto.DataAccessCode is not null);

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

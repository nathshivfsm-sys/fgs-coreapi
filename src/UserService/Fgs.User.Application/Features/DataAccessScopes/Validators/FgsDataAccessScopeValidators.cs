using Fgs.User.Application.Features.DataAccessScopes.Commands.CreateFgsDataAccessScope;
using Fgs.User.Application.Features.DataAccessScopes.Commands.PatchFgsDataAccessScope;
using Fgs.User.Application.Features.DataAccessScopes.Commands.UpdateFgsDataAccessScope;
using FluentValidation;

namespace Fgs.User.Application.Features.DataAccessScopes.Validators;

public sealed class CreateFgsDataAccessScopeCommandValidator : AbstractValidator<CreateFgsDataAccessScopeCommand>
{
    public CreateFgsDataAccessScopeCommandValidator()
    {
        RuleFor(x => x.Dto.FgsDataAccessId)
            .GreaterThan(0);

        RuleFor(x => x.Dto.ScopeType)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Operator)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Dto.ScopeValue)
            .MaximumLength(255)
            .When(x => x.Dto.ScopeValue is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class UpdateFgsDataAccessScopeCommandValidator : AbstractValidator<UpdateFgsDataAccessScopeCommand>
{
    public UpdateFgsDataAccessScopeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.ScopeType)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Operator)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Dto.ScopeValue)
            .MaximumLength(255)
            .When(x => x.Dto.ScopeValue is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0);
    }
}

public sealed class PatchFgsDataAccessScopeCommandValidator : AbstractValidator<PatchFgsDataAccessScopeCommand>
{
    public PatchFgsDataAccessScopeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.ScopeType)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.ScopeType is not null);

        RuleFor(x => x.Dto.Operator)
            .NotEmpty()
            .MaximumLength(20)
            .When(x => x.Dto.Operator is not null);

        RuleFor(x => x.Dto.ScopeValue)
            .MaximumLength(255)
            .When(x => x.Dto.ScopeValue is not null);

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThan((short)0)
            .When(x => x.Dto.DisplayOrder.HasValue);
    }
}

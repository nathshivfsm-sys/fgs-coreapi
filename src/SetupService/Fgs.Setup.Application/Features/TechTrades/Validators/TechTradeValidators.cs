using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.PatchTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;
using FluentValidation;

namespace Fgs.Setup.Application.Features.TechTrades.Validators;

public sealed class CreateTechTradeCommandValidator : AbstractValidator<CreateTechTradeCommand>
{
    public CreateTechTradeCommandValidator(ITechTradeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.TradeCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (command, tradeCode, cancellationToken) =>
                !await readRepository.ExistsByTradeCodeAsync(tradeCode, null, cancellationToken))
            .WithMessage("A tech trade with this trade code already exists.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active tech trade with this name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class UpdateTechTradeCommandValidator : AbstractValidator<UpdateTechTradeCommand>
{
    public UpdateTechTradeCommandValidator(ITechTradeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.TradeCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (command, tradeCode, cancellationToken) =>
                !await readRepository.ExistsByTradeCodeAsync(tradeCode, command.Id, cancellationToken))
            .WithMessage("A tech trade with this trade code already exists.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active tech trade with this name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}

public sealed class PatchTechTradeCommandValidator : AbstractValidator<PatchTechTradeCommand>
{
    public PatchTechTradeCommandValidator(ITechTradeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.TradeCode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (command, tradeCode, cancellationToken) =>
                !await readRepository.ExistsByTradeCodeAsync(tradeCode!, command.Id, cancellationToken))
            .WithMessage("A tech trade with this trade code already exists.")
            .When(x => x.Dto.TradeCode is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active tech trade with this name already exists.")
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(2000)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.SortOrder)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.SortOrder.HasValue);
    }
}

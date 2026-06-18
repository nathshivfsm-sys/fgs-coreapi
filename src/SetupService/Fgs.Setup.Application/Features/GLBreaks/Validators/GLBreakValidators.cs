using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.PatchGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;
using FluentValidation;

namespace Fgs.Setup.Application.Features.GLBreaks.Validators;

public sealed class CreateGLBreakCommandValidator : AbstractValidator<CreateGLBreakCommand>
{
    public CreateGLBreakCommandValidator(
        IGLBreakReadRepository glBreakReadRepository,
        ITechTradeReadRepository techTradeReadRepository)
    {
        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MustAsync(async (command, code, cancellationToken) =>
                !await glBreakReadRepository.ExistsByCodeAndBreakLevelAsync(
                    code, command.Dto.BreakLevel, null, cancellationToken))
            .WithMessage("A GL break with this code and break level already exists.");

        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.BreakLevel)
            .InclusiveBetween((short)1, (short)2)
            .WithMessage("BreakLevel must be 1 or 2.");

        RuleFor(x => x.Dto.TradeCodes)
            .NotNull()
            .Must(codes => (codes ?? []).Select(NormalizeTradeCode).Distinct().Count() == codes!.Count)
            .WithMessage("TradeCodes must not contain duplicates.");

        RuleForEach(x => x.Dto.TradeCodes)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, NormalizeTradeCode(code), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (tradeCode, cancellationToken) =>
                await techTradeReadRepository.ExistsActiveTradeCodeAsync(tradeCode, cancellationToken))
            .WithMessage("TradeCode must reference an active tech trade.");

        RuleFor(x => x.Dto.Address)
            .SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.Address is not null);
    }

    private static string NormalizeTradeCode(string tradeCode) => tradeCode.Trim().ToUpperInvariant();
}

public sealed class UpdateGLBreakCommandValidator : AbstractValidator<UpdateGLBreakCommand>
{
    public UpdateGLBreakCommandValidator(
        IGLBreakReadRepository glBreakReadRepository,
        ITechTradeReadRepository techTradeReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MustAsync(async (command, code, cancellationToken) =>
                !await glBreakReadRepository.ExistsByCodeAndBreakLevelAsync(
                    code, command.Dto.BreakLevel, command.Id, cancellationToken))
            .WithMessage("A GL break with this code and break level already exists.");

        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.BreakLevel)
            .InclusiveBetween((short)1, (short)2)
            .WithMessage("BreakLevel must be 1 or 2.");

        RuleFor(x => x.Dto.TradeCodes)
            .NotNull()
            .Must(codes => (codes ?? []).Select(NormalizeTradeCode).Distinct().Count() == codes!.Count)
            .WithMessage("TradeCodes must not contain duplicates.");

        RuleForEach(x => x.Dto.TradeCodes)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, NormalizeTradeCode(code), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (tradeCode, cancellationToken) =>
                await techTradeReadRepository.ExistsActiveTradeCodeAsync(tradeCode, cancellationToken))
            .WithMessage("TradeCode must reference an active tech trade.");

        RuleFor(x => x.Dto.Address)
            .SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.Address is not null);
    }

    private static string NormalizeTradeCode(string tradeCode) => tradeCode.Trim().ToUpperInvariant();
}

public sealed class PatchGLBreakCommandValidator : AbstractValidator<PatchGLBreakCommand>
{
    public PatchGLBreakCommandValidator(
        IGLBreakReadRepository glBreakReadRepository,
        ITechTradeReadRepository techTradeReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .MustAsync(async (command, code, cancellationToken) =>
            {
                short breakLevel = command.Dto.BreakLevel
                    ?? await glBreakReadRepository.GetBreakLevelByIdAsync(command.Id, cancellationToken)
                    ?? 1;
                return !await glBreakReadRepository.ExistsByCodeAndBreakLevelAsync(
                    code!, breakLevel, command.Id, cancellationToken);
            })
            .WithMessage("A GL break with this code and break level already exists.")
            .When(x => x.Dto.Code is not null);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.BreakLevel)
            .InclusiveBetween((short)1, (short)2)
            .WithMessage("BreakLevel must be 1 or 2.")
            .When(x => x.Dto.BreakLevel.HasValue);

        RuleFor(x => x.Dto.TradeCodes)
            .Must(codes => codes!.Select(NormalizeTradeCode).Distinct().Count() == codes!.Count)
            .WithMessage("TradeCodes must not contain duplicates.")
            .When(x => x.Dto.TradeCodes is not null);

        RuleForEach(x => x.Dto.TradeCodes)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, NormalizeTradeCode(code!), StringComparison.Ordinal))
            .WithMessage("TradeCode must be uppercase.")
            .MustAsync(async (tradeCode, cancellationToken) =>
                await techTradeReadRepository.ExistsActiveTradeCodeAsync(tradeCode!, cancellationToken))
            .WithMessage("TradeCode must reference an active tech trade.")
            .When(x => x.Dto.TradeCodes is not null);

        RuleFor(x => x.Dto.Address)
            .SetValidator(new LocationWriteDtoValidator()!)
            .When(x => x.Dto.Address is not null);
    }

    private static string NormalizeTradeCode(string tradeCode) => tradeCode.Trim().ToUpperInvariant();
}

internal sealed class LocationWriteDtoValidator : AbstractValidator<LocationWriteDto>
{
    public LocationWriteDtoValidator()
    {
        RuleFor(x => x.AddressLine1).MaximumLength(200);
        RuleFor(x => x.AddressLine2).MaximumLength(200);
        RuleFor(x => x.AddressLine3).MaximumLength(200);
        RuleFor(x => x.AddressLine4).MaximumLength(200);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.County).MaximumLength(100);
        RuleFor(x => x.Country).MaximumLength(100);
        RuleFor(x => x.PostalCode).MaximumLength(20);
        RuleFor(x => x.FormattedAddress).MaximumLength(1000);
        RuleFor(x => x.PlaceId).MaximumLength(500);
    }
}

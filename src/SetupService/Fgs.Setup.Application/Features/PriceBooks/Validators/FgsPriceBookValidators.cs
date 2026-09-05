using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Commands.CreateFgsPriceBook;
using Fgs.Setup.Application.Features.PriceBooks.Commands.PatchFgsPriceBook;
using Fgs.Setup.Application.Features.PriceBooks.Commands.UpdateFgsPriceBook;
using Fgs.Setup.Domain.Entities;
using FluentValidation;

namespace Fgs.Setup.Application.Features.PriceBooks.Validators;

public sealed class CreateFgsPriceBookCommandValidator : AbstractValidator<CreateFgsPriceBookCommand>
{
    public CreateFgsPriceBookCommandValidator(IFgsPriceBookReadRepository readRepository)
    {
        RuleFor(x => x.Dto.PriceBookCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.PriceBookCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A price book with this code already exists.");
        RuleFor(x => x.Dto.PriceBookName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.JobTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsJobTypeIdAsync(id, cancellationToken))
            .WithMessage("The specified job type was not found.");
        RuleFor(x => x.Dto.PricingModel)
            .Must(PriceBookValidation.IsAllowedPricingModel)
            .WithMessage("Pricing model must be 'Flat Rate' or 'Dynamic'.");
        RuleFor(x => x.Dto.EstimatedDurationMinutes).GreaterThan(0);
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.FlatRate || dto.BasePrice.HasValue)
            .WithMessage("Base price is required when pricing model is Flat Rate.");
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.Dynamic || dto.BasePrice is null)
            .WithMessage("Base price must be null when pricing model is Dynamic.");
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m).When(x => x.Dto.BasePrice.HasValue);
    }
}

public sealed class UpdateFgsPriceBookCommandValidator : AbstractValidator<UpdateFgsPriceBookCommand>
{
    public UpdateFgsPriceBookCommandValidator(IFgsPriceBookReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.PriceBookCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A price book with this code already exists.");
        RuleFor(x => x.Dto.PriceBookName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.JobTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.JobTypeId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsJobTypeIdAsync(id, cancellationToken))
            .WithMessage("The specified job type was not found.");
        RuleFor(x => x.Dto.PricingModel)
            .Must(PriceBookValidation.IsAllowedPricingModel)
            .WithMessage("Pricing model must be 'Flat Rate' or 'Dynamic'.");
        RuleFor(x => x.Dto.EstimatedDurationMinutes).GreaterThan(0);
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.FlatRate || dto.BasePrice.HasValue)
            .WithMessage("Base price is required when pricing model is Flat Rate.");
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.Dynamic || dto.BasePrice is null)
            .WithMessage("Base price must be null when pricing model is Dynamic.");
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m).When(x => x.Dto.BasePrice.HasValue);
    }
}

public sealed class PatchFgsPriceBookCommandValidator : AbstractValidator<PatchFgsPriceBookCommand>
{
    public PatchFgsPriceBookCommandValidator(IFgsPriceBookReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookCode).NotEmpty().MaximumLength(50).When(x => x.Dto.PriceBookCode is not null);
        RuleFor(x => x.Dto.PriceBookCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A price book with this code already exists.")
            .When(x => x.Dto.PriceBookCode is not null);
        RuleFor(x => x.Dto.PriceBookName).NotEmpty().MaximumLength(200).When(x => x.Dto.PriceBookName is not null);
        RuleFor(x => x.Dto.JobTypeId).GreaterThan(0).When(x => x.Dto.JobTypeId.HasValue);
        RuleFor(x => x.Dto.JobTypeId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsJobTypeIdAsync(id!.Value, cancellationToken))
            .WithMessage("The specified job type was not found.")
            .When(x => x.Dto.JobTypeId.HasValue);
        RuleFor(x => x.Dto.PricingModel)
            .Must(model => model is null || PriceBookValidation.IsAllowedPricingModel(model))
            .WithMessage("Pricing model must be 'Flat Rate' or 'Dynamic'.")
            .When(x => x.Dto.PricingModel is not null);
        RuleFor(x => x.Dto.EstimatedDurationMinutes).GreaterThan(0).When(x => x.Dto.EstimatedDurationMinutes.HasValue);
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m).When(x => x.Dto.BasePrice.HasValue);
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.FlatRate || dto.BasePrice.HasValue)
            .WithMessage("Base price is required when pricing model is Flat Rate.")
            .When(x => x.Dto.PricingModel == PriceBookPricingModels.FlatRate);
        RuleFor(x => x.Dto)
            .Must(dto => dto.PricingModel != PriceBookPricingModels.Dynamic || dto.BasePrice is null)
            .WithMessage("Base price must be null when pricing model is Dynamic.")
            .When(x => x.Dto.PricingModel == PriceBookPricingModels.Dynamic);
    }
}

internal static class PriceBookValidation
{
    public static bool IsAllowedPricingModel(string pricingModel) =>
        pricingModel is PriceBookPricingModels.FlatRate or PriceBookPricingModels.Dynamic;
}

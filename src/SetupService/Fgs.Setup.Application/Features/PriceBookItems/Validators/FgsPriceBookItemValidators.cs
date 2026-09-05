using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.CreateFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.DeleteFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.PatchFgsPriceBookItem;
using Fgs.Setup.Application.Features.PriceBookItems.Commands.UpdateFgsPriceBookItem;
using FluentValidation;

namespace Fgs.Setup.Application.Features.PriceBookItems.Validators;

public sealed class CreateFgsPriceBookItemCommandValidator : AbstractValidator<CreateFgsPriceBookItemCommand>
{
    public CreateFgsPriceBookItemCommandValidator(IFgsPriceBookItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.PriceBookId).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsPriceBookIdAsync(id, cancellationToken))
            .WithMessage("The specified price book was not found.");
        RuleFor(x => x.Dto.ItemCode).MaximumLength(50).When(x => x.Dto.ItemCode is not null);
        RuleFor(x => x.Dto.ItemDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.Quantity).GreaterThan(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class UpdateFgsPriceBookItemCommandValidator : AbstractValidator<UpdateFgsPriceBookItemCommand>
{
    public UpdateFgsPriceBookItemCommandValidator(IFgsPriceBookItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookId).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsPriceBookIdAsync(id, cancellationToken))
            .WithMessage("The specified price book was not found.");
        RuleFor(x => x.Dto.ItemCode).MaximumLength(50).When(x => x.Dto.ItemCode is not null);
        RuleFor(x => x.Dto.ItemDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.Quantity).GreaterThan(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class PatchFgsPriceBookItemCommandValidator : AbstractValidator<PatchFgsPriceBookItemCommand>
{
    public PatchFgsPriceBookItemCommandValidator(IFgsPriceBookItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.PriceBookId).GreaterThan(0).When(x => x.Dto.PriceBookId.HasValue);
        RuleFor(x => x.Dto.PriceBookId)
            .MustAsync(async (_, id, cancellationToken) =>
                await readRepository.ExistsPriceBookIdAsync(id!.Value, cancellationToken))
            .WithMessage("The specified price book was not found.")
            .When(x => x.Dto.PriceBookId.HasValue);
        RuleFor(x => x.Dto.ItemCode).MaximumLength(50).When(x => x.Dto.ItemCode is not null);
        RuleFor(x => x.Dto.ItemDescription).NotEmpty().MaximumLength(500).When(x => x.Dto.ItemDescription is not null);
        RuleFor(x => x.Dto.Quantity).GreaterThan(0m).When(x => x.Dto.Quantity.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class DeleteFgsPriceBookItemCommandValidator : AbstractValidator<DeleteFgsPriceBookItemCommand>
{
    public DeleteFgsPriceBookItemCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

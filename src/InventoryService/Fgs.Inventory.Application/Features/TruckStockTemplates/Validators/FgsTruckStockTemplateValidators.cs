using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.PatchFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Validators;

public sealed class CreateFgsTruckStockTemplateCommandValidator : AbstractValidator<CreateFgsTruckStockTemplateCommand>
{
    public CreateFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.TemplateCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TemplateCode must be uppercase.");
        RuleFor(x => x.Dto.TemplateCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTemplateCodeAsync(code, null, cancellationToken))
            .WithMessage("A truck stock template with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpdateFgsTruckStockTemplateCommandValidator : AbstractValidator<UpdateFgsTruckStockTemplateCommand>
{
    public UpdateFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.TemplateCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TemplateCode must be uppercase.");
        RuleFor(x => x.Dto.TemplateCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTemplateCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A truck stock template with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
    }
}

public sealed class PatchFgsTruckStockTemplateCommandValidator : AbstractValidator<PatchFgsTruckStockTemplateCommand>
{
    public PatchFgsTruckStockTemplateCommandValidator(IFgsTruckStockTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TemplateCode).NotEmpty().MaximumLength(100)
            .When(x => x.Dto.TemplateCode is not null);
        RuleFor(x => x.Dto.TemplateCode!)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("TemplateCode must be uppercase.")
            .When(x => x.Dto.TemplateCode is not null);
        RuleFor(x => x.Dto.TemplateCode!)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByTemplateCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A truck stock template with this code already exists.")
            .When(x => x.Dto.TemplateCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200)
            .When(x => x.Dto.Name is not null);
    }
}

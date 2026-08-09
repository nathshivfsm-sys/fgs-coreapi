using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Commands.PatchFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Commands.UpdateFgsInvoice;
using FluentValidation;

namespace Fgs.Billing.Application.Features.Invoices.Validators;

public sealed class CreateFgsInvoiceCommandValidator : AbstractValidator<CreateFgsInvoiceCommand>
{
    public CreateFgsInvoiceCommandValidator(IFgsInvoiceReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InvoiceNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.InvoiceNumber)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("InvoiceNumber must be uppercase.");
            RuleFor(x => x.Dto.InvoiceNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByInvoiceNumberAsync(number, null, cancellationToken))
                .WithMessage("An invoice with this number already exists.");
            RuleFor(x => x.Dto.InvoiceTypeId).GreaterThan((short)0);
            RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
            RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
            RuleFor(x => x.Dto.ServiceJobNum).MaximumLength(100);
            RuleFor(x => x.Dto.WorkOrderNumber).MaximumLength(50);
            RuleFor(x => x.Dto.CustomerPONumber).MaximumLength(100);
            RuleFor(x => x.Dto.InvoiceSubtotal).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TotalDiscount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TaxableAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TotalTax).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.InvoiceTotal).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.BalanceDue).GreaterThanOrEqualTo(0);
        });
    }
}

public sealed class UpdateFgsInvoiceCommandValidator : AbstractValidator<UpdateFgsInvoiceCommand>
{
    public UpdateFgsInvoiceCommandValidator(IFgsInvoiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InvoiceNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.InvoiceNumber)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("InvoiceNumber must be uppercase.");
            RuleFor(x => x.Dto.InvoiceNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByInvoiceNumberAsync(number, command.Id, cancellationToken))
                .WithMessage("An invoice with this number already exists.");
            RuleFor(x => x.Dto.InvoiceTypeId).GreaterThan((short)0);
            RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
            RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
            RuleFor(x => x.Dto.ServiceJobNum).MaximumLength(100);
            RuleFor(x => x.Dto.WorkOrderNumber).MaximumLength(50);
            RuleFor(x => x.Dto.CustomerPONumber).MaximumLength(100);
            RuleFor(x => x.Dto.InvoiceSubtotal).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TotalDiscount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TaxableAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TotalTax).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.InvoiceTotal).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.BalanceDue).GreaterThanOrEqualTo(0);
        });
    }
}

public sealed class PatchFgsInvoiceCommandValidator : AbstractValidator<PatchFgsInvoiceCommand>
{
    public PatchFgsInvoiceCommandValidator(IFgsInvoiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.InvoiceNumber).NotEmpty().MaximumLength(50).When(x => x.Dto.InvoiceNumber is not null);
            RuleFor(x => x.Dto.InvoiceNumber)
                .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("InvoiceNumber must be uppercase.")
                .When(x => x.Dto.InvoiceNumber is not null);
            RuleFor(x => x.Dto.InvoiceNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByInvoiceNumberAsync(number!, command.Id, cancellationToken))
                .WithMessage("An invoice with this number already exists.")
                .When(x => x.Dto.InvoiceNumber is not null);
            RuleFor(x => x.Dto.InvoiceTypeId).GreaterThan((short)0).When(x => x.Dto.InvoiceTypeId.HasValue);
            RuleFor(x => x.Dto.CustomerId).GreaterThan(0).When(x => x.Dto.CustomerId.HasValue);
            RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0).When(x => x.Dto.ServiceLocationId.HasValue);
            RuleFor(x => x.Dto.ServiceJobNum).MaximumLength(100).When(x => x.Dto.ServiceJobNum is not null);
            RuleFor(x => x.Dto.WorkOrderNumber).MaximumLength(50).When(x => x.Dto.WorkOrderNumber is not null);
            RuleFor(x => x.Dto.CustomerPONumber).MaximumLength(100).When(x => x.Dto.CustomerPONumber is not null);
            RuleFor(x => x.Dto.InvoiceSubtotal).GreaterThanOrEqualTo(0).When(x => x.Dto.InvoiceSubtotal.HasValue);
            RuleFor(x => x.Dto.TotalDiscount).GreaterThanOrEqualTo(0).When(x => x.Dto.TotalDiscount.HasValue);
            RuleFor(x => x.Dto.TaxableAmount).GreaterThanOrEqualTo(0).When(x => x.Dto.TaxableAmount.HasValue);
            RuleFor(x => x.Dto.TotalTax).GreaterThanOrEqualTo(0).When(x => x.Dto.TotalTax.HasValue);
            RuleFor(x => x.Dto.InvoiceTotal).GreaterThanOrEqualTo(0).When(x => x.Dto.InvoiceTotal.HasValue);
            RuleFor(x => x.Dto.BalanceDue).GreaterThanOrEqualTo(0).When(x => x.Dto.BalanceDue.HasValue);
        });
    }
}

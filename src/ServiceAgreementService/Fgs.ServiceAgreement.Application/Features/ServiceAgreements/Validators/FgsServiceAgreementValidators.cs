using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Commands.CreateFgsServiceAgreement;
using FluentValidation;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Validators;

public sealed class CreateFgsServiceAgreementCommandValidator : AbstractValidator<CreateFgsServiceAgreementCommand>
{
    public CreateFgsServiceAgreementCommandValidator(IFgsServiceAgreementReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.AgreementNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.AgreementNumber)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("AgreementNumber must be uppercase.");
            RuleFor(x => x.Dto.AgreementNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByAgreementNumberAsync(number, null, cancellationToken))
                .WithMessage("A service agreement with this number already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
            RuleFor(x => x.Dto.CustomerLocationId).GreaterThan(0);
            RuleFor(x => x.Dto.Break1Id).GreaterThan(0);
            RuleFor(x => x.Dto.Break2Id).GreaterThan(0);
            RuleFor(x => x.Dto.JobTypeId).GreaterThan(0);
            RuleFor(x => x.Dto.ServiceAgreementStatusId).InclusiveBetween((short)1, (short)4);
            RuleFor(x => x.Dto.VisitFrequencyId).GreaterThan((short)0);
            RuleFor(x => x.Dto.BillingFrequencyId).GreaterThan((short)0);
            RuleFor(x => x.Dto.ContractAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.LaborDiscountPercent).InclusiveBetween(0, 100);
            RuleFor(x => x.Dto.MaterialDiscountPercent).InclusiveBetween(0, 100);
            RuleFor(x => x.Dto.EndDate)
                .GreaterThanOrEqualTo(x => x.Dto.StartDate)
                .WithMessage("EndDate must be on or after StartDate.");
            RuleFor(x => x.Dto.ExternalEntityId).MaximumLength(200);
            RuleFor(x => x.Dto.ExternalVersion).MaximumLength(100);
        });
    }
}

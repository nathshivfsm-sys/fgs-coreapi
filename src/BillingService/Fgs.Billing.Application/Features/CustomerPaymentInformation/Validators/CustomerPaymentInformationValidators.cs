using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.CreateCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.DeleteCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.UpdateCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationById;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.ListCustomerPaymentInformation;
using FluentValidation;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Validators;

public sealed class ListCustomerPaymentInformationQueryValidator : AbstractValidator<ListCustomerPaymentInformationQuery>
{
    public ListCustomerPaymentInformationQueryValidator()
    {
        RuleFor(x => x.ArCustomerId).GreaterThan(0).WithMessage("arCustomerId is required.");
        RuleFor(x => x.Query.Page).GreaterThan(0);
        RuleFor(x => x.Query.PageSize).InclusiveBetween(1, 200);
        RuleFor(x => x.Query.Search).MaximumLength(100);
    }
}

public sealed class GetCustomerPaymentInformationByIdQueryValidator : AbstractValidator<GetCustomerPaymentInformationByIdQuery>
{
    public GetCustomerPaymentInformationByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public sealed class CreateCustomerPaymentInformationCommandValidator : AbstractValidator<CreateCustomerPaymentInformationCommand>
{
    public CreateCustomerPaymentInformationCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.ArCustomerId).GreaterThan(0);
            RuleFor(x => x.Dto.CardHolderName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.BillingZip).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Dto.OpaqueData).NotNull();
            When(x => x.Dto.OpaqueData is not null, () =>
            {
                RuleFor(x => x.Dto.OpaqueData.DataDescriptor).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Dto.OpaqueData.DataValue).NotEmpty().MaximumLength(10000);
            });
        });
    }
}

public sealed class UpdateCustomerPaymentInformationCommandValidator : AbstractValidator<UpdateCustomerPaymentInformationCommand>
{
    public UpdateCustomerPaymentInformationCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto).NotNull();
        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CardHolderName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Dto.ExpirationMonth).InclusiveBetween((short)1, (short)12);
            RuleFor(x => x.Dto.ExpirationYear)
                .Must(year => year is >= 0 and <= 99 || year is >= 2000 and <= 2100)
                .WithMessage("ExpirationYear must be a two-digit year or a four-digit year.");
            When(x => x.Dto.OpaqueData is not null, () =>
            {
                RuleFor(x => x.Dto.OpaqueData!.DataDescriptor).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Dto.OpaqueData!.DataValue).NotEmpty().MaximumLength(10000);
            });
        });
    }
}

public sealed class DeleteCustomerPaymentInformationCommandValidator : AbstractValidator<DeleteCustomerPaymentInformationCommand>
{
    public DeleteCustomerPaymentInformationCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

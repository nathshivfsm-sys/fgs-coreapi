using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Tenants.Commands.PatchTenant;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using FluentValidation;

namespace Fgs.User.Application.Features.Tenants.Validators;

public sealed class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.LegalName));
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Dto.Website).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Dto.Website));
        RuleFor(x => x.Dto.DefaultCurrency).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Dto.DefaultCurrency));
    }
}

public sealed class PatchTenantCommandValidator : AbstractValidator<PatchTenantCommand>
{
    public PatchTenantCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.LegalName).MaximumLength(300).When(x => x.Dto.LegalName is not null);
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(300)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PhoneNumber))
            .WithMessage(SignupErrorMessages.InvalidPhoneFormat);
        RuleFor(x => x.Dto.Website).MaximumLength(500).When(x => x.Dto.Website is not null);
        RuleFor(x => x.Dto.DefaultCurrency).MaximumLength(20).When(x => x.Dto.DefaultCurrency is not null);
    }
}

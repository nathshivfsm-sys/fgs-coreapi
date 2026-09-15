using FluentValidation;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;

namespace Fgs.Setup.Application.Features.GloLookups.Validators;

public sealed class LookupGloStateProvincesQueryValidator : AbstractValidator<LookupGloStateProvincesQuery>
{
    public LookupGloStateProvincesQueryValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .MaximumLength(2);
    }
}

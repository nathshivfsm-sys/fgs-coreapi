using FluentValidation;

namespace Fgs.User.Application.Common.Locations;

public sealed class LocationWriteDtoValidator : AbstractValidator<LocationWriteDto>
{
    public LocationWriteDtoValidator()
    {
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AddressLine2).MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.County).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.County));
        RuleFor(x => x.Country).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Country));
        RuleFor(x => x.PlaceId).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.PlaceId));
    }
}

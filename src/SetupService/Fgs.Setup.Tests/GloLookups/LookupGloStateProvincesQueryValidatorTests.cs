using FluentValidation.TestHelper;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;
using Fgs.Setup.Application.Features.GloLookups.Validators;

namespace Fgs.Setup.Tests.GloLookups;

public sealed class LookupGloStateProvincesQueryValidatorTests
{
    private readonly LookupGloStateProvincesQueryValidator _validator = new();

    [Fact]
    public void CountryCode_WhenEmpty_IsInvalid()
    {
        var result = _validator.TestValidate(new LookupGloStateProvincesQuery(string.Empty));
        result.ShouldHaveValidationErrorFor(x => x.CountryCode);
    }

    [Fact]
    public void CountryCode_WhenProvided_IsValid()
    {
        var result = _validator.TestValidate(new LookupGloStateProvincesQuery("US"));
        result.ShouldNotHaveValidationErrorFor(x => x.CountryCode);
    }
}

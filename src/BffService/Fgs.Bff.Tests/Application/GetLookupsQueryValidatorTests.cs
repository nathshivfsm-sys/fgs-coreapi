using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;
using FluentValidation.TestHelper;

namespace Fgs.Bff.Tests.Application;

public sealed class GetLookupsQueryValidatorTests
{
    private readonly GetLookupsQueryValidator _validator = new();

    [Fact]
    public void Validate_EmptyRequests_Fails()
    {
        var result = _validator.TestValidate(new GetLookupsQuery([]));
        result.ShouldHaveValidationErrorFor(x => x.Requests);
    }

    [Fact]
    public void Validate_MoreThanMaxBatch_Fails()
    {
        var oversized = Enum.GetValues<LookupKey>()
            .Take(GetLookupsQueryValidator.MaxBatchSize + 1)
            .Select(k => new LookupRequestDto(k))
            .ToList();

        oversized.Should().HaveCountGreaterThan(GetLookupsQueryValidator.MaxBatchSize);

        var result = _validator.TestValidate(new GetLookupsQuery(oversized));
        result.ShouldHaveValidationErrorFor(x => x.Requests);
    }

    [Fact]
    public void Validate_DuplicateKeys_Fails()
    {
        var result = _validator.TestValidate(new GetLookupsQuery(
        [
            new LookupRequestDto(LookupKey.GloCountry),
            new LookupRequestDto(LookupKey.GloCountry)
        ]));

        result.ShouldHaveValidationErrorFor(x => x.Requests);
    }

    [Fact]
    public void Validate_MissingRequiredCountryCode_Fails()
    {
        var result = _validator.TestValidate(new GetLookupsQuery(
        [
            new LookupRequestDto(LookupKey.GloStateProvince)
        ]));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("countryCode", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_MissingFgsRoleId_Fails()
    {
        var result = _validator.TestValidate(new GetLookupsQuery(
        [
            new LookupRequestDto(LookupKey.RolePermission)
        ]));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("fgsRoleId", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_ValidBatch_Passes()
    {
        var result = _validator.TestValidate(new GetLookupsQuery(
        [
            new LookupRequestDto(LookupKey.GloCountry),
            new LookupRequestDto(LookupKey.GloStateProvince, CountryCode: "US"),
            new LookupRequestDto(LookupKey.RoleMenu, RoleId: 1),
            new LookupRequestDto(LookupKey.UserRole, UserId: Guid.NewGuid())
        ]));

        result.ShouldNotHaveAnyValidationErrors();
    }
}

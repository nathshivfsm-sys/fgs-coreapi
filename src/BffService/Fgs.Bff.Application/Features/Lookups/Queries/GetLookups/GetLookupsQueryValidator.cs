using FluentValidation;

namespace Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;

public sealed class GetLookupsQueryValidator : AbstractValidator<GetLookupsQuery>
{
    public const int MaxBatchSize = 20;

    public GetLookupsQueryValidator()
    {
        RuleFor(x => x.Requests)
            .NotEmpty()
            .WithMessage("At least one lookup request is required.")
            .Must(r => r.Count <= MaxBatchSize)
            .WithMessage($"A maximum of {MaxBatchSize} lookup keys may be requested per batch.")
            .Must(HaveUniqueKeys)
            .WithMessage("Duplicate lookup keys are not allowed in a single batch.");

        RuleForEach(x => x.Requests).ChildRules(request =>
        {
            request.RuleFor(r => r.Key)
                .IsInEnum()
                .WithMessage("Unknown lookup key.");

            request.RuleFor(r => r)
                .Custom(ValidateRequiredFilters);
        });
    }

    private static bool HaveUniqueKeys(IReadOnlyList<Dtos.LookupRequestDto> requests) =>
        requests.Select(r => r.Key).Distinct().Count() == requests.Count;

    private static void ValidateRequiredFilters(
        Dtos.LookupRequestDto request,
        ValidationContext<Dtos.LookupRequestDto> context)
    {
        if (!LookupCatalog.TryGet(request.Key, out var definition))
        {
            context.AddFailure(nameof(request.Key), $"No catalog definition for key '{request.Key}'.");
            return;
        }

        foreach (var filter in definition.RequiredFilters)
        {
            if (HasFilterValue(request, filter))
            {
                continue;
            }

            context.AddFailure(
                filter,
                $"Filter '{filter}' is required for lookup key '{request.Key}'.");
        }
    }

    private static bool HasFilterValue(Dtos.LookupRequestDto request, string filter) =>
        filter switch
        {
            LookupFilterNames.CountryCode => !string.IsNullOrWhiteSpace(request.CountryCode),
            LookupFilterNames.UnitType => !string.IsNullOrWhiteSpace(request.UnitType),
            LookupFilterNames.SourceTypeId => request.SourceTypeId.HasValue,
            LookupFilterNames.JobTypeId => request.JobTypeId.HasValue,
            LookupFilterNames.PriceBookId => request.PriceBookId.HasValue,
            LookupFilterNames.PricingMatrixId => request.PricingMatrixId.HasValue,
            LookupFilterNames.PricingMatrixLaborId => request.PricingMatrixLaborId.HasValue,
            LookupFilterNames.UniversalPricingServiceId => request.UniversalPricingServiceId.HasValue,
            LookupFilterNames.InventoryItemId => request.InventoryItemId.HasValue,
            LookupFilterNames.FgsRoleId => request.FgsRoleId.HasValue,
            LookupFilterNames.RoleId => request.RoleId.HasValue,
            LookupFilterNames.UserId => request.UserId.HasValue,
            LookupFilterNames.ShowToFieldTech => request.ShowToFieldTech.HasValue,
            LookupFilterNames.AllowToPick => request.AllowToPick.HasValue,
            LookupFilterNames.IsMobileVisible => request.IsMobileVisible.HasValue,
            LookupFilterNames.IsCustomerPortalVisible => request.IsCustomerPortalVisible.HasValue,
            LookupFilterNames.StateProvinceCode => !string.IsNullOrWhiteSpace(request.StateProvinceCode),
            _ => true
        };
}

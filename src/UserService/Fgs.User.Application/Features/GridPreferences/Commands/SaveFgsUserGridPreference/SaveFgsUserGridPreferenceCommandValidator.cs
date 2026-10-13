using Fgs.User.Application.Features.GridPreferences;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;

public sealed class SaveFgsUserGridPreferenceCommandValidator : AbstractValidator<SaveFgsUserGridPreferenceCommand>
{
    public SaveFgsUserGridPreferenceCommandValidator()
    {
        RuleFor(x => x.GridCode).ValidGridCode();
        RuleFor(x => x.Columns)
            .NotNull()
            .WithMessage("Columns are required.");

        When(x => x.Columns is not null, () =>
        {
            RuleFor(x => x.Columns!)
                .Must(columns => columns.Count <= FgsUserGridPreferenceLimits.MaxColumnCount)
                .WithMessage("At most 100 column codes are allowed.")
                .Must(HaveDistinctColumnCodes)
                .WithMessage("Column codes must be distinct.");

            RuleForEach(x => x.Columns)
                .Must(column => !string.IsNullOrWhiteSpace(column))
                .WithMessage("Column code is required.")
                .Must(column => column is null || column.Trim().Length <= FgsUserGridPreferenceLimits.MaxCodeLength)
                .WithMessage("Column code must be 100 characters or fewer.");
        });
    }

    private static bool HaveDistinctColumnCodes(IReadOnlyList<string> columns)
    {
        var trimmed = columns.Select(column => column?.Trim() ?? string.Empty).ToList();
        return trimmed.Distinct(StringComparer.Ordinal).Count() == trimmed.Count;
    }
}

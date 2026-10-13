using Fgs.User.Application.Features.GridPreferences;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;

public sealed class GetFgsUserGridPreferenceQueryValidator : AbstractValidator<GetFgsUserGridPreferenceQuery>
{
    public GetFgsUserGridPreferenceQueryValidator()
    {
        RuleFor(x => x.GridCode).ValidGridCode();
    }
}

using Fgs.User.Application.Features.GridPreferences;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;

public sealed class DeleteFgsUserGridPreferenceCommandValidator : AbstractValidator<DeleteFgsUserGridPreferenceCommand>
{
    public DeleteFgsUserGridPreferenceCommandValidator()
    {
        RuleFor(x => x.GridCode).ValidGridCode();
    }
}

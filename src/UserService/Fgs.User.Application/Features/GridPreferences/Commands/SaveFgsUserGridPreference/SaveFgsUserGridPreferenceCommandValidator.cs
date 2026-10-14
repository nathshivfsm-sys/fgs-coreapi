using Fgs.User.Application.Features.GridPreferences;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;

public sealed class SaveFgsUserGridPreferenceCommandValidator : AbstractValidator<SaveFgsUserGridPreferenceCommand>
{
    public SaveFgsUserGridPreferenceCommandValidator()
    {
        RuleFor(x => x.GridCode).ValidGridCode();
        RuleFor(x => x.GridConfiguration)
            .Must(configuration => FgsUserGridPreferenceSupport.TryReadConfiguration(configuration, out _, out _))
            .WithMessage(command =>
                FgsUserGridPreferenceSupport.TryReadConfiguration(command.GridConfiguration, out _, out var error)
                    ? string.Empty
                    : error);
    }
}

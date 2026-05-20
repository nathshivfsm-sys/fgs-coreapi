using FluentValidation;

namespace Fgs.User.Application.Features.Auth.Queries.EntraCallback;

public sealed class EntraCallbackQueryValidator : AbstractValidator<EntraCallbackQuery>
{
    public EntraCallbackQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}

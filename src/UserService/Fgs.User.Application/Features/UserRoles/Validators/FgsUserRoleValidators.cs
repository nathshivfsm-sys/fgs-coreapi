using Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;
using FluentValidation;

namespace Fgs.User.Application.Features.UserRoles.Validators;

public sealed class SyncFgsUserRolesCommandValidator : AbstractValidator<SyncFgsUserRolesCommand>
{
    public SyncFgsUserRolesCommandValidator()
    {
        RuleFor(x => x.Dto.UserId).NotEmpty();
        RuleFor(x => x.Dto.FgsRoleIds).NotNull();
        RuleForEach(x => x.Dto.FgsRoleIds).GreaterThan(0);
    }
}

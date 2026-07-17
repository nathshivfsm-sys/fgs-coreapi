using Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;
using Fgs.User.Application.Features.UserRoles.Commands.DeleteFgsUserRole;
using FluentValidation;

namespace Fgs.User.Application.Features.UserRoles.Validators;

public sealed class CreateFgsUserRoleCommandValidator : AbstractValidator<CreateFgsUserRoleCommand>
{
    public CreateFgsUserRoleCommandValidator()
    {
        RuleFor(x => x.Dto.UserId).NotEmpty();
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
    }
}

public sealed class DeleteFgsUserRoleCommandValidator : AbstractValidator<DeleteFgsUserRoleCommand>
{
    public DeleteFgsUserRoleCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

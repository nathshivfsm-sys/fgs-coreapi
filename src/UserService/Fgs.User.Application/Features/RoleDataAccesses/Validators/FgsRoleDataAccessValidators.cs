using Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.DeleteFgsRoleDataAccess;
using FluentValidation;

namespace Fgs.User.Application.Features.RoleDataAccesses.Validators;

public sealed class CreateFgsRoleDataAccessCommandValidator : AbstractValidator<CreateFgsRoleDataAccessCommand>
{
    public CreateFgsRoleDataAccessCommandValidator()
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsDataAccessId).GreaterThan(0);
    }
}

public sealed class DeleteFgsRoleDataAccessCommandValidator : AbstractValidator<DeleteFgsRoleDataAccessCommand>
{
    public DeleteFgsRoleDataAccessCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

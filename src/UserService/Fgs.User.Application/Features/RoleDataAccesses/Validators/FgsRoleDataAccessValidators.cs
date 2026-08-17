using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using FluentValidation;

namespace Fgs.User.Application.Features.RoleDataAccesses.Validators;

public sealed class SyncFgsRoleDataAccessesCommandValidator : AbstractValidator<SyncFgsRoleDataAccessesCommand>
{
    public SyncFgsRoleDataAccessesCommandValidator()
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsDataAccessIds).NotNull();
        RuleForEach(x => x.Dto.FgsDataAccessIds).GreaterThan(0);
    }
}

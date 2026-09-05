using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.PatchFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.UpdateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.LookupFgsRoleDataAccesses;
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

public sealed class CreateFgsRoleDataAccessCommandValidator : AbstractValidator<CreateFgsRoleDataAccessCommand>
{
    public CreateFgsRoleDataAccessCommandValidator(IFgsRoleDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Dto.FgsRoleId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsDataAccessId)
            .GreaterThan(0)
            .MustAsync(async (command, dataAccessId, cancellationToken) =>
                !await readRepository.ExistsByRoleIdAndDataAccessIdAsync(
                    command.Dto.FgsRoleId,
                    dataAccessId,
                    null,
                    cancellationToken))
            .WithMessage("A role-data-access assignment with this FgsRoleId and FgsDataAccessId already exists.");
    }
}

public sealed class UpdateFgsRoleDataAccessCommandValidator : AbstractValidator<UpdateFgsRoleDataAccessCommand>
{
    public UpdateFgsRoleDataAccessCommandValidator(IFgsRoleDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsDataAccessId)
            .GreaterThan(0)
            .MustAsync(async (command, dataAccessId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByRoleIdAndDataAccessIdAsync(
                    existing.FgsRoleId,
                    dataAccessId,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A role-data-access assignment with this FgsRoleId and FgsDataAccessId already exists.");
    }
}

public sealed class PatchFgsRoleDataAccessCommandValidator : AbstractValidator<PatchFgsRoleDataAccessCommand>
{
    public PatchFgsRoleDataAccessCommandValidator(IFgsRoleDataAccessReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.FgsDataAccessId)
            .GreaterThan(0)
            .MustAsync(async (command, dataAccessId, cancellationToken) =>
            {
                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByRoleIdAndDataAccessIdAsync(
                    existing.FgsRoleId,
                    dataAccessId!.Value,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A role-data-access assignment with this FgsRoleId and FgsDataAccessId already exists.")
            .When(x => x.Dto.FgsDataAccessId.HasValue);
    }
}

public sealed class LookupFgsRoleDataAccessesQueryValidator : AbstractValidator<LookupFgsRoleDataAccessesQuery>
{
    public LookupFgsRoleDataAccessesQueryValidator()
    {
        RuleFor(x => x.FgsRoleId).GreaterThan(0);
    }
}

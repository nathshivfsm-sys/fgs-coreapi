using Fgs.File.Application.Common;
using FluentValidation;

namespace Fgs.File.Application.Features.Files.Queries.GetFilesByEntity;

public sealed class GetFilesByEntityQueryValidator : AbstractValidator<GetFilesByEntityQuery>
{
    public GetFilesByEntityQueryValidator()
    {
        RuleFor(x => x.EntityType)
            .NotEmpty()
            .Must(FileEntityTypes.IsSupported)
            .WithMessage("Unsupported entity type.");
        RuleFor(x => x.EntityId).GreaterThan(0);
    }
}

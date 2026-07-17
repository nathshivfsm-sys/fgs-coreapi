using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Commands.CreateFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.PatchFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.RevokeFgsApiSecret;
using FluentValidation;

namespace Fgs.User.Application.Features.ApiSecrets.Validators;

public sealed class CreateFgsApiSecretCommandValidator : AbstractValidator<CreateFgsApiSecretCommand>
{
    public CreateFgsApiSecretCommandValidator(
        IFgsApiSecretReadRepository readRepository,
        IFgsApiClientReadRepository apiClientReadRepository)
    {
        RuleFor(x => x.Dto.FgsApiClientId).GreaterThan(0);

        RuleFor(x => x.Dto.FgsApiClientId)
            .MustAsync(async (fgsApiClientId, cancellationToken) =>
                await apiClientReadRepository.GetByIdAsync(fgsApiClientId, cancellationToken) is not null)
            .WithMessage("The specified API client was not found.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(command.Dto.FgsApiClientId, name, null, cancellationToken))
            .WithMessage("An API secret with this name already exists for the client.");
    }
}

public sealed class PatchFgsApiSecretCommandValidator : AbstractValidator<PatchFgsApiSecretCommand>
{
    public PatchFgsApiSecretCommandValidator(IFgsApiSecretReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                if (command.Dto.Name is null)
                {
                    return true;
                }

                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                return !await readRepository.ExistsByNameAsync(
                    existing.FgsApiClientId,
                    command.Dto.Name,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("An API secret with this name already exists for the client.")
            .When(x => x.Dto.Name is not null);
    }
}

public sealed class RevokeFgsApiSecretCommandValidator : AbstractValidator<RevokeFgsApiSecretCommand>
{
    public RevokeFgsApiSecretCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

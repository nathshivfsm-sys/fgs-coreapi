using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Commands.CreateFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.PatchFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.UpdateFgsApiClient;
using FluentValidation;

namespace Fgs.User.Application.Features.ApiClients.Validators;

public sealed class CreateFgsApiClientCommandValidator : AbstractValidator<CreateFgsApiClientCommand>
{
    public CreateFgsApiClientCommandValidator(IFgsApiClientReadRepository readRepository)
    {
        RuleFor(x => x.Dto.ApplicationName)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, applicationName, cancellationToken) =>
                !await readRepository.ExistsByApplicationNameAsync(applicationName, null, cancellationToken))
            .WithMessage("An API client with this application name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.ContactName)
            .MaximumLength(100)
            .When(x => x.Dto.ContactName is not null);

        RuleFor(x => x.Dto.ContactEmail)
            .MaximumLength(300)
            .When(x => x.Dto.ContactEmail is not null);

        RuleFor(x => x.Dto.RateLimitPerMinute)
            .GreaterThan(0);
    }
}

public sealed class UpdateFgsApiClientCommandValidator : AbstractValidator<UpdateFgsApiClientCommand>
{
    public UpdateFgsApiClientCommandValidator(IFgsApiClientReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.ApplicationName)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, applicationName, cancellationToken) =>
                !await readRepository.ExistsByApplicationNameAsync(applicationName, command.Id, cancellationToken))
            .WithMessage("An API client with this application name already exists.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.ContactName)
            .MaximumLength(100)
            .When(x => x.Dto.ContactName is not null);

        RuleFor(x => x.Dto.ContactEmail)
            .MaximumLength(300)
            .When(x => x.Dto.ContactEmail is not null);

        RuleFor(x => x.Dto.RateLimitPerMinute)
            .GreaterThan(0);
    }
}

public sealed class PatchFgsApiClientCommandValidator : AbstractValidator<PatchFgsApiClientCommand>
{
    public PatchFgsApiClientCommandValidator(IFgsApiClientReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.ApplicationName)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, applicationName, cancellationToken) =>
                !await readRepository.ExistsByApplicationNameAsync(applicationName!, command.Id, cancellationToken))
            .WithMessage("An API client with this application name already exists.")
            .When(x => x.Dto.ApplicationName is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.ContactName)
            .MaximumLength(100)
            .When(x => x.Dto.ContactName is not null);

        RuleFor(x => x.Dto.ContactEmail)
            .MaximumLength(300)
            .When(x => x.Dto.ContactEmail is not null);

        RuleFor(x => x.Dto.RateLimitPerMinute)
            .GreaterThan(0)
            .When(x => x.Dto.RateLimitPerMinute.HasValue);
    }
}

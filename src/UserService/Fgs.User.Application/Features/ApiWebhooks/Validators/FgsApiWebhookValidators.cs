using Fgs.User.Application.Features.ApiWebhooks.Commands.CreateFgsApiWebhook;
using Fgs.User.Application.Features.ApiWebhooks.Commands.PatchFgsApiWebhook;
using Fgs.User.Application.Features.ApiWebhooks.Commands.UpdateFgsApiWebhook;
using FluentValidation;

namespace Fgs.User.Application.Features.ApiWebhooks.Validators;

public sealed class CreateFgsApiWebhookCommandValidator : AbstractValidator<CreateFgsApiWebhookCommand>
{
    public CreateFgsApiWebhookCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EndpointUrl)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Dto.AuthenticationType)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Dto.AuthenticationValue)
            .MaximumLength(500)
            .When(x => x.Dto.AuthenticationValue is not null);

        RuleFor(x => x.Dto.Secret)
            .MaximumLength(255)
            .When(x => x.Dto.Secret is not null);

        RuleFor(x => x.Dto.TimeoutSeconds)
            .GreaterThan((short)0);

        RuleFor(x => x.Dto.MaximumRetryCount)
            .GreaterThanOrEqualTo((short)0);
    }
}

public sealed class UpdateFgsApiWebhookCommandValidator : AbstractValidator<UpdateFgsApiWebhookCommand>
{
    public UpdateFgsApiWebhookCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EndpointUrl)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Dto.AuthenticationType)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Dto.AuthenticationValue)
            .MaximumLength(500)
            .When(x => x.Dto.AuthenticationValue is not null);

        RuleFor(x => x.Dto.Secret)
            .MaximumLength(255)
            .When(x => x.Dto.Secret is not null);

        RuleFor(x => x.Dto.TimeoutSeconds)
            .GreaterThan((short)0);

        RuleFor(x => x.Dto.MaximumRetryCount)
            .GreaterThanOrEqualTo((short)0);
    }
}

public sealed class PatchFgsApiWebhookCommandValidator : AbstractValidator<PatchFgsApiWebhookCommand>
{
    public PatchFgsApiWebhookCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(255)
            .When(x => x.Dto.Description is not null);

        RuleFor(x => x.Dto.EndpointUrl)
            .NotEmpty()
            .MaximumLength(500)
            .When(x => x.Dto.EndpointUrl is not null);

        RuleFor(x => x.Dto.AuthenticationType)
            .NotEmpty()
            .MaximumLength(30)
            .When(x => x.Dto.AuthenticationType is not null);

        RuleFor(x => x.Dto.AuthenticationValue)
            .MaximumLength(500)
            .When(x => x.Dto.AuthenticationValue is not null);

        RuleFor(x => x.Dto.Secret)
            .MaximumLength(255)
            .When(x => x.Dto.Secret is not null);

        RuleFor(x => x.Dto.TimeoutSeconds)
            .GreaterThan((short)0)
            .When(x => x.Dto.TimeoutSeconds.HasValue);

        RuleFor(x => x.Dto.MaximumRetryCount)
            .GreaterThanOrEqualTo((short)0)
            .When(x => x.Dto.MaximumRetryCount.HasValue);
    }
}

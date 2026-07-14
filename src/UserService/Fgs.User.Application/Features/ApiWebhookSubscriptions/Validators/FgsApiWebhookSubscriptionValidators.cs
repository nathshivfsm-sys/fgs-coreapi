using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.CreateFgsApiWebhookSubscription;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.DeleteFgsApiWebhookSubscription;
using FluentValidation;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Validators;

public sealed class CreateFgsApiWebhookSubscriptionCommandValidator
    : AbstractValidator<CreateFgsApiWebhookSubscriptionCommand>
{
    public CreateFgsApiWebhookSubscriptionCommandValidator()
    {
        RuleFor(x => x.Dto.FgsApiWebhookId).GreaterThan(0);
        RuleFor(x => x.Dto.FgsApiEventId).GreaterThan(0);
    }
}

public sealed class DeleteFgsApiWebhookSubscriptionCommandValidator
    : AbstractValidator<DeleteFgsApiWebhookSubscriptionCommand>
{
    public DeleteFgsApiWebhookSubscriptionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

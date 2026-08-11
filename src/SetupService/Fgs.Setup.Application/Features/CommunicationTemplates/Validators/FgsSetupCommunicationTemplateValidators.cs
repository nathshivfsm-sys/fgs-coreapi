using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.CreateFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.PatchFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.UpdateFgsSetupCommunicationTemplate;
using FluentValidation;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Validators;

public sealed class CreateFgsSetupCommunicationTemplateCommandValidator : AbstractValidator<CreateFgsSetupCommunicationTemplateCommand>
{
    public CreateFgsSetupCommunicationTemplateCommandValidator(IFgsSetupCommunicationTemplateReadRepository readRepository)
    {


        RuleFor(x => x.Dto.CommunicationChannel).NotEmpty();
        RuleFor(x => x.Dto.CommunicationChannel).MaximumLength(25);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(dto.CommunicationChannel, dto.TemplateType, dto.Code, null, cancellationToken))
            .WithMessage("A communication template with this combination already exists.");
        RuleFor(x => x.Dto.TemplateType).NotEmpty();
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.Body).NotEmpty();


        RuleFor(x => x.Dto.CommunicationChannel).NotEmpty();
        RuleFor(x => x.Dto.CommunicationChannel).MaximumLength(25);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(dto.CommunicationChannel, dto.TemplateType, dto.Code, null, cancellationToken))
            .WithMessage("A communication template with this combination already exists.");
        RuleFor(x => x.Dto.TemplateType).NotEmpty();
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.Body).NotEmpty();

    }
}

public sealed class UpdateFgsSetupCommunicationTemplateCommandValidator : AbstractValidator<UpdateFgsSetupCommunicationTemplateCommand>
{
    public UpdateFgsSetupCommunicationTemplateCommandValidator(IFgsSetupCommunicationTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);


        RuleFor(x => x.Dto.CommunicationChannel).NotEmpty();
        RuleFor(x => x.Dto.CommunicationChannel).MaximumLength(25);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(dto.CommunicationChannel, dto.TemplateType, dto.Code, command.Id, cancellationToken))
            .WithMessage("A communication template with this combination already exists.");
        RuleFor(x => x.Dto.TemplateType).NotEmpty();
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty();

        RuleFor(x => x.Dto.Body).NotEmpty();

    }
}

public sealed class PatchFgsSetupCommunicationTemplateCommandValidator : AbstractValidator<PatchFgsSetupCommunicationTemplateCommand>
{
    public PatchFgsSetupCommunicationTemplateCommandValidator(IFgsSetupCommunicationTemplateReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);


        RuleFor(x => x.Dto.CommunicationChannel).NotEmpty().When(x => x.Dto.CommunicationChannel is not null);
        RuleFor(x => x.Dto.CommunicationChannel).MaximumLength(25).When(x => x.Dto.CommunicationChannel is not null);
        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(dto.CommunicationChannel!, dto.TemplateType!, dto.Code!, command.Id, cancellationToken))
            .WithMessage("A communication template with this combination already exists.").When(x => x.Dto.CommunicationChannel is not null && x.Dto.TemplateType is not null && x.Dto.Code is not null);
        RuleFor(x => x.Dto.TemplateType).NotEmpty().When(x => x.Dto.TemplateType is not null);
        RuleFor(x => x.Dto.Code).NotEmpty().When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);

        RuleFor(x => x.Dto.Body).NotEmpty().When(x => x.Dto.Body is not null);

    }
}

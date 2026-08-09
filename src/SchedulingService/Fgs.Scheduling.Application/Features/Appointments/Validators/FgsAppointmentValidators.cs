using Fgs.Scheduling.Application.Features.Appointments.Commands.CreateFgsAppointment;
using FluentValidation;

namespace Fgs.Scheduling.Application.Features.Appointments.Validators;

public sealed class CreateFgsAppointmentCommandValidator : AbstractValidator<CreateFgsAppointmentCommand>
{
    public CreateFgsAppointmentCommandValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.SourceTypeId).GreaterThan((short)0);
            RuleFor(x => x.Dto.SourceId).GreaterThan(0);
            RuleFor(x => x.Dto.CustomerContactName).MaximumLength(200);
            RuleFor(x => x.Dto.EstimatedHours).GreaterThan(0);
            RuleFor(x => x.Dto.AppointmentStatusId).InclusiveBetween((short)1, (short)3);
        });
    }
}

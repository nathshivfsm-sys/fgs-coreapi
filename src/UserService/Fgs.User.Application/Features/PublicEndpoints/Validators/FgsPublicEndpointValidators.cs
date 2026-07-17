using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.PatchFgsPublicEndpoint;
using Fgs.User.Application.Features.PublicEndpoints.Commands.UpdateFgsPublicEndpoint;
using FluentValidation;

namespace Fgs.User.Application.Features.PublicEndpoints.Validators;

public sealed class CreateFgsPublicEndpointCommandValidator : AbstractValidator<CreateFgsPublicEndpointCommand>
{
    public CreateFgsPublicEndpointCommandValidator(IFgsPublicEndpointReadRepository readRepository)
    {
        RuleFor(x => x.Dto.EndpointType)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, PublicEndpointCodes.Normalize(code), StringComparison.Ordinal))
            .WithMessage("EndpointType must be uppercase.")
            .Must(code => PublicEndpointCodes.EndpointTypes.Contains(PublicEndpointCodes.Normalize(code)))
            .WithMessage("EndpointType must be BFF or API.");

        RuleFor(x => x.Dto.EnvironmentCode)
            .NotEmpty()
            .MaximumLength(20)
            .Must(code => string.Equals(code, PublicEndpointCodes.Normalize(code), StringComparison.Ordinal))
            .WithMessage("EnvironmentCode must be uppercase.")
            .Must(code => PublicEndpointCodes.EnvironmentCodes.Contains(PublicEndpointCodes.Normalize(code)))
            .WithMessage("EnvironmentCode must be one of PROD, SANDBOX, TRAINING, QA, PREVIEW, DEVELOPMENT.");

        RuleFor(x => x.Dto.BaseUrl)
            .NotEmpty()
            .MaximumLength(500)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("BaseUrl must be an absolute HTTP or HTTPS URL.");

        RuleFor(x => x.Dto.DisplayName)
            .MaximumLength(100)
            .When(x => x.Dto.DisplayName is not null);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                !await readRepository.ExistsByTypeAndEnvironmentAsync(
                    command.Dto.EndpointType,
                    command.Dto.EnvironmentCode,
                    null,
                    cancellationToken))
            .WithMessage("A public endpoint with this type and environment already exists.");
    }
}

public sealed class UpdateFgsPublicEndpointCommandValidator : AbstractValidator<UpdateFgsPublicEndpointCommand>
{
    public UpdateFgsPublicEndpointCommandValidator(IFgsPublicEndpointReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.EndpointType)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code, PublicEndpointCodes.Normalize(code), StringComparison.Ordinal))
            .WithMessage("EndpointType must be uppercase.")
            .Must(code => PublicEndpointCodes.EndpointTypes.Contains(PublicEndpointCodes.Normalize(code)))
            .WithMessage("EndpointType must be BFF or API.");

        RuleFor(x => x.Dto.EnvironmentCode)
            .NotEmpty()
            .MaximumLength(20)
            .Must(code => string.Equals(code, PublicEndpointCodes.Normalize(code), StringComparison.Ordinal))
            .WithMessage("EnvironmentCode must be uppercase.")
            .Must(code => PublicEndpointCodes.EnvironmentCodes.Contains(PublicEndpointCodes.Normalize(code)))
            .WithMessage("EnvironmentCode must be one of PROD, SANDBOX, TRAINING, QA, PREVIEW, DEVELOPMENT.");

        RuleFor(x => x.Dto.BaseUrl)
            .NotEmpty()
            .MaximumLength(500)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("BaseUrl must be an absolute HTTP or HTTPS URL.");

        RuleFor(x => x.Dto.DisplayName)
            .MaximumLength(100)
            .When(x => x.Dto.DisplayName is not null);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                !await readRepository.ExistsByTypeAndEnvironmentAsync(
                    command.Dto.EndpointType,
                    command.Dto.EnvironmentCode,
                    command.Id,
                    cancellationToken))
            .WithMessage("A public endpoint with this type and environment already exists.");
    }
}

public sealed class PatchFgsPublicEndpointCommandValidator : AbstractValidator<PatchFgsPublicEndpointCommand>
{
    public PatchFgsPublicEndpointCommandValidator(IFgsPublicEndpointReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.EndpointType)
            .NotEmpty()
            .MaximumLength(50)
            .Must(code => string.Equals(code!, PublicEndpointCodes.Normalize(code!), StringComparison.Ordinal))
            .WithMessage("EndpointType must be uppercase.")
            .Must(code => PublicEndpointCodes.EndpointTypes.Contains(PublicEndpointCodes.Normalize(code!)))
            .WithMessage("EndpointType must be BFF or API.")
            .When(x => x.Dto.EndpointType is not null);

        RuleFor(x => x.Dto.EnvironmentCode)
            .NotEmpty()
            .MaximumLength(20)
            .Must(code => string.Equals(code!, PublicEndpointCodes.Normalize(code!), StringComparison.Ordinal))
            .WithMessage("EnvironmentCode must be uppercase.")
            .Must(code => PublicEndpointCodes.EnvironmentCodes.Contains(PublicEndpointCodes.Normalize(code!)))
            .WithMessage("EnvironmentCode must be one of PROD, SANDBOX, TRAINING, QA, PREVIEW, DEVELOPMENT.")
            .When(x => x.Dto.EnvironmentCode is not null);

        RuleFor(x => x.Dto.BaseUrl)
            .NotEmpty()
            .MaximumLength(500)
            .Must(url => Uri.TryCreate(url!, UriKind.Absolute, out var uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("BaseUrl must be an absolute HTTP or HTTPS URL.")
            .When(x => x.Dto.BaseUrl is not null);

        RuleFor(x => x.Dto.DisplayName)
            .MaximumLength(100)
            .When(x => x.Dto.DisplayName is not null);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                if (command.Dto.EndpointType is null && command.Dto.EnvironmentCode is null)
                {
                    return true;
                }

                var existing = await readRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing is null)
                {
                    return true;
                }

                var type = command.Dto.EndpointType ?? existing.EndpointType;
                var environment = command.Dto.EnvironmentCode ?? existing.EnvironmentCode;
                return !await readRepository.ExistsByTypeAndEnvironmentAsync(
                    type,
                    environment,
                    command.Id,
                    cancellationToken);
            })
            .WithMessage("A public endpoint with this type and environment already exists.")
            .When(x => x.Dto.EndpointType is not null || x.Dto.EnvironmentCode is not null);
    }
}

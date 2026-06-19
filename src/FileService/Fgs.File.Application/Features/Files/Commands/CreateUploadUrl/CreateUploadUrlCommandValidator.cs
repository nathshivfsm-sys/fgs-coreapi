using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Common;
using FluentValidation;

namespace Fgs.File.Application.Features.Files.Commands.CreateUploadUrl;

public sealed class CreateUploadUrlCommandValidator : AbstractValidator<CreateUploadUrlCommand>
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpeg",
        "image/webp",
        "image/svg+xml"
    };

    public CreateUploadUrlCommandValidator()
    {
        RuleFor(x => x.Request.FileName).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Request.ContentType)
            .NotEmpty()
            .Must(contentType => AllowedContentTypes.Contains(contentType))
            .WithMessage("Unsupported content type.");
        RuleFor(x => x.Request.FileSizeBytes).GreaterThan(0);
        RuleFor(x => x.Request.EntityType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Request.EntityId).GreaterThan(0);
        RuleFor(x => x.Request.RequestedVariants)
            .NotEmpty()
            .Must(variants => variants.All(FileLogoVariants.IsSupported))
            .WithMessage("One or more requested variants are not supported.");
    }
}

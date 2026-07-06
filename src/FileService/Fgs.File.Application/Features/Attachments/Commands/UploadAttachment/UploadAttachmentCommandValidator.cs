using Fgs.File.Application.Common;
using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;

public sealed class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
{
    public UploadAttachmentCommandValidator(IOptions<AttachmentValidationOptions> validationOptions)
    {
        var options = validationOptions.Value;

        RuleFor(x => x.OriginalFileName).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ContentType).NotEmpty().Must(ct => AttachmentFileValidator.IsAllowedContentType(ct, options))
            .WithMessage("Unsupported content type.");
        RuleFor(x => x.OriginalFileName).Must(name => AttachmentFileValidator.IsAllowedExtension(name, options))
            .WithMessage("Unsupported file extension.");
        RuleFor(x => x.FileSizeBytes).GreaterThan(0);
        RuleFor(x => x.EntityType).NotEmpty().Must(FileEntityTypes.IsSupported).WithMessage("Unsupported entity type.");
        RuleFor(x => x.EntityId).GreaterThan(0);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LogoVariant)
            .Must((cmd, variant) => string.IsNullOrWhiteSpace(variant) || FileLogoVariants.IsSupported(variant))
            .WithMessage("Logo variant is not supported.")
            .When(x => x.Category.Equals("logo", StringComparison.OrdinalIgnoreCase));
    }
}

using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;

namespace Fgs.Setup.Infrastructure.CommunicationTemplates;

internal sealed class FgsSetupCommunicationTemplateSummaryRow
{
    public long Id { get; set; }
    public string CommunicationChannel { get; set; }
    public string TemplateType { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupCommunicationTemplateSummaryDto ToDto() =>
        new(
            Id,
            CommunicationChannel,
            TemplateType,
            Code,
            Name,
            Subject,
            Body,
            IsMobileVisible,
            IsActive);
}

internal sealed class FgsSetupCommunicationTemplateDetailRow
{
    public long Id { get; set; }
    public string CommunicationChannel { get; set; }
    public string TemplateType { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupCommunicationTemplateDetailDto ToDto() =>
        new(
            Id,
            CommunicationChannel,
            TemplateType,
            Code,
            Name,
            Subject,
            Body,
            IsMobileVisible,
            IsActive);
}

internal sealed class FgsSetupCommunicationTemplateLookupRow
{
    public long Id { get; set; }
    public string CommunicationChannel { get; set; }
    public string TemplateType { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public FgsSetupCommunicationTemplateLookupDto ToDto() => new(Id,
            CommunicationChannel,
            TemplateType,
            Code,
            Name);
}

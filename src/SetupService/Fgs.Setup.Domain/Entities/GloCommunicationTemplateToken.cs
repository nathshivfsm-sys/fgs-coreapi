namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Junction table defining valid communication tokens for a communication template.
/// </summary>
public class GloCommunicationTemplateToken
{
    public long CommunicationTemplateId { get; set; }

    public GloCommunicationTemplate CommunicationTemplate { get; set; } = null!;

    public int CommunicationTokenId { get; set; }

    public GloCommunicationToken CommunicationToken { get; set; } = null!;
}

namespace Fgs.Setup.Application.Features.CommunicationTemplates;

internal static class CommunicationTemplateChannelMapper
{
    public static bool TryMapTemplateTypeToCommunicationChannel(
        string templateType,
        out string communicationChannel)
    {
        communicationChannel = templateType.Trim().ToUpperInvariant() switch
        {
            "EMAIL" => "Email",
            "SMS" => "SMS",
            "PUSH" => "PushNotification",
            _ => string.Empty
        };

        return communicationChannel.Length > 0;
    }
}

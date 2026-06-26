namespace Fgs.Notification.Application.Notifications.Templates;

public sealed class TemplateRenderingException : Exception
{
    public TemplateRenderingException(string message)
        : base(message)
    {
    }

    public TemplateRenderingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public IReadOnlyList<string> MissingTokens { get; init; } = [];
}

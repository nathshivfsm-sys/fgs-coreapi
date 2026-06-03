namespace Fgs.Notification.Application.Notifications.Templates;

public interface ITemplateRenderer
{
    /// <summary>
    /// Replaces {{Token}} placeholders and validates that all required tokens are supplied.
    /// </summary>
    string Render(string content, IReadOnlyDictionary<string, string> tokens);
}

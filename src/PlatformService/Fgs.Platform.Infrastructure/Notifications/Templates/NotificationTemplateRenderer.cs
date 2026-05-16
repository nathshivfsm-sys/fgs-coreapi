using System.Net;
using Fgs.Platform.Application.Notifications.Templates;

namespace Fgs.Platform.Infrastructure.Notifications.Templates;

public sealed class NotificationTemplateRenderer : INotificationTemplateRenderer
{
    private static readonly Dictionary<string, Func<IReadOnlyDictionary<string, string>, RenderedNotificationTemplate>> Templates =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["user-invite"] = data => BuildInvite("You're invited to FGS", data),
            ["password-reset"] = data => BuildInvite("Reset your FGS password", data),
            ["company-created"] = data => BuildCompanyCreated(data),
            ["CompanySignupInviteEmail"] = data => BuildInvite("Complete your FGS company signup", data),
            ["UserInvited"] = data => BuildInvite("You're invited to FGS", data),
            ["PasswordReset"] = data => BuildInvite("Reset your FGS password", data),
            ["CompanyCreated"] = data => BuildCompanyCreated(data)
        };

    public RenderedNotificationTemplate Render(
        string templateName,
        IReadOnlyDictionary<string, string> data)
    {
        if (Templates.TryGetValue(templateName, out var factory))
        {
            return factory(data);
        }

        var subject = data.TryGetValue("Subject", out var s) ? s : "FGS Notification";
        var body = data.TryGetValue("Body", out var b) ? b : "You have a new notification.";
        return new RenderedNotificationTemplate(subject, $"<p>{WebUtility.HtmlEncode(body)}</p>", body);
    }

    private static RenderedNotificationTemplate BuildInvite(
        string subject,
        IReadOnlyDictionary<string, string> data)
    {
        var name = data.GetValueOrDefault("DisplayName") ?? "there";
        var url = data.GetValueOrDefault("InviteUrl") ?? data.GetValueOrDefault("ResetUrl") ?? "#";
        var html =
            $"<p>Hello {WebUtility.HtmlEncode(name)},</p>" +
            $"<p><a href=\"{WebUtility.HtmlEncode(url)}\">Click here to continue</a></p>";
        var text = $"Hello {name}, continue at {url}";
        return new RenderedNotificationTemplate(subject, html, text);
    }

    private static RenderedNotificationTemplate BuildCompanyCreated(IReadOnlyDictionary<string, string> data)
    {
        var company = data.GetValueOrDefault("CompanyName") ?? "your company";
        var subject = $"Welcome to FGS — {company}";
        var html = $"<p>Your company <strong>{WebUtility.HtmlEncode(company)}</strong> has been created.</p>";
        return new RenderedNotificationTemplate(subject, html, $"Company {company} created.");
    }
}

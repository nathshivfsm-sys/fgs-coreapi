using System.Net;

namespace Fgs.Notification.Infrastructure.Notifications.Templates;

internal static class NotificationEmailBodyFormatter
{
    public static string ToHtmlBody(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
        {
            return string.Empty;
        }

        var normalized = plainText.Replace("\r\n", "\n").Trim();
        var paragraphs = new List<string>();
        var currentLines = new List<string>();

        foreach (var line in normalized.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentLines.Count > 0)
                {
                    paragraphs.Add(FormatParagraph(currentLines));
                    currentLines.Clear();
                }

                continue;
            }

            currentLines.Add(line);
        }

        if (currentLines.Count > 0)
        {
            paragraphs.Add(FormatParagraph(currentLines));
        }

        return string.Concat(paragraphs.Select(p => $"<p>{p}</p>"));
    }

    private static string FormatParagraph(IReadOnlyList<string> lines) =>
        string.Join("<br/>", lines.Select(line => WebUtility.HtmlEncode(line)));
}

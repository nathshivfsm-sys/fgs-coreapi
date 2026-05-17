using System.Text.RegularExpressions;
using Fgs.Platform.Application.Notifications.Templates;

namespace Fgs.Platform.Infrastructure.Notifications.Templates;

public sealed partial class TemplateRenderer : ITemplateRenderer
{
    private static readonly Regex TokenPattern = TokenRegex();

    public string Render(string content, IReadOnlyDictionary<string, string> tokens)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentNullException.ThrowIfNull(tokens);

        var requiredTokens = ExtractTokenNames(content);
        var missing = requiredTokens
            .Where(name => !tokens.ContainsKey(name) || string.IsNullOrEmpty(tokens[name]))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

        if (missing.Count > 0)
        {
            throw new TemplateRenderingException(
                $"Template is missing required token value(s): {string.Join(", ", missing)}.")
            {
                MissingTokens = missing
            };
        }

        return TokenPattern.Replace(content, match =>
        {
            var tokenName = match.Groups[1].Value;
            return tokens[tokenName];
        });
    }

    public static HashSet<string> ExtractTokenNames(string content)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (Match match in TokenPattern.Matches(content))
        {
            names.Add(match.Groups[1].Value);
        }

        return names;
    }

    [GeneratedRegex(@"\{\{([A-Za-z][A-Za-z0-9_]*)\}\}", RegexOptions.Compiled)]
    private static partial Regex TokenRegex();
}

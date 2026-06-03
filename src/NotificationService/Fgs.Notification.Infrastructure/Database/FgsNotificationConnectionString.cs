using Microsoft.Extensions.Configuration;

namespace Fgs.Notification.Infrastructure.Database;

public static class FgsNotificationConnectionString
{
    public const string Name = "FgsNotification";

    public static string ResolveRequired(IConfiguration configuration)
    {
        var value = configuration.GetConnectionString(Name);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Connection string '{Name}' is not configured.");
        }

        return value;
    }
}

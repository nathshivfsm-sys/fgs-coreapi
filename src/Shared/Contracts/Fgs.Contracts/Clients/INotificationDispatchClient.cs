using Fgs.Contracts.Requests;
using Refit;

namespace Fgs.Contracts.Clients;

public interface INotificationDispatchClient
{
    [Post("/api/v1/notification/dispatch")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> DispatchAsync(
        [Body] DispatchNotificationRequest request,
        CancellationToken cancellationToken = default);
}

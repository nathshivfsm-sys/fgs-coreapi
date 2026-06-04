using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Notification.Application.Features.Credentials.Commands.ReloadCredentialConfiguration;

public sealed record ReloadCredentialConfigurationCommand : IRequest<ApiResponse<object>>;

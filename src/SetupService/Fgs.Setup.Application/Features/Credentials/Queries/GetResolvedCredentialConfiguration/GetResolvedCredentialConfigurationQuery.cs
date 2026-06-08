using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetResolvedCredentialConfiguration;

public sealed record GetResolvedCredentialConfigurationQuery(
    string? InternalServiceKey,
    string? RequestingServiceName) : IRequest<ApiResponse<ResolvedCredentialConfigurationDto>>;

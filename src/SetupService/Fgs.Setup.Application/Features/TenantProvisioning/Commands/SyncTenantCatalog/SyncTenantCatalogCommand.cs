using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;

public sealed record SyncTenantCatalogCommand : IRequest<ApiResponse<TenantCatalogSyncResultDto>>;

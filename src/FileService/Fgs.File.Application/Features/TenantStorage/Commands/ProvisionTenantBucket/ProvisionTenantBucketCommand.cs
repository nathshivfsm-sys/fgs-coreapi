using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.TenantStorage.Commands.ProvisionTenantBucket;

public sealed record ProvisionTenantBucketCommand(long TenantId, ProvisionTenantBucketRequest Request)
    : IRequest<ApiResponse<ProvisionTenantBucketResponse>>;

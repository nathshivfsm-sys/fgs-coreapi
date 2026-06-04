using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.File.Application.Features.TenantStorage.Commands.InitializeTenantFolders;

public sealed record InitializeTenantFoldersCommand(long TenantId, InitializeTenantFoldersRequest Request)
    : IRequest<ApiResponse<object>>;

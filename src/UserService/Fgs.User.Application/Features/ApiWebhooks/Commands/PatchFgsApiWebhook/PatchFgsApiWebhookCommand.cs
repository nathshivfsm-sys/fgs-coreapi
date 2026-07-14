using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.PatchFgsApiWebhook;

public sealed record PatchFgsApiWebhookCommand(long Id, FgsApiWebhookPatchDto Dto)
    : IRequest<ApiResponse<FgsApiWebhookDetailDto>>;

using Fgs.Contracts.Api;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;

public sealed record GetFgsUserGridPreferenceQuery(string GridCode)
    : IRequest<ApiResponse<FgsUserGridPreferenceDto>>;

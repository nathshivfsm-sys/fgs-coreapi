using Fgs.Contracts.Api;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Auth.Commands.StartLogin;

public sealed class StartLoginCommandHandler(
    IUnitOfWork unitOfWork,
    IEntraExternalIdService entraService,
    IEmailNormalizer emailNormalizer,
    IConfiguration configuration) : IRequestHandler<StartLoginCommand, ApiResponse<StartLoginResultDto>>
{
    public async Task<ApiResponse<StartLoginResultDto>> Handle(
        StartLoginCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = emailNormalizer.Normalize(request.Email);

        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultIgnoreFiltersAsync(
                u => !u.IsDeleted
                     && u.IsActive
                     && u.Email.ToUpper() == normalizedEmail,
                cancellationToken);

        if (user is null)
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.LoginNotAvailable],
                ApiStatusCodes.BadRequest);
        }

        var redirectUri = configuration[ConfigurationKeys.EntraExternalId.RedirectUri]
            ?? ApplicationUrlDefaults.EntraCallbackRedirect;

        var state = $"{OAuthStatePrefixes.UserLogin}{user.Id}";
        var redirectUrl = entraService.BuildAuthorizationUrl(state, redirectUri, user.Email);

        return ApiResponse<StartLoginResultDto>.Ok(new StartLoginResultDto(redirectUrl));
    }
}

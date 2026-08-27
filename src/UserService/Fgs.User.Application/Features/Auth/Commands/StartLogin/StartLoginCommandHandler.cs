using Fgs.Contracts.Api;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
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
    IInvitationReadQuery invitationReadQuery,
    ILoginPkceStore loginPkceStore,
    IConfiguration configuration) : IRequestHandler<StartLoginCommand, ApiResponse<StartLoginResultDto>>
{
    public async Task<ApiResponse<StartLoginResultDto>> Handle(
        StartLoginCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = emailNormalizer.Normalize(request.Email);

        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultIgnoreFiltersAsync(
                u => !u.IsDeleted && u.Email.ToUpper() == normalizedEmail,
                cancellationToken);

        if (user is null)
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.LoginNotAvailable],
                ApiStatusCodes.BadRequest);
        }

        if (!user.IsActive)
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.UserNotActive],
                ApiStatusCodes.Forbidden);
        }

        var hasEntraBinding = !string.IsNullOrWhiteSpace(user.EntraObjectId);
        if (!hasEntraBinding
            && !await invitationReadQuery.HasAcceptedInvitationForUserAsync(user.Id, cancellationToken))
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.InvitationNotAccepted],
                ApiStatusCodes.Forbidden);
        }

        var tenant = await unitOfWork.Repository<FgsTenant>()
            .GetByIdAsync(user.TenantId, cancellationToken);
        if (tenant is null || !tenant.IsActive)
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.TenantNotActive],
                ApiStatusCodes.Forbidden);
        }

        var companyCache = await unitOfWork.Repository<FgsTenantCompanyCache>()
            .FirstOrDefaultIgnoreFiltersAsync(
                c => c.TenantId == user.TenantId && c.CompanyId == user.CompanyId,
                cancellationToken);
        if (companyCache is null || !companyCache.IsActive)
        {
            return ApiResponse<StartLoginResultDto>.Fail(
                [AuthErrorMessages.CompanyNotActive],
                ApiStatusCodes.Forbidden);
        }

        var redirectUri = ApplicationPublicUrlResolver.ResolveLoginRedirect(configuration);

        var state = $"{OAuthStatePrefixes.UserLogin}{user.Id}";
        var (codeVerifier, codeChallenge) = EntraExternalIdPkce.CreatePair();
        await loginPkceStore.SaveAsync(
            state,
            new LoginPkceState(codeVerifier, redirectUri, user.Id),
            cancellationToken);

        var userFlow = EntraUserFlowResolver.Resolve(
            user.AuthenticationMethod,
            configuration[ConfigurationKeys.EntraExternalId.UserFlow],
            configuration[ConfigurationKeys.EntraExternalId.PasswordUserFlow]);

        var redirectUrl = entraService.BuildLoginAuthorizationUrl(
            state,
            redirectUri,
            codeChallenge,
            user.Email,
            userFlow);

        return ApiResponse<StartLoginResultDto>.Ok(new StartLoginResultDto(redirectUrl));
    }
}

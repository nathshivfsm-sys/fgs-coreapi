using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Features.Signup;

public sealed class SignupUniquenessValidator : ISignupUniquenessValidator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IDateTimeProvider _dateTime;
    private readonly IInvitationReadQuery _invitationReadQuery;

    public SignupUniquenessValidator(
        IUnitOfWork unitOfWork,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime,
        IInvitationReadQuery invitationReadQuery)
    {
        _unitOfWork = unitOfWork;
        _emailNormalizer = emailNormalizer;
        _dateTime = dateTime;
        _invitationReadQuery = invitationReadQuery;
    }

    public async Task<IReadOnlyList<string>> ValidateAsync(
        CreateCompanySignupCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = _emailNormalizer.Normalize(command.Contact.Email);
        var userRepo = _unitOfWork.Repository<FgsUser>();

        var existingUser = await userRepo.FirstOrDefaultIgnoreFiltersAsync(
            u => !u.IsDeleted && u.Email.ToUpper() == normalizedEmail,
            cancellationToken);

        if (existingUser is not null)
        {
            return [SignupErrorMessages.EmailAlreadyUsed];
        }

        if (await _invitationReadQuery.HasPendingInvitationForNormalizedEmailAsync(
                normalizedEmail,
                _dateTime.UtcNow,
                cancellationToken))
        {
            return [SignupErrorMessages.EmailAlreadyUsed];
        }

        return [];
    }
}

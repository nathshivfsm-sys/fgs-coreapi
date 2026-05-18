using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Signup;

public sealed class SignupUniquenessValidator : ISignupUniquenessValidator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IDateTimeProvider _dateTime;

    public SignupUniquenessValidator(
        IUnitOfWork unitOfWork,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime)
    {
        _unitOfWork = unitOfWork;
        _emailNormalizer = emailNormalizer;
        _dateTime = dateTime;
    }

    public async Task<IReadOnlyList<string>> ValidateAsync(
        CreateCompanySignupCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = _emailNormalizer.Normalize(command.Contact.Email);
        var userRepo = _unitOfWork.Repository<FgsUser>();
        var invitationRepo = _unitOfWork.Repository<FgsInvitation>();

        if (await EmailExistsAsync(userRepo, invitationRepo, normalizedEmail, cancellationToken))
        {
            return ["This email address is already associated with an account or pending invitation."];
        }

        return [];
    }

    private async Task<bool> EmailExistsAsync(
        IRepository<FgsUser> userRepo,
        IRepository<FgsInvitation> invitationRepo,
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        if (await userRepo.AnyAsync(
                u => !u.IsDeleted && u.Email.ToUpper() == normalizedEmail,
                cancellationToken))
        {
            return true;
        }

        var now = _dateTime.UtcNow;
        var invitations = await invitationRepo.ListAsync(
            i => !i.IsDeleted
                && i.Status == InvitationStatus.Pending
                && i.ExpiresAtUtc > now,
            cancellationToken);

        return invitations.Any(i => _emailNormalizer.Normalize(i.Email) == normalizedEmail);
    }
}

using Fgs.Persistence.Implementations;
using Fgs.Setup.Infrastructure.Database;
using Fgs.User.Application.Abstractions.Persistence;

namespace Fgs.User.Infrastructure.Database.UnitOfWorks;

public sealed class SetupUnitOfWork(FgsSetupDbContext context)
    : EfUnitOfWork<FgsSetupDbContext>(context), ISetupUnitOfWork;

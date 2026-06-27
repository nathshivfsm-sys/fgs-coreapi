using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database.Schemas;

namespace Fgs.User.Infrastructure.Persistence.Read;

internal enum UserEntityIdKind
{
    Long,
    Guid
}

internal sealed record UserEntityReadDescriptor(string Table, UserEntityIdKind IdKind);

internal static class UserEntityReadRegistry
{
    private static readonly Dictionary<Type, UserEntityReadDescriptor> Descriptors = BuildDescriptors();

    public static UserEntityReadDescriptor GetDescriptor<TEntity>() where TEntity : class =>
        Descriptors.TryGetValue(typeof(TEntity), out var descriptor)
            ? descriptor
            : throw new InvalidOperationException(
                $"No read registry entry for entity '{typeof(TEntity).Name}'. Add it to UserEntityReadRegistry.");

    private static Dictionary<Type, UserEntityReadDescriptor> BuildDescriptors() => new()
    {
        [typeof(FgsTenant)] = new(Qualify("FgsTenant"), UserEntityIdKind.Long),
        [typeof(FgsTenantCompany)] = new(Qualify("FgsTenantCompany"), UserEntityIdKind.Long),
        [typeof(FgsLocation)] = new(Qualify("FgsLocation"), UserEntityIdKind.Guid),
        [typeof(FgsUser)] = new(Qualify("FgsUser"), UserEntityIdKind.Guid),
        [typeof(FgsInvitation)] = new(Qualify("FgsInvitation"), UserEntityIdKind.Guid),
        [typeof(FgsUserRole)] = new(Qualify("FgsUserRole"), UserEntityIdKind.Long),
        [typeof(FgsRole)] = new(Qualify("FgsRole"), UserEntityIdKind.Long),
    };

    private static string Qualify(string tableName) => EntitySchemaRegistry.QualifyTable(tableName);
}

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Allowed <see cref="FgsVehicle.OwnershipType"/> values (see CK_FgsVehicle_OwnershipType).
/// </summary>
public static class VehicleOwnershipTypes
{
    public const string Owned = "Owned";

    public const string Leased = "Leased";

    public const string Rented = "Rented";
}

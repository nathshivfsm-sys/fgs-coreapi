namespace UserService.Application.Common.Security;

/// <summary>
/// Generates Entra-compatible complex passwords (never stored; user resets via invite flow).
/// </summary>
public static class EntraPasswordGenerator
{
    private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Lower = "abcdefghijkmnopqrstuvwxyz";
    private const string Digits = "23456789";
    private const string Special = "@$!%*?&";

    public static string Generate()
    {
        var rng = Random.Shared;
        var required = new[]
        {
            Upper[rng.Next(Upper.Length)],
            Lower[rng.Next(Lower.Length)],
            Digits[rng.Next(Digits.Length)],
            Special[rng.Next(Special.Length)]
        };

        const string pool = Upper + Lower + Digits + Special;
        var rest = Enumerable.Range(0, 24).Select(_ => pool[rng.Next(pool.Length)]).ToArray();
        var chars = required.Concat(rest).OrderBy(_ => rng.Next()).ToArray();
        return new string(chars);
    }
}

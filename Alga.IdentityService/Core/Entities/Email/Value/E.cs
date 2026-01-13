using System.Text.RegularExpressions;

namespace Alga.IdentityService.Core.Entities.Email.Value;

internal class E
{
    private static readonly Regex _regex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly string LogBaseTryAdd = $"ERROR: {typeof(E).FullName}.{nameof(AddValue)}()";

    public static async Task<GuidVO?> GetIdAsync(string value) => string.IsNullOrEmpty(value) ? null : (await new SR().GetIdAsync(value) is not { } v) ? null : (GuidVO)v;

    public static async Task<string?> GetValueAsync(GuidVO? guid) => !guid.HasValue ? null : (await new SR().GetValueAsync(guid.Value) is not { } v) ? null : v.ToString();

    public static async Task<bool> AddValue(Guid guid, string value)
    {
        if (guid == Guid.Empty) throw new ArgumentException($"{LogBaseTryAdd} - the 'guid' cannot be: {Guid.Empty}");
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{LogBaseTryAdd} - the 'value' cannot be: null or empty");
        if (!_regex.IsMatch(value)) throw new ArgumentException($"{LogBaseTryAdd} - the 'value' has invalid format");

        if (!await new SR().SetValueAsync(guid, value)) { Console.WriteLine($"{LogBaseTryAdd} - the 'value' was not added to collection"); return false; }

        return true;
    }
}

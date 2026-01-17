using System.Text.RegularExpressions;

namespace Alga.IdentityService.Core.Entities.Email.Value;

public class E
{
    class SR : Infrastructure.KVA.AdbBase { public SR() : base("email_value_table") { } }

    internal static async Task<GuidVO?> GetIdAsync(string value) => string.IsNullOrEmpty(value) ? null : (await new SR().GetIdAsync(value) is not { } v) ? null : (GuidVO)v;

    internal static async Task<string?> GetVAsync(GuidVO? guid) => !guid.HasValue ? null : (await new SR().GetValueAsync(guid.Value) is not { } v) ? null : v.ToString();

    static readonly string _logAddVAsync = $"Log (error): {typeof(E).FullName}.{nameof(AddVAsync)}()";

    static readonly Regex _regex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static async Task<bool> AddVAsync(Guid guid, string value)
    {
        if (guid == Guid.Empty) throw new ArgumentException($"{_logAddVAsync} - the 'guid' cannot be: {Guid.Empty}");
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{_logAddVAsync} - the 'value' cannot be: null or empty");
        if (!_regex.IsMatch(value)) throw new ArgumentException($"{_logAddVAsync} - the 'value' has invalid format");

        if (!await new SR().SetValueAsync(guid, value)) { Console.WriteLine($"{_logAddVAsync} - the 'value' was not added to collection"); return false; }

        return true;
    }
}

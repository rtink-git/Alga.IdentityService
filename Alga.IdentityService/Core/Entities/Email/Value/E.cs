using System.Text.RegularExpressions;

namespace Alga.IdentityService.Core.Entities.Email.Value;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly byte _valueMaxLength = 254;

    public E(ILogger? logger = null) : base("email_value_table", logger) { }

    //internal static async Task<GuidVO?> GetIdAsync(string value) => string.IsNullOrEmpty(value) ? null : (await new SR().GetIdAsync(value) is not { } v) ? null : (GuidVO)v;

    // internal static async Task<string?> GetVAsync(GuidVO? guid) => !guid.HasValue ? null : (await new SR().GetValueAsync(guid.Value) is not { } v) ? null : v.ToString();

    readonly string _logAddAsync = $"Log (error): {typeof(E).FullName}.{nameof(AddAsync)}()";

    internal async Task<Guid?> AddAsync(string? value)
    {
        if (IsValidateV(value))
        {
            //throw new ArgumentException($"{_logAddAsync} - the 'value' is not validate");
            return null;
        }

        var id = Guid.NewGuid();

        if (!await AddValueAsync(id, value))
        {
            // Console.WriteLine($"{_logAddAsync} - the 'value' was not added to collection"); 
            return null;
        }

        return id;
    }

    internal async Task<Guid?> GetOrAddIdAsync(string? value)
    {
        if (await GetIdAsync(value) is { } id) return id;
        return await AddAsync(value);
    }

    readonly Regex _regex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    bool IsValidateV(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (value.Length > _valueMaxLength) return false;
        if (!_regex.IsMatch(value)) return false;

        return true;
    }
}

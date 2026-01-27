using System.Security.Cryptography;

namespace Alga.IdentityService.Core.Entities.Project.SecretKey;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly byte _valueLength = 64;
    public E(ILogger? logger = null) : base("project_secret_key_table", logger) { }

    readonly string _logAddVAsync = $"Log (error): {typeof(E).FullName}.{nameof(AddAsync)}()";

    internal async Task<(Guid Id, string Value)?> AddAsync()
    {
        var id = Guid.NewGuid();
        var secretKey = GetRandomString();

        if (!await AddValueAsync(id, secretKey)) { Console.WriteLine($"{_logAddVAsync} - the 'value' was not added to collection"); return null; }

        return new(id, secretKey);
    }

    public string GetRandomString()
    {
        byte[] randomBytes = new byte[_valueLength];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes).TrimEnd('=').Substring(0, _valueLength);
    }
}

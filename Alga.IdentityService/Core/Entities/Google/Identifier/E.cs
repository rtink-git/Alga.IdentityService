namespace Alga.IdentityService.Core.Entities.Google.Identifier;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly byte _valueMaxLength = 255;
    readonly ILogger<E>? _logger;
    public E(ILogger<E>? logger = null) : base("google_identifier_table", logger)
    {
        _logger = logger;
        _ = CreateStore_VARCHAR_Unique_Async(_valueMaxLength);
    }

    internal async Task<Guid?> GetOrAddIdAsync(string? value)
    {
        if (await GetIdAsync(value) is { } id) return id;
        return await AddAsync(value);
    }

    readonly string _logAddAsync = $"{nameof(AddAsync)}() - the 'value' was not added to collection";

    internal async Task<Guid?> AddAsync(string? value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{_logAddAsync} - the 'value' cannot be: null or empty");
        if (value.Length > _valueMaxLength) throw new ArgumentException($"{_logAddAsync} - the 'value' length cannot be more then: {_valueMaxLength}");

        var id = Guid.NewGuid();

        if (!await AddValueAsync(id, value))
        {
            _logger?.LogError(_logAddAsync);
            return null;
        }

        return id;
    }
}

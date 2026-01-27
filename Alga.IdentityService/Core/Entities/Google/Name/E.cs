namespace Alga.IdentityService.Core.Entities.Google.Name;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly byte _valueMaxLength = 255;
    readonly ILogger<E>? _logger;
    public E(ILogger<E>? logger = null) : base("google_name_table", logger)
    {
        _logger = logger;
        _ = CreateStore_VARCHAR_Async(_valueMaxLength);
    }

    readonly string _logAddAsync = $"{typeof(E).FullName}.{nameof(AddAsync)}()";

    internal async Task<bool?> AddAsync(Guid? guid, string? value)
    {
        if (guid == null) throw new ArgumentException($"{_logAddAsync} - the 'guid' cannot be: null");
        if (guid == Guid.Empty) throw new ArgumentException($"{_logAddAsync} - the 'guid' cannot be: {Guid.Empty}");
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{_logAddAsync} - the 'value' cannot be: null or empty");
        if (value.Length > _valueMaxLength) throw new ArgumentException($"{_logAddAsync} - the 'value' length cannot be more then: {_valueMaxLength}");
        if (!await AddValueAsync((Guid)guid, value)) { _logger?.LogError($"{_logAddAsync} - the 'value' was not added to collection"); return false; }
        return true;
    }
}

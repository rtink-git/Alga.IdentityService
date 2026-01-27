namespace Alga.IdentityService.Core.Entities.UserEmailUMap;

public class E : Infrastructure.KVA.AdbABase<Guid?>, IE
{
    readonly ILogger<E>? _logger;
    public E(ILogger<E>? logger = null) : base("user_email_umap_table", logger)
    {
        _logger = logger;
        _ = CreateStore_GUID_Unique_Async();
    }

    readonly string _logAddAsync = $"Log (error): {typeof(E).FullName}.{nameof(AddAsync)}()";

    internal async Task<bool> AddAsync(Guid? guid, Guid? value)
    {
        if (guid == null) throw new ArgumentException($"{_logAddAsync} - the 'guid' cannot be: null");
        if (guid == Guid.Empty) throw new ArgumentException($"{_logAddAsync} - the 'guid' cannot be: {Guid.Empty}");
        if (value == null) throw new ArgumentException($"{_logAddAsync} - the 'value' cannot be: null");
        if (value == Guid.Empty) throw new ArgumentException($"{_logAddAsync} - the 'value' cannot be: {Guid.Empty}");
        if (!await AddValueAsync((Guid)guid, value)) { _logger?.LogError($"{_logAddAsync} - the 'value' was not added to collection"); return false; }
        return true;
    }
}
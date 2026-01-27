using System;

namespace Alga.IdentityService.Core.Entities.ProjectUserMap;

public class E : Infrastructure.KVA.AdbABase<Guid?>, IE
{
    readonly ILogger<E>? _logger;
    public E(ILogger<E>? logger = null) : base("project_user_map_table", logger)
    {
        _logger = logger;
        _ = CreateStore_GUID_Async();
    }

    readonly string _logAddVAsync = $"{typeof(E).FullName}.{nameof(AddAsync)}()";

    internal async Task<bool> AddAsync(Guid? guid, Guid? value)
    {
        if (guid == null) throw new ArgumentException($"{_logAddVAsync} - the 'guid' cannot be: null");
        if (guid == Guid.Empty) throw new ArgumentException($"{_logAddVAsync} - the 'guid' cannot be: {Guid.Empty}");
        if (value == null) throw new ArgumentException($"{_logAddVAsync} - the 'value' cannot be: null");
        if (value == Guid.Empty || value == null) throw new ArgumentException($"{_logAddVAsync} - the 'guid' cannot be: {Guid.Empty} or null");
        if (!await AddValueAsync((Guid)guid, value)) { _logger?.LogError($"{_logAddVAsync} - the 'value' was not added to collection"); return false; }
        return true;
    }
}

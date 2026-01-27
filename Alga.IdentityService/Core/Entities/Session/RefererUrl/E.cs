using System;

namespace Alga.IdentityService.Core.Entities.Session.RefererUrl;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly short _valueMinLength = 7;
    readonly short _valueMaxLength = 2048;
    readonly ILogger<E>? _logger;

    public E(ILogger<E>? logger = null) : base("session_referal_url_table", logger)
    {
        _logger = logger;
        _ = CreateStore_VARCHAR_Async(_valueMaxLength);
    }

    readonly string _logAddAsync = $"{nameof(AddAsync)}() - the 'value' was not added to collection";

    internal async Task<bool> AddAsync(Guid? id, string? value)
    {
        if (id == null) return false;
        if (string.IsNullOrEmpty(value)) return false;
        if (value.Length < _valueMinLength) return false;
        if (value.Length > _valueMaxLength) return false;
        if (!Uri.TryCreate(value, UriKind.Absolute, out _)) return false;

        if (!await AddValueAsync((Guid)id, value))
        {
            _logger?.LogError(_logAddAsync);
            return false;
        }

        return true;
    }
}

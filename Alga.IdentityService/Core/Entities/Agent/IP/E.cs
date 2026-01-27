using System.Net;

namespace Alga.IdentityService.Core.Entities.Agent.IP;

public class E : Infrastructure.KVA.AdbABase<string?>, IE
{
    readonly short _valueMaxLength = 50;
    readonly ILogger<E>? _logger;

    public E(ILogger<E>? logger = null) : base("agent_ip_table", logger)
    {
        _logger = logger;
        _ = CreateStore_VARCHAR_Async(_valueMaxLength);
    }

    readonly string _logAddAsync = $"{nameof(AddAsync)}() - the 'value' was not added to collection";

    internal async Task<Guid?> AddAsync(string? value)
    {
        var id = Guid.NewGuid();

        if (!await AddValueAsync(id, value))
        {
            _logger?.LogError(_logAddAsync);

            return null;
        }

        return id;
    }
}

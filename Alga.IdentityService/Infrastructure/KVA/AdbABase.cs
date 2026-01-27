using System;

namespace Alga.IdentityService.Infrastructure.KVA;

public class AdbABase<TV> : Alga.transport.Providers.Postgres.KVABClientBase<TV>
{
    public AdbABase(string storeName, ILogger? logger = null) : base(Context.PostgresConnectionString, storeName, logger)
    {
        // _ = EnsureTableExists();
    }

}

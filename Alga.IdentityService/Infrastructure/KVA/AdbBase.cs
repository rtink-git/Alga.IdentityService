namespace Alga.IdentityService.Infrastructure.KVA;

internal abstract class AdbBase : Alga.transport.Providers.Postgres.KVAClientBase { public AdbBase(string storeName) : base(Context.Res.PostgresConnectionString, storeName) { } }

namespace Alga.IdentityService.Infrastructure.KVA;

public abstract class AdbBase : Alga.transport.Providers.Postgres.KVAClientBase { public AdbBase(string storeName) : base(Context.PostgresConnectionString, storeName) { } }